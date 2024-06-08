using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Text.Json;
using System.Reflection;
using System.IO;

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
                Logging.logger.Information("Loading covers from JSON file.");
                vacationCoversList = LoadCoversFromJson(exepath + "/covers/covers.json");
                MakeCover();
            }
            catch (Exception ex)
            {
                Logging.logger.Error(ex, "Failed to load covers, making new file");
                string coverFilepath = System.IO.Path.Combine(exepath, "covers");
                System.IO.Directory.CreateDirectory(coverFilepath);
                SaveJsonToFile("", exepath + "/covers/covers.json");
            }
        }

        static List<VacationCover> LoadCoversFromJson(string path)
        {
            List<VacationCover> covers = null;
            try
            {
                using (StreamReader stream = new StreamReader(path))
                {
                    string serializedData = stream.ReadToEnd();
                    covers = JsonSerializer.Deserialize<List<VacationCover>>(serializedData);
                    Logging.logger.Information("Loaded covers from JSON.", covers.Count);
                }
            }
            catch (Exception ex)
            {
                Logging.logger.Error(ex, "Error loading covers from JSON file.");
            }
            return covers;
        }

        private void MakeCover()
        {
            try
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
                }
            }
            catch (Exception ex)
            {
                Logging.logger.Error(ex, "Error creating cover in UI.");
            }
        }

        private void LeftButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (x > 0)
                {
                    x--;
                    MakeCover();
                }
            }
            catch (Exception ex)
            {
                Logging.logger.Error(ex, "Error handling left button click.");
            }
        }

        private void RightButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (x < vacationCoversList.Count - 1)
                {
                    x++;
                    MakeCover();
                }
            }
            catch (Exception ex)
            {
                Logging.logger.Error(ex, "Error handling right button click.");
            }
        }

        private void DeleteHolidayButtonClicked(object sender, EventArgs e)
        {
            try
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
                    }

                    vacationCoversList.RemoveAt(x);
                    if (x >= vacationCoversList.Count && x > 0)
                    {
                        x = vacationCoversList.Count - 1;
                    }

                    MakeCover();
                }
            }
            catch (Exception ex)
            {
                Logging.logger.Error(ex, "Error deleting holiday.");
            }
        }

        private async void AddHolidayButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var addHolidayPage = new Rechnung_Page();
                await Navigation.PushAsync(addHolidayPage);
            }
            catch (Exception ex)
            {
                Logging.logger.Error(ex, "Error getting to the Holiday page.");
            }
        }

        public static string CleanFileName(string input)
        {
            try
            {
                string invalidChars = new string(System.IO.Path.GetInvalidFileNameChars());
                string pattern = $"[{Regex.Escape(invalidChars)}]";
                string result = Regex.Replace(input, pattern, "");
                result = result.Replace(" ", "_");
                return result;
            }
            catch (Exception ex)
            {
                Logging.logger.Error(ex, "Error getting clean file name.");
                throw;
            }
        }

        static void SaveJsonToFile(string jsonString, string path)
        {
            try
            {
                using (StreamWriter stream = new StreamWriter(path, append: false))
                {
                    stream.WriteLine(jsonString);
                }
                Logging.logger.Information($"JSON saved successfully to {path}.");
            }
            catch (Exception ex)
            {
                Logging.logger.Error(ex, $"Error saving JSON to {path}.");
            }
        }
    }
}
