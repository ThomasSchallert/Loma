namespace LomaPro;

public partial class Add_Holiday : ContentPage
{
    public TaskCompletionSource<VacationCover> Tcs { get; set; }
    private string selectedFilePath; // Add this line

    public Add_Holiday()
    {
        InitializeComponent();
        Tcs = new TaskCompletionSource<VacationCover>();
    }


    async void AddImageBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select an image",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                selectedFilePath = result.FullPath;
                PreviewImage.Source = ImageSource.FromFile(selectedFilePath); // Add this line
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex}");
        }
    }

    void OnAddButtonClicked(object sender, EventArgs e)
    {
        string imagePath = selectedFilePath;
        string location = LocationEntry.Text;
        string title = TitleEntry.Text;
        DateTime startDate = StartDateEntry.Date;
        DateTime endDate = EndDateEntry.Date;

        VacationCover vacationCover = new VacationCover
        {
            Image_Path = imagePath,
            Location = location,
            Title = title,
            StartDate = startDate,
            EndDate = endDate
        };

        Tcs.SetResult(vacationCover);

        Navigation.PopAsync();
    }


    void OnCancelButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}
