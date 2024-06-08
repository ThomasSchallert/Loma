using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Text.Json;
using System.Reflection;


namespace LomaPro
{
    public partial class MainPage : ContentPage
    {

        private int x = 0;
        private List<VacationCover> vacationCoversList = new List<VacationCover>();

        public MainPage()
        {
            InitializeComponent();
            string exepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                vacationCoversList = LoadCoversFromJson(exepath + "/covers/covers.json");
                MakeCover();
            }
            catch
            {
                string coverFilepath = System.IO.Path.Combine(exepath, "covers");
                System.IO.Directory.CreateDirectory(coverFilepath);
                SaveJsonToFile("", exepath + "/covers/covers.json");
                Logging.logger.Information("No covers found created new directory");
            }

        }


        static List<VacationCover> LoadCoversFromJson(string path)
        {
            List<VacationCover> covers = null;
            using (StreamReader stream = new StreamReader(path))
            {
                string serializedData = stream.ReadToEnd();
                covers = JsonSerializer.Deserialize<List<VacationCover>>(serializedData);
            }
            Logging.logger.Information("Loaded covers from json");

            return covers;
        }

        private void MakeCover()
        {
            ImageStackPanel.Children.Clear();

            if (vacationCoversList.Count > 0)
            {
                var cover = vacationCoversList[x];
                string jsonvacationcovers = JsonSerializer.Serialize(vacationCoversList);
                string exepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                SaveJsonToFile(jsonvacationcovers, exepath + "/covers/covers.json");

                var image = new Image
                {
                    Source = cover.Image_Path,
                    Aspect = Aspect.AspectFit,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    BackgroundColor = Microsoft.Maui.Graphics.Colors.Transparent
                };
                var frame = new Frame
                {
                    Content = image,
                    Margin = 10,
                    CornerRadius = 10,
                    Padding = 0,
                    BackgroundColor = Microsoft.Maui.Graphics.Colors.Transparent,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };

                var title = new Label { Text = cover.Title, FontSize = 20, TextColor = Microsoft.Maui.Graphics.Colors.White };
                var year = new Label { Text = cover.StartDate.ToShortDateString() + " - " + cover.EndDate.ToShortDateString(), FontSize = 16, TextColor = Microsoft.Maui.Graphics.Colors.White };
                var location = new Label { Text = cover.Location, FontSize = 16, TextColor = Microsoft.Maui.Graphics.Colors.White };

                var stackLayout = new StackLayout
                {
                    Children = { frame, title, year, location },
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += async (s, e) =>
                {
                    string filename = CleanFileName(cover.Title + "_" + cover.StartDate.ToShortDateString() + " - " + cover.EndDate.ToShortDateString() + "_" + cover.Location);
                    var galleryPage = new Gallery("/galleries/" + filename + ".json");
                    await Navigation.PushAsync(galleryPage);
                };
                stackLayout.GestureRecognizers.Add(tapGestureRecognizer);

                ImageStackPanel.Children.Add(stackLayout);
                Logging.logger.Information("Cover created");
            }
        }



        private void LeftButtonClicked(object sender, EventArgs e)
        {
            if (x > 0)
            {
                x--;
                MakeCover();
                Logging.logger.Information("Left button clicked");
            }
        }

        private void RightButtonClicked(object sender, EventArgs e)
        {
            if (x < vacationCoversList.Count - 1)
            {
                x++;
                MakeCover();
                Logging.logger.Information("Right button clicked");
            }

        }

        private void DeleteHolidayButtonClicked(object sender, EventArgs e)
        {
            if (vacationCoversList.Count > 0)
            {
                var cover = vacationCoversList[x];
                string filename = CleanFileName(cover.Title + "_" + cover.StartDate.ToShortDateString() + " - " + cover.EndDate.ToShortDateString() + "_" + cover.Location);
                string exepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string jsonFilePath = exepath + "/galleries/" + filename + ".json";

                if (File.Exists(jsonFilePath))
                {
                    File.Delete(jsonFilePath);
                    Logging.logger.Information("Gallery deleted");
                }

                vacationCoversList.RemoveAt(x);
                if (x >= vacationCoversList.Count && x > 0)
                {
                    x = vacationCoversList.Count - 1;
                    Logging.logger.Information("Index changed");
                }

                MakeCover();
                Logging.logger.Information("Cover deleted");
            }
        }


        //private async void AddHolidayButtonClicked(object sender, EventArgs e)
        //    {
        //        var addHolidayPage = new Add_Holiday();
        //        await Navigation.PushAsync(addHolidayPage);
        //        var result = await addHolidayPage.Tcs.Task;
        //        vacationCoversList.Add(result);
        //        MakeCover();
        //    }
        private async void AddHolidayButtonClicked(object sender, EventArgs e)
        {
            var addHolidayPage = new Rechnung_Page();
            await Navigation.PushAsync(addHolidayPage);
        }

        public static string CleanFileName(string input)
        {
            string invalidChars = new string(System.IO.Path.GetInvalidFileNameChars());
            string pattern = $"[{Regex.Escape(invalidChars)}]";
            string result = Regex.Replace(input, pattern, "");
            result = result.Replace(" ", "_");
            Logging.logger.Information("Filename cleaned");
            return result;
        }
        static void SaveJsonToFile(string jsonString, string path)
        {
            using (StreamWriter stream = new StreamWriter(path, append: false))
            {
                stream.WriteLine(jsonString);
            }
            Logging.logger.Information("Saved json to file");
        }

    }
}