using System.Reflection;
using System.Text.Json;

namespace LomaPro
{
    public partial class Gallery : ContentPage
    {


        public List<Image_gal> imageList = new List<Image_gal>();
        private string jsonfile;

        public Gallery(string filename)
        {
            Logging.logger.Information("Gallery Page opened");
            this.jsonfile = filename;
            InitializeComponent();
            BackgroundColor = Color.FromArgb("#333333");
            string exepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                imageList = LoadImagesFromJson(exepath + jsonfile);
                DrawImages();
                Logging.logger.Information("Loaded images from json");
            }
            catch
            {
                string galleriesFilepath = System.IO.Path.Combine(exepath, "galleries");
                System.IO.Directory.CreateDirectory(galleriesFilepath);
                SaveJsonToFile("", exepath + jsonfile);
                Logging.logger.Information("No images found created new directory");
                
            }
        }
        static List<Image_gal> LoadImagesFromJson(string path)
        {
            List<Image_gal> imageList = null;
            using (StreamReader stream = new StreamReader(path))
            {
                string serializedData = stream.ReadToEnd();
                imageList = JsonSerializer.Deserialize<List<Image_gal>>(serializedData);

            }

            return imageList;
        }
        public void DrawImages()
        {
            foreach (Image_gal image in imageList)
            {
                image.drawImage(galleryScrollView, ImageExpand, overlay, CloseButton);
            }
            Logging.logger.Information("Drew images");
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
                    foreach (var result in results)
                    {
                        string filePath = result.FullPath;
                        string fileName = Path.GetFileName(filePath);


                        Image_gal image = new Image_gal("Description", filePath, fileName);
                        imageList.Add(image);
                        var options = new JsonSerializerOptions() { WriteIndented = true };
                        string jsonImages = JsonSerializer.Serialize(imageList, options);
                        string exepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                        SaveJsonToFile(jsonImages, exepath + jsonfile);
                        Logging.logger.Information("Added image to json");

                        image.drawImage(galleryScrollView, ImageExpand, overlay, CloseButton);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
                Logging.logger.Error("Error adding image");
            }
        }
        static void SaveJsonToFile(string jsonString, string path)
        {
            using (StreamWriter stream = new StreamWriter(path, append: false))
            {
                stream.WriteLine(jsonString);
            }
            
        }


        private void AddImage_Button_Clicked(object sender, EventArgs e)
        {
            addImage();
            Logging.logger.Information("Add image button clicked");
        }

        private void CloseButton_Clicked(object sender, EventArgs e)
        {
            ImageExpand.IsVisible = false;
            ImageExpand.Source = null;

            overlay.IsVisible = false;
            overlay.Color = Colors.Transparent;

            CloseButton.IsVisible = false;
            Logging.logger.Information("Big image closed");
        }
    }
}