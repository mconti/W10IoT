using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.UserProfile;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento per la pagina vuota è documentato all'indirizzo http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace conti.maurizio._5x.DesktopFromWebApi
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void loa_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = "test_image.jpg";
                Uri uri = new Uri("http://www.ucl.ac.uk/news/news-articles/1213/muscle-fibres-heart.jpg");
                StorageFile file;

                // Elimina il vecchio se esiste.
                try
                {
                    file = await ApplicationData.Current.LocalFolder.GetFileAsync(name);
                    if (file != null)
                        await file.DeleteAsync();
                }
                catch{}

                // Scarica dal web il file.
                file = await StorageFile.CreateStreamedFileFromUriAsync(name, uri, RandomAccessStreamReference.CreateFromUri(uri));

                // lo copia in locale
                var file2 = await file.CopyAsync(ApplicationData.Current.LocalFolder);

                // lo visualizza
                immagine.Source= new BitmapImage(new Uri(file2.Path));

                // Ci sono due sistemi per settare il desktop differenti tra Phone e PC
                bool risultato;
                if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
                    // Phone
                    risultato = await UserProfilePersonalizationSettings.Current.TrySetLockScreenImageAsync(file2);
                else
                    // PC
                    risultato = await UserProfilePersonalizationSettings.Current.TrySetWallpaperImageAsync(file2);

                if (risultato)
                    VisualizzaRisultato(true, "OK Fatto!");
                else
                    VisualizzaRisultato(false, "Desktop non cambiato!");

            }
            catch (Exception Erore) { VisualizzaRisultato(false, Erore.Message); }
        }


        private static void VisualizzaRisultato( bool stato, string msg )
        {
            var curentView = ApplicationView.GetForCurrentView();
            if (curentView != null)
            {
                curentView.Title = msg;

                if (stato)
                {
                    curentView.TitleBar.ButtonBackgroundColor = Colors.DarkBlue;
                    curentView.TitleBar.ButtonForegroundColor = Colors.White;
                    curentView.TitleBar.BackgroundColor = Colors.Blue;
                    curentView.TitleBar.ForegroundColor = Colors.White;
                }
                else
                {
                    curentView.TitleBar.ButtonBackgroundColor = Colors.Yellow;
                    curentView.TitleBar.ButtonForegroundColor = Colors.Black;
                    curentView.TitleBar.BackgroundColor = Colors.Yellow;
                    curentView.TitleBar.ForegroundColor = Colors.Black;
                }
            }
        }
    }
}
