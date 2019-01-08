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
            //Here's an example:
            
            //Create the List. ( Name of your Monitor, Image )
            Dictionary<string, Image> imagesWithMonitor = new Dictionary<string, Image>();
            imagesWithMonitor.Add(Screen.PrimaryScreen.DeviceName, Image.FromFile(@"C:\Users\Kevin\Pictures\WallpaperChanger\wallhaven-93402.jpg"));

            //Create the Object. The attribute is going to be your final image.
            Merger merger = new Merger(@"C:\Users\Kevin\Desktop\final.png");

            //your final image Path gets returned
            string finalImage = merger.MergeImagesAccordingToMonitors(imagesWithMonitor, Merger.SCALEMODE.STRETCHED);

            //Additionaly you can set (any) image as Wallpaper using this Method.
            merger.setWallpaperFromFile(finalImage);
        }
    }
}
