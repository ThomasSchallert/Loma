using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace LomaPro
{
    public class Image_gal
    {
        public double totalw = 0;
        public string Imagedescription;
        public string Imagepath;
        public string ImageName;
        private int height = 200;

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
            image.Margin = new Thickness(5, 5, 0, 0);

            FlexLayout flexLayout = galleryScrollView.Content as FlexLayout;

            if (flexLayout == null)
            {
                flexLayout = new FlexLayout();
                flexLayout.AlignItems = FlexAlignItems.Start;
                flexLayout.Wrap = FlexWrap.Wrap;
                galleryScrollView.Content = flexLayout;
            }

            flexLayout.Children.Add(image);
        }
    }
}
