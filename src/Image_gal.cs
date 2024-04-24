using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LomaPro
{
    public class Image_gal
    {
        public string Imagedescription;
        public string Imagepath;
        public string ImageName;
        private int width = 200;
        private int height = 200;
        //public string date;
        public Image_gal(string imagedescription, string imagepath, string imagename)
        {
            Imagedescription = imagedescription;
            Imagepath = imagepath;
            ImageName = imagename;
        }
        public void drawImage(StackLayout galleryStackLayout)
        {
            Image image = new Image();
            image.Source = "testbild_strand.jpeg";
            galleryStackLayout.Children.Add(image);
        }
    }
    
}
