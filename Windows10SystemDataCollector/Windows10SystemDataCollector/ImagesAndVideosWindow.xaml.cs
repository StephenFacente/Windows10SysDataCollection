using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Windows10SystemDataCollector.DataTypes;

namespace Windows10SystemDataCollector
{
    /// <summary>
    /// Interaction logic for ImagesAndVideosWindow.xaml
    /// </summary>
    public partial class ImagesAndVideosWindow
    {
        //image and video file types
        private string[] _imageTypes = {".jpg", ".png", ".tiff", ".jpeg", ".tif", ".bmp", ".exif", ".gif", ".jif", ".jfif" };
        private string[] _videoTypes = { ".avi", ".mkv", ".flv", ".vob", ".gif", ".mov", ".wmv", ".qt", ".mp4", "..m4v", ".3gp", ".m4p"};

        private List<ImageVideoData> _foundMedia = new List<ImageVideoData>();

        public ImagesAndVideosWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     When the window is loaded start to gather all of the data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //search each attached drive for videos and images
            foreach (var driveInfo in DriveInfo.GetDrives())
            {
                SearchDirectory(new DirectoryInfo(driveInfo.Name));
            }
            MainDataGrid.ItemsSource = _foundMedia;
        }

        /// <summary>
        ///     recursively search the directory and gather all file information that is needed.
        /// </summary>
        /// <param name="di"></param>
        private void SearchDirectory(DirectoryInfo di)
        {
            try
            {
                //for each directory in the current directory search within it
                foreach (var dir in di.GetDirectories())
                {
                    SearchDirectory(dir);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            try
            {
                //foreach file in the current directory, if it is an image or video add it to list
                foreach (var fileInfo in di.GetFiles())
                {
                    if (_imageTypes.Contains(fileInfo.Extension))
                    {
                        _foundMedia.Add(new ImageVideoData
                        {
                            FileExtension = fileInfo.Extension,
                            FilePath = fileInfo.FullName,
                            FileSize = fileInfo.Length/1024 + "KB",
                            FileType = "Image"
                        });
                    }
                    else if (_videoTypes.Contains(fileInfo.Extension))
                    {
                        _foundMedia.Add(new ImageVideoData
                        {
                            FileExtension = fileInfo.Extension,
                            FilePath = fileInfo.FullName,
                            FileSize = fileInfo.Length/1024 + "KB",
                            FileType = "Video"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void MainDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ImageVideoData) e.AddedItems[0]).FileType.Equals("Video"))
            {
                var video = new ImageVideoViewer();
                video.Show();
                video.TheImage.Visibility = Visibility.Hidden;
                video.TheVideo.Source = new Uri(((ImageVideoData)e.AddedItems[0]).FilePath);
            }
            else
            {
                var image = new ImageVideoViewer();
                image.Show();
                image.TheVideo.Visibility = Visibility.Hidden;
                image.TheImage.Source = new BitmapImage(new Uri(((ImageVideoData)e.AddedItems[0]).FilePath));
            }
        }
    }
}
