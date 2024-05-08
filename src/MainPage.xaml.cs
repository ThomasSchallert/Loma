using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;

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
                for (int i = 0; i < 3; i++)
                {
                    int index = (i + x + vacationCoversList.Count) % vacationCoversList.Count;
                    var cover = vacationCoversList[index];

                    var image = new Image { Source = cover.Image_Path };
                    var title = new Label { Text = cover.Title, FontSize = 20 };
                    var year = new Label { Text = cover.Year.ToString(), FontSize = 16 };
                    var location = new Label { Text = cover.Location, FontSize = 16 };

                    var stackLayout = new StackLayout();
                    stackLayout.Children.Add(title);
                    stackLayout.Children.Add(year);
                    stackLayout.Children.Add(image);
                    stackLayout.Children.Add(location);

                    ImageStackPanel.Children.Add(stackLayout);
                }
            }
        }

        private void LeftButtonClicked(object sender, EventArgs e)
        {
            x--;
            if (x < 0)
            {
                x += vacationCoversList.Count;
            }
            MakeCover();
        }

        private void RightButtonClicked(object sender, EventArgs e)
        {
            x++;
            x %= vacationCoversList.Count;
            MakeCover();
        }
    }
}
