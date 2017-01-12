
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento per la pagina vuota è documentato all'indirizzo http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace bianchi.conti.figure
{
    // Link Utili

    // Win10 con Raspberry
    // https://github.com/ms-iot

    // UWP per Windows    
    //https://github.com/Microsoft/Windows-universal-samples
    
    // Progetto per vedere la UI
    // https://github.com/Microsoft/Windows-universal-samples/tree/master/Samples/XamlUIBasics


    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            MioMetodo();

            Figura F1 = new Quadrato(10);
            double a = F1.Area();

            // List è un container generico.
            // Può "listare" oggetti di qaualsiasi tipo.
            List<Figura> disegno = new List<Figura>();
            disegno.Add(F1);
            disegno.Add(new Quadrato(20));
            disegno.Add(new Rettangolo(20, 2));
            disegno.Add(new Rettangolo(10, 4));

            foreach ( Figura f in disegno)
            {
                visualizza.Items.Add( $"Nome: {f.Nome}, Lati: {f.Lati}, Area: {f.Area()}" );
            }



        }

        public void MioMetodo()
        {
            //var path = @"MyText.txt";
            //var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            //// acquire file
            //var file = await folder.GetFileAsync("MyText.txt");
            //var readFile = await Windows.Storage.FileIO.ReadLinesAsync(file);
            //foreach (var line in readFile)
            //{
            //    Debug.WriteLine("" + line.Split(';')[0]);
            //}
        }

    }
}
