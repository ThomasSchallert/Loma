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
            FlexLayout flexLayout = galleryScrollView.Content as FlexLayout;

            if (flexLayout == null)
            {
                flexLayout = new FlexLayout();
                flexLayout.AlignItems = FlexAlignItems.Start;
                flexLayout.Wrap = FlexWrap.Wrap;
                galleryScrollView.Content = flexLayout;
            }

            Frame imageFrame = new Frame();
            imageFrame.WidthRequest = double.NaN;
            imageFrame.HeightRequest = height;
            imageFrame.Margin = new Thickness(5, 5, 0, 0);
            imageFrame.Padding = new Thickness(0);
            imageFrame.CornerRadius = 0; // optional: set corner radius to 0 to remove rounded corners

            // Create the image
            Image image = new Image();
            image.Source = ImageSource.FromFile(Imagepath);

            imageFrame.Content = image;

            image.HorizontalOptions = LayoutOptions.Center;
            image.VerticalOptions = LayoutOptions.Center;
            image.Aspect = Aspect.AspectFit;

            flexLayout.Children.Add(imageFrame);
        }

    }
}
