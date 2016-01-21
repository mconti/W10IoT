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

namespace conti.maurizio.RSSClient2016
{
    /// <summary>
    /// Pagina vuota che può essere utilizzata autonomamente oppure esplorata all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DispatcherTimer timer { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //string url1 = "http://www.ilmattino.it/rss/home.xml";
            //try
            //{
            //    Sorgente s1 = new Sorgente(url1);
            //    lvItems1.ItemsSource = s1.Items;
            //    txtUpdate1.Text = url1 + " - " + DateTime.Now.ToUniversalTime();
            //}
            //catch
            //{
            //    txtUpdate1.Text = url1 + " non disponibile";
            //}

            //string url2 = "http://www.corriere.it/rss/homepage_innovazione.xml";
            //try
            //{
            //    Sorgente s2 = new Sorgente(url2);
            //    lvItems2.ItemsSource = s2.Items;
            //    txtUpdate2.Text = url2 + " non disponibile";
            //}
            //catch { }

            //timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromMilliseconds(2000);
            //timer.Tick += Timer_Tick1;
            //timer.Start();
        }

        private void Timer_Tick1(object sender, object e)
        {
            timer.Stop();
            btnUpdate_Click(sender, null);
            timer.Start();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            //string url = "https://news.google.it/?output=rss";
            //string url = "http://www.ilmattino.it/rss/home.xml";

            //btnUpdate.IsEnabled = false;
            //txtUpdate.Text = "updating...";

            //Sorgente s = new Sorgente(url);
            //lvItems.ItemsSource = s.Items;

            //txtUpdate.Text = url + " - " + DateTime.Now.ToUniversalTime();
            //+ " " + DateTime.Now.ToLongTimeString();
            //btnUpdate.IsEnabled = true;
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pivot p = sender as Pivot;
            if( p != null )
            {
                PivotItem pi = p.SelectedItem as PivotItem;
                switch(p.SelectedIndex)
                {
                    case 0:
                        string url1 = "http://www.ilmattino.it/rss/home.xml";
                        try
                        {
                            Sorgente s1 = new Sorgente(url1);
                            lvItems1.ItemsSource = s1.Items;
                            txtUpdate1.Text = url1 + " - " + DateTime.Now.ToUniversalTime();
                        }
                        catch
                        {
                            txtUpdate1.Text = url1 + " non disponibile";
                        }
                        break;

                    case 1:
                        string url2 = "http://www.corriere.it/rss/homepage_innovazione.xml";
                        try
                        {
                            Sorgente s2 = new Sorgente(url2);
                            lvItems2.ItemsSource = s2.Items;
                            txtUpdate2.Text = url2 + " - " + DateTime.Now.ToUniversalTime();
                        }
                        catch { txtUpdate2.Text = url2 + " non disponibile"; }
                        break;

                }
            }
        }
    }
}
