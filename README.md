# MultiMonitor-ImageMerger
This C# Class can merge multiple images into one, according to your monitors resolution and position. Tested with triple Monitor Set-Up. (1x FullHD, 1x 2k, 1x 4k) Great for Multiple Wallpaper use.


## How To Use:
1. Create a Dictionary List with `string, Image` (Image is `System.Drawing.Image`)

   - For the string parameter you enter the __Name__ of your monitor. (You can use  System.Windows.Forms.Screen.AllScreens[index].DeviceName)
   - For the Image parameter you can use `Image.FromFile("yourImage.png");`
  
  
2. After you created your List, you can create the `merger` object. In the constructor you enter the __Full Path__ of the final image, that will be generated.
3. Call the Method `MergeImagesAccordingToMonitors` and pass your list as the attribute. The second attribute defines how the images get drawn on the monitors. (Centered, Streched) It will return the path of the generated image.
4. Additionally you can set this Image as a Wallpaper using the `setWallpaperFromFile` method.


## Example:
1. Create the List:

   `Dictionary<string, Image> imagesWithMonitor = new Dictionary<string, Image>();`
   
   `images.Add(Screen.AllScreens[0].DeviceName, Image.FromFile("image.png"));`
   
   Repeat this step with (in theory) as many images you want.

2. Create the object and call the method:

   `Merger merger = new Merger(@"C:\Users\YourUser\Desktop\final.png");`
   
   `string finalImage = merger.MergeImagesAccordingToMonitors(imagesWithMonitor, Merger.SCALEMODE.STRETCHED);`

3. Additionally set it as a Wallpaper:

   `merger.setWallpaperFromFile(finalImage);`
