namespace LomaPro
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        public void OnGalleryClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Gallery());
        }

        private async void AddHolidayButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Add_Holiday());
        }


    }

}
