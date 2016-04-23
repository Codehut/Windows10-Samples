using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ThumbnailSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void picSelect_Click(object sender, RoutedEventArgs e)
        {
            // Pick a photo
            FileOpenPicker openPicker = new FileOpenPicker();
            foreach (string extension in FileExtensions.Image)
            {
                openPicker.FileTypeFilter.Add(extension);
            }

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {

                const uint size = 200; //Send your required size
                using (StorageItemThumbnail thumbnail = await file.GetThumbnailAsync(ThumbnailMode.PicturesView, size, ThumbnailOptions.UseCurrentScale))
                {
                    if (thumbnail != null)
                    {
                        //Prepare thumbnail to display
                        BitmapImage bitmapImage = new BitmapImage();

                        bitmapImage.SetSource(thumbnail);
                        picThumbnailHolder.Source = bitmapImage;

                        picthumbnailInfo.Text = String.Format("ThumbnailMode.{0}\n"
                                                       + "File used: {1}\n"
                                                       + "Requested size: {2}\n"
                                                       + "Returned size: {3}x{4}",
                                                       ThumbnailMode.PicturesView.ToString(),
                                                       file.Name,
                                                       size,
                                                       thumbnail.OriginalWidth,
                                                       thumbnail.OriginalHeight);
                    }
                }
            }

            //Handle null cases as per your need

        }

        private async void musicSelect_Click(object sender, RoutedEventArgs e)
        {
            // Pick a music file
            FileOpenPicker openPicker = new FileOpenPicker();
            foreach (string extension in FileExtensions.Music)
            {
                openPicker.FileTypeFilter.Add(extension);
            }

            StorageFile file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                uint size = 100; //Give ur size
                using (StorageItemThumbnail thumbnail = await file.GetThumbnailAsync(ThumbnailMode.MusicView, size, ThumbnailOptions.ResizeThumbnail))
                {
                    // Also verify the type is ThumbnailType.Image (album art) instead of ThumbnailType.Icon
                    // (which may be returned as a fallback if the file does not provide album art)
                    if (thumbnail != null && thumbnail.Type == ThumbnailType.Image)
                    {
                        // Display the thumbnail
                        BitmapImage bitmapImage = new BitmapImage();

                        bitmapImage.SetSource(thumbnail);
                        musicThumbnailHolder.Source = bitmapImage;

                        musicthumbnailInfo.Text = String.Format("ThumbnailMode.{0}\n"
                                                       + "File used: {1}\n"
                                                       + "Requested size: {2}\n"
                                                       + "Returned size: {3}x{4}",
                                                       ThumbnailMode.PicturesView.ToString(),
                                                       file.Name,
                                                       size,
                                                       thumbnail.OriginalWidth,
                                                       thumbnail.OriginalHeight);

                    }
                    else
                    {
                        //Handle fallback error if thumbnail is null or not type of image
                    }
                }
            }
            else
            {
                //Handle error
            }
        }
    }
}
