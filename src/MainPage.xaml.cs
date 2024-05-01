
namespace LomaPro
{
    public partial class MainPage : ContentPage
    {
        List<VacationCover> vacationCoversList = new List<VacationCover>();
        int x = 0; // gratulation, x your finaly in class level!
        public MainPage()
        {
            InitializeComponent();

            VacationCover cover1 = new VacationCover
            {
                Image_Path = "Resources/Images/testbild_strand.jpeg",
                Location = "Location1",
                Title = "Title1",
                Year = 2014
            };
            VacationCover cover2 = new VacationCover
            {
                Image_Path = "Resources/Images/testbild_strand.jpeg",
                Location = "Location2",
                Title = "Title2",
                Year = 2020
            };
            VacationCover cover3 = new VacationCover
            {
                Image_Path = "Resources/Images/testbild_strand.jpeg",
                Location = "Location3",
                Title = "Title3",
                Year = 2024
            };

            VacationCover cover4 = new VacationCover
            {
                Image_Path = "Resources/Images/testbild_strand.jpeg",
                Location = "Location4",
                Title = "Title4",
                Year = 2025
            };

            VacationCover cover5 = new VacationCover
            {
                Image_Path = "Resources/Images/testbild_strand.jpeg",
                Location = "Location5",
                Title = "Title5",
                Year = 2026
            };

            VacationCover cover6 = new VacationCover
            {
                Image_Path = "Resources/Images/testbild_strand.jpeg",
                Location = "Location6",
                Title = "Title6",
                Year = 2027
            };

            VacationCover cover7 = new VacationCover
            {
                Image_Path = "Resources/Images/testbild_strand.jpeg",
                Location = "Location7",
                Title = "Title7",
                Year = 2028
            };
            vacationCoversList.Add(cover1);
            vacationCoversList.Add(cover2);
            vacationCoversList.Add(cover3);
            vacationCoversList.Add(cover4);
            vacationCoversList.Add(cover5);
            vacationCoversList.Add(cover6);
            vacationCoversList.Add(cover7);
            MakeCover();

        }
        public void OnGalleryClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Gallery());
        }

        private async void AddHolidayButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Add_Holiday());
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

            for (int i = 0; i < 3; i++)
            {
                int index = (i + x + vacationCoversList.Count) % vacationCoversList.Count;
                var cover = vacationCoversList[index];

                var image = new Image { Source = cover.Image_Path };
                var title = new Label { Text = cover.Title, FontSize = 20};
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