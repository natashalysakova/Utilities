using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
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


namespace Utilities
{
    /// <summary>
    /// Interaction logic for Export.xaml
    /// </summary>
    public partial class Export : Window
    {
        private readonly Check check;

        public Export(Check check)
        {
            this.check = check;
            DataContext = check;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var dialog = new VistaFolderBrowserDialog();
            dialog.Multiselect = false;
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                try
                {
                    var fileName = $"UtilityExport-{check.Date.ToString("ddMMyyyy")}-{DateTime.Now.ToString("ddMMyyyy-HHmmss")}.png";
                    using (Stream fileStream = File.Create(Path.Combine(dialog.SelectedPath, fileName)))
                    {
                        GeneratePng().Save(fileStream);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private PngBitmapEncoder GeneratePng() {
            RenderTargetBitmap renderTargetBitmap = RenderImage();
            PngBitmapEncoder pngImage = new PngBitmapEncoder();
            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            return pngImage;
        }

        private RenderTargetBitmap RenderImage()
        {
            var dpi = 300.0;
            int width = (int) (ForExport.ActualWidth * (dpi / 96));
            int height = (int)(ForExport.ActualHeight * (dpi/ 96));

            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(width, height, dpi, dpi, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(ForExport);
            return renderTargetBitmap;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var img = RenderImage();
            Clipboard.SetImage(img);
        }
    }
}
