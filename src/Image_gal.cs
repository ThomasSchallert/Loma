using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace LomaPro
{
    public class Image_gal
    {
        public double totalw = 0;
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
        public void drawImage(ScrollView galleryScrollView)
        {
            Image image = new Image();
            image.Source = Imagepath;
            image.WidthRequest = double.NaN;
            image.HeightRequest = height;
            image.HorizontalOptions = LayoutOptions.Start;
            image.VerticalOptions = LayoutOptions.Start;
            image.Margin = new Thickness(5, 5, 0, 0);

            if (galleryScrollView.Content is StackLayout existingStackLayout && totalw < galleryScrollView.Width)
            {
                existingStackLayout.Children.Add(image);
                totalw += image.Width;                
            }
            else
            {
                // If the ScrollView's Content is not a StackLayout, create a new one and add the image
                StackLayout newStackLayout = new StackLayout();
                newStackLayout.Orientation = StackOrientation.Horizontal;
                newStackLayout.Children.Add(image);
                galleryScrollView.Content = newStackLayout;
                totalw += image.Width;
            }
        }



    }

}
