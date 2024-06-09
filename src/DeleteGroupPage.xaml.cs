using System.Collections.ObjectModel;

namespace LomaPro;

public partial class DeleteGroupPage : ContentPage
{
    public List<Group> Groups { get; set; }
    public Group SelectedGroup { get; set; }

    public event Action<Group> GroupDeleted;

    public DeleteGroupPage(List<Group> groups)
    {
        InitializeComponent();
        Logging.logger.Information("Delete Group Page opened");
        Groups = groups;
        BindingContext = this;
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }


    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        if (SelectedGroup != null)
        {
            bool confirm = await DisplayAlert("Confirm", $"Do you want to delete {SelectedGroup.Name}?", "Yes", "No");
            if (confirm)
            {
                Groups.Remove(SelectedGroup);
                GroupDeleted?.Invoke(SelectedGroup);
                Logging.logger.Information("Group deleted");
                await Navigation.PopAsync();
            }
        }
        else
        {
            await DisplayAlert("Error", "Please select a group to delete.", "OK");
            Logging.logger.Error("No group selected");
        }

    }
}
