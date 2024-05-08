namespace LomaPro;

public partial class Add_Holiday : ContentPage
{
    public TaskCompletionSource<VacationCover> Tcs { get; set; }

    public Add_Holiday()
    {
        InitializeComponent();
        Tcs = new TaskCompletionSource<VacationCover>();
    }

    void OnAddButtonClicked(object sender, EventArgs e)
    {
        string imagePath = "testbild_strand.jpeg";/*ImagePathEntry.Text;*/
        string location = LocationEntry.Text;
        string title = TitleEntry.Text;
        int year = StartDateEntry.Date.Year;

        VacationCover vacationCover = new VacationCover
        {
            Image_Path = imagePath,
            Location = location,
            Title = title,
            Year = year
        };

        Tcs.SetResult(vacationCover);

        Navigation.PopAsync();
    }

    void OnCancelButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}
