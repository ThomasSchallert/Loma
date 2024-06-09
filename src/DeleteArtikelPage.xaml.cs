using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LomaPro;

public partial class DeleteArtikelPage : ContentPage
{
    public List<Artikel> Artikels { get; set; }

    private Artikel _selectedArtikel;
    public Artikel SelectedArtikel
    {
        get => _selectedArtikel;
        set
        {
            _selectedArtikel = value;
            OnPropertyChanged();
        }
    }

    public event Action<Artikel> ArtikelDeleted;

    public DeleteArtikelPage(List<Artikel> artikels)
    {
        InitializeComponent();
        Logging.logger.Information("Delete Artikel Page opened");
        Artikels = artikels;
        BindingContext = this;
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
        Logging.logger.Information("Delete Artikel Page closed");
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        if (SelectedArtikel != null)
        {
            bool confirm = await DisplayAlert("Confirm", $"Do you want to delete {SelectedArtikel.Name}?", "Yes", "No");
            if (confirm)
            {
                Artikels.Remove(SelectedArtikel);
                ArtikelDeleted?.Invoke(SelectedArtikel);
                await Navigation.PopAsync();
                Logging.logger.Information("Artikel deleted");
            }
        }
        else
        {
            await DisplayAlert("Error", "Please select an article to delete.", "OK");
            Logging.logger.Error("No article selected");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
