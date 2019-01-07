using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MutliMonitorImageMerger
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, Image> imagesWithMonitor = new Dictionary<string, Image>();
            imagesWithMonitor.Add(Screen.PrimaryScreen.DeviceName, Image.FromFile(@"C:\Users\Kevin\Pictures\WallpaperChanger\wallhaven-93402.jpg"));

            Merger merger = new Merger(@"C:\Users\Kevin\Desktop\final.png");
            string finalImage = merger.MergeImagesAccordingToMonitors(imagesWithMonitor);


            merger.setWallpaperFromFile(finalImage);
        }
    }
}
