using System.Reflection;
using System.Text.Json;
namespace LomaPro;

public partial class RechnungAnsehenPage : ContentPage
{
    public List<Artikel> Articels = new List<Artikel>();
    private string Name { get; set; }

    public RechnungAnsehenPage(string name)
    {
        Name = name;
        InitializeComponent();
        Logging.logger.Information("Rechnung Page opened");

    }

    public void UpdateUI()
    {
        // Leeren Sie das StackLayout
        RechnungStackLayout.Children.Clear();

        // Fügen Sie für jede Gruppe in der Liste ein Label zum StackLayout hinzu
        foreach (var artikel in Articels)
        {
            var frame = new Frame { BorderColor = Microsoft.Maui.Graphics.Colors.LightGray, CornerRadius = 5, Padding = 10, Margin = 10 };
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var nameLabel = new Label { Text = artikel.Name };
            var sizeLabel = new Label { Text = $"Zu zahlender Betrag: {artikel.Price}€", HorizontalOptions = LayoutOptions.End };

            grid.Children.Add(nameLabel);
            Grid.SetColumn(nameLabel, 0);

            grid.Children.Add(sizeLabel);
            Grid.SetColumn(sizeLabel, 1);

            frame.Content = grid;

            RechnungStackLayout.Children.Add(frame);

        }
        Logging.logger.Information("Updated UI");
    }



    static void SaveJsonToFile(string jsonString, string path)
    {
        using (StreamWriter stream = new StreamWriter(path, append: false))
        {
            stream.WriteLine(jsonString);
        }
        Logging.logger.Information("Saved Articel to json");
    }
    public async void Rechnung_Schließen(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}

public class Artikel()
{
	public string Name { get; set; }
	public double Price { get; set; }
	public string WhoPayed { get; set; }
}