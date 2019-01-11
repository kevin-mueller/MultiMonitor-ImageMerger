using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MutliMonitorImageMerger
{
    class Merger
    {

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, String pvParam, UInt32 fWinIni);
        
       
        const int SetDeskWallpaper = 20;
        const int UpdateIniFile = 0x01;
        const int SendWinIniChange = 0x02;

        private readonly string finalImageFullPath;
        public Merger(string finalImageFullPath)
        {
            this.finalImageFullPath = finalImageFullPath;
        }

        public enum SCALEMODE
        {
            CENTERED,
            STRETCHED,
            OVERSCALED,
            OVERSCALEDANDCENTERED
        }

        public string MergeImagesAccordingToMonitors(Dictionary<string, Image> images, SCALEMODE scaling)
        {
            return CreateBackgroundImage(images, scaling);
        }

        private string CreateBackgroundImage(Dictionary<string, Image> imageFiles, SCALEMODE scaling)
        {
            using (var virtualScreenBitmap = new Bitmap((int)SystemInformation.VirtualScreen.Width, (int)SystemInformation.VirtualScreen.Height))
            {
                using (var virtualScreenGraphic = Graphics.FromImage(virtualScreenBitmap))
                {
                    Dictionary<string, Image> test = new Dictionary<string, Image>();
                    var screens = Screen.AllScreens;


                    int addToCompLeft = 0;
                    int addToCompTop = 0;

                    //Can't draw into negative on bitmap -> move everything thats negative to 0 and everything else by that movement
                    foreach (var item in Screen.AllScreens)
                    {
                        if (item.Bounds.Top < 0)
                            addToCompTop = item.Bounds.Top * -1;
                        if (item.Bounds.Left < 0)
                            addToCompLeft = item.Bounds.Left * -1;
                    }

                    foreach (var screen in screens)
                    {
                        var image = (imageFiles.ContainsKey(screen.DeviceName)) ? imageFiles[screen.DeviceName] : null;

                        var monitorDimensions = screen.Bounds;
                        var width = monitorDimensions.Width;
                        var monitorBitmap = new Bitmap(width, monitorDimensions.Height);
                        var fromImage = Graphics.FromImage(monitorBitmap);
                        fromImage.FillRectangle(SystemBrushes.Desktop, 0, 0, monitorBitmap.Width, monitorBitmap.Height);

                        if (image != null) //This Monitor has an Image -> Draw it as wished
                        {
                            switch (scaling)
                            {
                                case SCALEMODE.CENTERED:
                                    DrawImageCentered(fromImage, image, new Rectangle(0, 0, monitorBitmap.Width, monitorBitmap.Height));
                                    break;
                                case SCALEMODE.STRETCHED:
                                    DrawImageStretched(fromImage, image, new Rectangle(0, 0, monitorBitmap.Width, monitorBitmap.Height));
                                    break;
                                case SCALEMODE.OVERSCALED:
                                    DrawImageOverScaled(fromImage, image, new Rectangle(0, 0, monitorBitmap.Width, monitorBitmap.Height));
                                    break;
                                case SCALEMODE.OVERSCALEDANDCENTERED:
                                    DrawImageOverScaledAndCentered(fromImage, image, new Rectangle(0, 0, monitorBitmap.Width, monitorBitmap.Height));
                                    break;
                            }
                            
                        }

                        //Can't draw into negative on bitmap -> move everything thats negative to 0 and everything else by that movement
                        var left = monitorDimensions.Left + addToCompLeft;
                        var top = monitorDimensions.Top + addToCompTop;

                        Rectangle rectangle = new Rectangle(left, top, monitorDimensions.Width, monitorDimensions.Height);

                        virtualScreenGraphic.DrawImage(monitorBitmap, rectangle);
                        virtualScreenGraphic.Save();
                    }

                    virtualScreenBitmap.Save(finalImageFullPath, ImageFormat.Png);
                }
            }

            return this.finalImageFullPath;
        }

        public void setWallpaperFromFile(string filename)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            key.SetValue(@"WallpaperStyle", 0.ToString());
            key.SetValue(@"TileWallpaper", 1.ToString());
            SystemParametersInfo(SetDeskWallpaper, 0, filename, UpdateIniFile | SendWinIniChange);
        }

        private void DrawImageOverScaledAndCentered(Graphics g, Image img, Rectangle monitorRec)
        {
            throw (new NotImplementedException());
        }

        private void DrawImageOverScaled(Graphics g, Image img, Rectangle monitorRec)
        {
            g.DrawImageUnscaledAndClipped(img, monitorRec);
        }


        private static void DrawImageStretched(Graphics g, Image img, Rectangle monitorRec)
        {
            g.DrawImage(img, monitorRec);
        }

        private static void DrawImageCentered(Graphics g, Image img, Rectangle monitorRect)
        {
            float heightRatio = (float)monitorRect.Height / (float)img.Height;
            float widthRatio = (float)monitorRect.Width / (float)img.Width;
            int height = monitorRect.Height;
            int width = monitorRect.Width;
            int x = 0;
            int y = 0;

            if (heightRatio > 1f && widthRatio > 1f)
            {
                height = img.Height;
                width = img.Width;
                x = (int)((monitorRect.Width - width) / 2f);
                y = (int)((monitorRect.Height - height) / 2f);
            }
            else
            {
                if (heightRatio < widthRatio)
                {
                    width = (int)(img.Width * heightRatio);
                    height = (int)(img.Height * heightRatio);
                    x = (int)((monitorRect.Width - width) / 2f);
                }
                else
                {
                    width = (int)(img.Width * widthRatio);
                    height = (int)(img.Height * widthRatio);
                    y = (int)((monitorRect.Height - height) / 2f);
                }
            }

            Rectangle rect = new Rectangle(x, y, width, height);
            g.DrawImage(img, rect);
        }
    }
}
