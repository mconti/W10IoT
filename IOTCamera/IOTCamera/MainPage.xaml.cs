using Microsoft.Maker.RemoteWiring;
using Microsoft.Maker.Serial;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento per la pagina vuota è documentato all'indirizzo http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace IOTCamera
{
    /// <summary>
    /// Pagina vuota che può essere utilizzata autonomamente oppure esplorata all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Snapshot(object sender, RoutedEventArgs e)
        {
            try {
                UsbCamera u = new UsbCamera();
                bool b = await u.InitializeAsync();
                if (b)
                {
                    var foto = await u.CapturePhoto();
                    BitmapImage i = new BitmapImage(new Uri(foto.Path));
                    imgFoto.Source = i;
                }
                else
                    await new MessageDialog("Non ho trovato Webcam Win10 compliant collegate...").ShowAsync();


                int a = 0;
            }
            catch(Exception err)
            {
                await new MessageDialog(err.Message).ShowAsync();
            }
        }

        private async void Connect(object sender, RoutedEventArgs e)
        {
            //BluetoothSerial.listAvailableDevicesAsync();
            var elenco = await UsbSerial.listAvailableDevicesAsync();
            foreach( var d in elenco)
            {
                Debug.WriteLine(d.Name);
            }

            BluetoothSerial bluetooth = new BluetoothSerial("FabLab");

            RemoteDevice arduino = new RemoteDevice(bluetooth);
            bluetooth.ConnectionEstablished += Bluetooth_ConnectionEstablished;

            //these parameters don't matter for bluetooth
            bluetooth.begin(0, 0);


            int a = 0;
        }

        private void Bluetooth_ConnectionEstablished()
        {
        }
    }
}
