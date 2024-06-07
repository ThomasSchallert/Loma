using OpenMeteo;
using System.Security.Cryptography.X509Certificates;

namespace LomaPro;

public partial class Add_Holiday : ContentPage
{
    public TaskCompletionSource<VacationCover> Tcs { get; set; }
    private string selectedFilePath; // Add this line

    public Add_Holiday()
    {
        InitializeComponent();
        Tcs = new TaskCompletionSource<VacationCover>();
        //RunAsync(Test, "London");
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
        if(location == null || title == null)
        {
            DisplayAlert("Error", "Please fill all the fields", "OK");
            return;
        }
        else if (startDate > endDate)
        {
            DisplayAlert("Error", "Start date cannot be greater than end date", "OK");
            return;
        }
        else if (imagePath == null)
        {
            DisplayAlert("Error", "Please select an image", "OK");
            return;
        }


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
    //static async void RunAsync(Label test, string loc)
    //{
    //    try
    //    {
    //        OpenMeteo.OpenMeteoClient client = new OpenMeteo.OpenMeteoClient();
    //        WeatherForecast weatherData = await client.QueryAsync(loc);
    //        test.Text = $"Weather in {loc}: {weatherData.Current.Temperature} {weatherData.CurrentUnits.Temperature}";
    //    }
    //    catch
    //    {
            
    //    }
        
    //}


    void OnCancelButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}
