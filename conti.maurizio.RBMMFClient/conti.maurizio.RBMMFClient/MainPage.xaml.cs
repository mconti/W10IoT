using System;
using PubNubMessaging.Core;
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
using Windows.UI.Core;
using Newtonsoft.Json;
using System.Collections.ObjectModel;


// Il modello di elemento per la pagina vuota è documentato all'indirizzo http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace conti.maurizio.RBMMFClient
{
    /// <summary>
    /// Pagina vuota che può essere utilizzata autonomamente oppure esplorata all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Pubnub pubnub;
        ObservableCollection<Ambiente> Campioni;
        string CanaleAmbiente { get; set; } = "Ambiente1";
        string CanaleColore { get; set; } = "Faretto1";

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            pubnub = new Pubnub(
                "pub-c-531543bb-e12b-4db6-9c4b-f9df0fee3067",
                "sub-c-021829ca-e9cb-11e5-8346-0619f8945a4f"
            );

            Campioni = new ObservableCollection<Ambiente>();
            lvData.ItemsSource = Campioni;

            pubnub.Subscribe<string>(CanaleAmbiente, userCallBack, connectCallback, errorCallback);
            pubnub.EnableJsonEncodingForPublish = true;

        }

        private void connectCallback(string obj)
        {
            //Dispatcher.RunAsync(
            //    CoreDispatcherPriority.Normal,
            //    () =>
            //    {
            //        lvLog.Dispatcher.RunAsync(
            //            CoreDispatcherPriority.Normal,
            //            () => lvLog.Items.Add("Connesso: " + obj));
            //    }
            //);
        }

        private void errorCallback(PubnubClientError obj)
        {
            //Dispatcher.RunAsync(
            //    CoreDispatcherPriority.Normal,
            //    () =>
            //    {
            //        lvLog.Dispatcher.RunAsync(
            //            CoreDispatcherPriority.Normal,
            //            () => lvLog.Items.Add("Erore: " + obj.Message));
            //    }
            //);
        }

        private async void userCallBack(string DatiInArrivo)
        {
            try
            {
                await Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                    {
                        List<object> datiDecodificati = pubnub.JsonPluggableLibrary.DeserializeToListOfObject(DatiInArrivo);
                        EON eon = JsonConvert.DeserializeObject<EON>(datiDecodificati[0].ToString());
                        Campioni.Add(eon.eon);
                    }
                );
            }
            catch { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pubnub.EnableJsonEncodingForPublish = true;

            int temperatura, luminosita, rumore;
            int.TryParse(edtTemperatura.Text, out temperatura);
            int.TryParse(edtLuminosita.Text, out luminosita);
            int.TryParse(edtRumore.Text, out rumore);

            EON e1 = new EON();
            Ambiente ambiente = new Ambiente { Luminosita = luminosita, Rumore = rumore, Temperatura = temperatura };
            e1.eon = ambiente;
            pubnub.Publish(CanaleAmbiente, e1, userPublish, userPubError);
            Campioni.Add(ambiente);
            lvData.ItemsSource = Campioni.Reverse().Take(10);

            Colore colore = new Colore { Red = (int)luminosita, Green = (int)rumore, Blue = (int)temperatura };
            pubnub.Publish(CanaleColore, colore, userPublish, userPubError);
        }

        private void userPublish(object obj)
        {
        }

        private void userPubError(PubnubClientError obj)
        {
        }
    }

    /*
                // Event Handler user
                async (string obj) =>
                    await Dispatcher.RunAsync(
                        CoreDispatcherPriority.Normal,
                        () => {
                            List<object> deserializedMessage = pubnub.JsonPluggableLibrary.DeserializeToListOfObject(obj);
                            //string serializedResultMessage = pubnub.JsonPluggableLibrary.SerializeToJsonString(deserializedMessage[0]);
                            Campione campione = JsonConvert.DeserializeObject<Campione>(deserializedMessage[0].ToString());
                            Campioni.Add(campione);
                        }
                    ),

                // Event Handler connection
                async (string obj) =>
                    await lvLog.Dispatcher.RunAsync(
                        CoreDispatcherPriority.Normal,
                        () => lvLog.Items.Add("Connesso: " + obj)
                    ),

                // Event Handler error
                async (PubnubClientError obj) =>
                    await lvLog.Dispatcher.RunAsync(
                        CoreDispatcherPriority.Normal,
                        () => lvLog.Items.Add("Erore: " + obj.Message)
                    )

    */


    public class Ambiente
    {
        public double Temperatura { get; set; }
        public double Luminosita { get; set; }
        public double Rumore { get; set; }
    }

    public class Colore
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
    }

    public class EON
    {
        public Ambiente eon { get; set; }
    }
}
