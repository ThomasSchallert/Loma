using System;
using System.Collections.Generic;
using System.Linq;
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
        }
        public async void OnGalleryClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Gallery());
        }
        private async void AddHolidayButtonClicked(object sender, EventArgs e)
        {
            var addHolidayPage = new Add_Holiday();
            addHolidayPage.Tcs = new TaskCompletionSource<VacationCover>();

            await Navigation.PushAsync(addHolidayPage);

            var result = await addHolidayPage.Tcs.Task;

            if (result != null)
            {
                vacationCoversList.Add(result);
                vacationCoversList = vacationCoversList.OrderBy(cover => cover.Year).ToList();

                // Set x to the index of the added VacationCover
                x = vacationCoversList.IndexOf(result);

                MakeCover();
            }
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
                    Aspect = Aspect.AspectFill,
                    Margin = -10
                };
                var frame = new Frame
                {
                    Content = image,
                    Margin = 10,
                    CornerRadius = 10,
                    Padding = 0,
                    BackgroundColor = Microsoft.Maui.Graphics.Colors.Transparent
                };
                var title = new Label { Text = cover.Title, FontSize = 20, TextColor = Microsoft.Maui.Graphics.Colors.White };
                var year = new Label { Text = cover.Year.ToString(), FontSize = 16, TextColor = Microsoft.Maui.Graphics.Colors.White };
                var location = new Label { Text = cover.Location, FontSize = 16, TextColor = Microsoft.Maui.Graphics.Colors.White };

                var stackLayout = new StackLayout();
                stackLayout.Children.Add(frame);
                stackLayout.Children.Add(title);
                stackLayout.Children.Add(year);
                stackLayout.Children.Add(location);

                ImageStackPanel.Children.Add(stackLayout);
            }
        }



        private void LeftButtonClicked(object sender, EventArgs e)
        {
            if (vacationCoversList.Count > 0)
            {
                x--;
                if (x < 0)
                {
                    x += vacationCoversList.Count;
                }
                MakeCover();
            }
        }

        private void RightButtonClicked(object sender, EventArgs e)
        {
            if (vacationCoversList.Count > 0)
            {
                x++;
                x %= vacationCoversList.Count;
                MakeCover();
            }
        }
    }
}
