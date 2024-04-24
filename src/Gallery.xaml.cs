namespace LomaPro;

public partial class Gallery : ContentPage
{
	public Gallery()
	{
		InitializeComponent();
        BackgroundColor = Color.FromArgb("#333333");
        Image image = new Image();
        image.Source = "testbild_strand.jpeg";
        galleryStackLayout.Children.Add(image);
    }
}