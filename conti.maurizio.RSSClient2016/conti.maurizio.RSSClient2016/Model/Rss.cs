using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// WPF
//using System.Windows.Media.Imaging;

// WinRT
using Windows.UI.Xaml.Media.Imaging;

using System.Xml.Linq;
using System.Net.Http;
using Windows.Data.Xml.Dom;
using System.Xml;
using System.Collections.ObjectModel;
using System.IO;

namespace conti.maurizio.RSSClient2016
{
    public class Item
    {
        public static int id { get; set; }
        public string Titolo { get; set; }
        public BitmapImage Immagine { get; set; }

        public Item(XElement item)
        {
            id++;

            try { Titolo = item.Element("title").Value; } catch { }

            // RSS Standard
            try
            {
                string urlImmagine = item.Element("enclosure").Attribute("url").Value;
                Immagine = new BitmapImage(new Uri(urlImmagine));
            }
            catch
            {
                try
                {
                    string urlImmagine = item.Element("info1").Element("fullimage").Attribute("url").Value;
                    Immagine = new BitmapImage(new Uri(urlImmagine));
                }
                catch
                {
                }
            }
        }
    }

    // Siccome stiamo programmando in modalità async, serve sempre una ObservableCollection
    public class Items : ObservableCollection<Item>
    {
        public int id { get; set; }
        public string Url { get; set; }
        public string Titolo { get; set; }
        public XElement XML { get; set; }

        public Items(string url)
        {
            // In UWP, la programmazione async è di casa
            // Non potendo battezzare il costruttore come metodo async, sono costretto a fare un metodo separato
            GetData(url);
        }

        public async void GetData(string url)
        {
            // Questo try serve perchè i metodi async non possono sollevare eccezioni (se sono void)
            // Ma in questo caso, essendo chiamato da un costruttore, 
            // devono per forza essere void, quindi non c'è pezza!!
            try
            {
                // Nel caso serva una autenticazione...
                //HttpClientHandler handler = new HttpClientHandler() { UseDefaultCredentials = true, AllowAutoRedirect = true };
                //HttpClient client = new HttpClient(handler);

                // Creo un Clinet HTTP per leggere dalla rete...
                HttpClient client = new HttpClient();

                HttpResponseMessage response = await client.GetAsync(url);

                // Forzo una eccezione in caso di errore
                response.EnsureSuccessStatusCode();

                // Se prelevo l'XML come string, appaiono caratteri speciali quindi ...
                //string XMLRaw = await response.Content.ReadAsStringAsync();

                // ... devo leggere tutto byte per byte ...
                byte[] byteData = await response.Content.ReadAsByteArrayAsync();

                // Il metodo Load tiene conto dell'encoding che c'è sulla prima riga dell'XML
                // Per usare Load(), bisogna trasformare il byte[] in uno MemoryStream
                XML = XElement.Load(new MemoryStream(byteData), LoadOptions.None);

                // Metodo alternativo meno corretto perche forza l'encoding a UTF7
                // string XMLRaw = Encoding.UTF7.GetString(byteData);
                // XML = XElement.Parse(XMLRaw);

                // Passo uno a uno gli elementi "item" (in formato xml) per ricavare la notizia
                // e li trasformo in oggetti Item (ci pensa il costruttore a farlo)
                foreach (XElement elemento in XML.Element("channel").Elements("item"))
                    Add(new Item(elemento));

                Titolo = XML.Element("channel").Element("title").Value;
            }
            catch { }

            int a = 0;
        }

        #region Metodi non buoni... tenuti come reference
        public async void GetData2(string url)
        {
            // Entrambi i metodi sollevano una eccezione perchè vogliono un file e non un URL
            // XDocument doc = XDocument.Load(url);
            XElement doc = XElement.Load(url);
            int a = 0;
        }

        public async void GetData1(string url)
        {
            // Non da Errori ma sembra vuoto...
            Windows.Data.Xml.Dom.XmlDocument doc = await Windows.Data.Xml.Dom.XmlDocument.LoadFromUriAsync(new Uri(url));
            var ele = doc.DocumentElement;
            int a = 0;
        }
        #endregion

    }

    public class Sorgente
    {
        public int id { get; set; }
        public string Url { get; set; }
        public Items Items { get; set; }

        public Sorgente() { }
        public Sorgente(string url)
        {
            Url = url;
            Items = new Items( url );
        }
    }
}
