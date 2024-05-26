using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;


namespace LomaPro
{
    public partial class MainPage : ContentPage
    {
        private List<VacationCover> vacationCoversList = new List<VacationCover>();
        private int x = 0;

        public MainPage()
        {
            InitializeComponent();
            LoadCovers();
        }

        private void LoadCovers()
        {
            // Load your covers here
            // For example:
            // vacationCoversList.Add(new VacationCover { Image_Path = "image1.jpg", Title = "Title1", Year = 2021, Location = "Location1" });
            // vacationCoversList.Add(new VacationCover { Image_Path = "image2.jpg", Title = "Title2", Year = 2022, Location = "Location2" });
        }

        private void MakeCover()
        {
            ImageStackPanel.Children.Clear();

            if (vacationCoversList.Count > 0)
            {
                var cover = vacationCoversList[x];

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
                var year = new Label { Text = cover.Year.ToString(), FontSize = 16, TextColor = Microsoft.Maui.Graphics.Colors.White };
                var location = new Label { Text = cover.Location, FontSize = 16, TextColor = Microsoft.Maui.Graphics.Colors.White };

                var stackLayout = new StackLayout
                {
                    Children = { frame, title, year, location },
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += async (s, e) =>
                {
                    string filename = CleanFileName(cover.Title + "_" + cover.Year + "_" + cover.Location);
                    var galleryPage = new Gallery("/" + filename + ".json");
                    await Navigation.PushAsync(galleryPage);
                };
                stackLayout.GestureRecognizers.Add(tapGestureRecognizer);


                ImageStackPanel.Children.Add(stackLayout);
            }
        }


        private void LeftButtonClicked(object sender, EventArgs e)
        {
            if (x > 0)
            {
                x--;
                MakeCover();
            }
        }

        private void RightButtonClicked(object sender, EventArgs e)
        {
            if (x < vacationCoversList.Count - 1)
            {
                x++;
                MakeCover();
            }
        }

        private async void AddHolidayButtonClicked(object sender, EventArgs e)
        {
            var addHolidayPage = new Add_Holiday();
            await Navigation.PushAsync(addHolidayPage);
            var result = await addHolidayPage.Tcs.Task;
            vacationCoversList.Add(result);
            MakeCover();
        }

        private async void OnGalleryClicked(object sender, EventArgs e)
        {
            var galleryPage = new Gallery("/Gallerysave.json");
            await Navigation.PushAsync(galleryPage);
        }
        public static string CleanFileName(string input)
        {
            string invalidChars = new string(System.IO.Path.GetInvalidFileNameChars());
            string pattern = $"[{Regex.Escape(invalidChars)}]";
            string result = Regex.Replace(input, pattern, "");
            result = result.Replace(" ", "_");
            return result;
        }
    }
}
