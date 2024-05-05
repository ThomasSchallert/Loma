
using Microsoft.Maui.Controls.StyleSheets;

namespace LomaPro
{
    public partial class MainPage : ContentPage
    {
        List<VacationCover> vacationCoversList = new List<VacationCover>();
        int x = 0; // gratulation, x your finaly in class level!
        public MainPage()
        {
            InitializeComponent();

            using (var reader = new StringReader("^contentpage { background-color: lightgray; }"))
            {
                this.Resources.Add(StyleSheet.FromReader(reader));
            }

        }
        public void OnGalleryClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Gallery());
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
                MakeCover();
            }
        }


        public void OnRightButtonClicked(object sender, EventArgs e)
        {
            x++;
            MakeCover();

        }

        public void OnLeftButtonClicked(object sender, EventArgs e)
        {
            x--;
            if (x < 0) x += vacationCoversList.Count;
            MakeCover();
        }

        private void MakeCover()
        {
            ImageStackPanel.Children.Clear();

            int currentYear = DateTime.Now.Year;
            while (true)
            {
                if (x >= (vacationCoversList.Count - 1))
                {
                    break;
                }
                var element = vacationCoversList[x];
                if (element.Year >= currentYear)
                {
                    break;
                }
                x++;
            }

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
    }
}