using System.Reflection;

namespace LomaPro
{
    public partial class Gallery : ContentPage
    {
        public List<Image_gal> imageList = new List<Image_gal>();

        public Gallery()
        {
            InitializeComponent();
            BackgroundColor = Color.FromArgb("#333333");
            LoadImages();
        }
        

        public void LoadImages()
        {

            string imagesFolder = "./";

            if (Directory.Exists(imagesFolder))
            {
                var imageExtensions = new string[] { ".jpeg", ".jpg", ".png", ".gif", ".bmp" };
               
                var imageFiles = Directory.GetFiles(imagesFolder).Where(file => imageExtensions.Contains(Path.GetExtension(file).ToLower()));
                imageFiles = new string[] { "testbild_strand.jpeg" };
                if (galleryScrollView.Content is StackLayout stackLayout)
                {
                stackLayout.Children.Clear();
                }

                foreach (var imagePath in imageFiles)
                {
                    string fileName = Path.GetFileName(imagePath);
                    Image_gal image = new Image_gal("Description", imagePath, fileName);
                    imageList.Add(image);
                }
                foreach (Image_gal image in imageList)
                {
                    image.drawImage(galleryScrollView, ImageExpand, overlay, CloseButton);
                }
            }
        }

        public async void addImage()
        {
            try
            {
                var results = await FilePicker.PickMultipleAsync(new PickOptions
                {
                    PickerTitle = "Select images",
                    FileTypes = FilePickerFileType.Images
                });

                if (results != null)
                {
                    //string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    //string projectDirectory = Directory.GetParent(baseDirectory).Parent.Parent.Parent.Parent.Parent.FullName;
                    //string destinationFolder = Path.Combine(projectDirectory, "Resources", "Images");
                    string destinationFolder = "Resources/Images"; 

                    foreach (var result in results)
                    {
                        string filePath = result.FullPath;
                        string fileName = Path.GetFileName(filePath);
                        string destinationPath = Path.Combine(destinationFolder, fileName);

                        // Kopieren der ausgewählten Datei an den Zielort
                        File.Copy(filePath, destinationPath, overwrite: true);

                        Image_gal image = new Image_gal("Description", destinationPath, fileName);
                        image.drawImage(galleryScrollView, ImageExpand, overlay, CloseButton);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            addImage();
        }

        private void CloseButton_Clicked(object sender, EventArgs e)
        {
            ImageExpand.IsVisible = false;
            ImageExpand.Source = null;

            overlay.IsVisible = false;
            overlay.Color = Colors.Transparent;

            CloseButton.IsVisible = false;
        }
    }
}
