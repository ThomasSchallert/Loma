namespace LomaPro;

public partial class Gallery : ContentPage
{
    public List<Image_gal> imageList = new List<Image_gal>();

	public Gallery()
	{
        InitializeComponent();
        BackgroundColor = Color.FromArgb("#333333");
    }
    public async void addImage()
    {
        try
        {
            var results = await FilePicker.PickMultipleAsync(new PickOptions
            {
                PickerTitle = "Select images"
            });

            if (results != null)
            {
                foreach (var result in results)
                {
                    string filePath = result.FullPath;
                    string fileName = Path.GetFileName(filePath);
                    Image_gal image = new Image_gal("Description", filePath, fileName);
                    image.drawImage(galleryScrollView);
                }
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions
            Console.WriteLine($"Exception: {ex}");
        }
    }
    private void Button_Clicked(object sender, EventArgs e)
    {
        addImage();
    }
}