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

                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(name);
                if (file != null)
                    await file.DeleteAsync();

                file = await StorageFile.CreateStreamedFileFromUriAsync(name, uri, RandomAccessStreamReference.CreateFromUri(uri));

                // file is readonly, copy to a new location to remove restrictions
                var file2 = await file.CopyAsync(ApplicationData.Current.LocalFolder);

                // try set lockscreen/wallpaper
                if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
                {// Phone
                    var success = await UserProfilePersonalizationSettings.Current.TrySetLockScreenImageAsync(file2);
                }
                else
                {// PC
                    var success = await UserProfilePersonalizationSettings.Current.TrySetWallpaperImageAsync(file2);
                    if (success)
                    {
                        VisualizzaRisultato(success, "OK Fatto!");
                    }
                }
            }
            catch(Exception Erore)
            {
                VisualizzaRisultato(false, Erore.Message);
            }
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
