using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;

namespace LomaPro
{
    public class Image_gal
    {
        public double totalw = 0;
        public string Imagedescription { get; set; }
        public string Imagepath { get; set; }
        public string ImageName { get; set; }
        private int height = 200;


        public Image_gal(string imagedescription, string imagepath, string imagename)
        {
            Imagedescription = imagedescription;
            Imagepath = imagepath;
            ImageName = imagename;
            Logging.logger.Information("Image created");
        }

        public void drawImage(ScrollView galleryScrollView, Image expandedimage, VisualElement blurElement, Button closeButton)
        {
            FlexLayout flexLayout = (FlexLayout)galleryScrollView.Content;
            galleryScrollView.ZIndex = 0;

            if (flexLayout == null)
            {
                flexLayout = new FlexLayout();
                flexLayout.AlignItems = FlexAlignItems.Start;
                flexLayout.Wrap = FlexWrap.Wrap;
                galleryScrollView.Content = flexLayout;
            }

            Frame imageFrame = new Frame
            {
                WidthRequest = double.NaN,
                HeightRequest = height,
                Margin = new Thickness(5, 5, 0, 0),
                Padding = new Thickness(0),
                CornerRadius = 0,
                ZIndex = 0
            };

            Image image = new Image
            {
                Source = ImageSource.FromFile(Imagepath),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Aspect = Aspect.AspectFit,
            };

            imageFrame.Content = image;

            imageFrame.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    expandImage(expandedimage, blurElement, closeButton);
                })
            });

            flexLayout.Children.Add(imageFrame);
            Logging.logger.Information("Image added to gallery");
        }

        public void expandImage(Image expandImage, VisualElement blurElement, Button closeButton)
        {
            expandImage.Source = Imagepath;
            expandImage.IsVisible = true;
            expandImage.ZIndex = 4;

            closeButton.IsVisible = true;

            if (blurElement is BoxView boxView)
            {
                blurElement.ZIndex = 2;
                boxView.Color = Colors.Black.MultiplyAlpha((float)0.9);
                boxView.IsVisible = true;
            }
            Logging.logger.Information("Image expanded");

        }

    }
}