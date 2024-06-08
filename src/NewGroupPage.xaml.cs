using Microsoft.Maui.Controls;
using System;
using System.Text.Json.Serialization;

namespace LomaPro
{
    public partial class NewGroupPage : ContentPage
    {
        public event Action<Group> GroupAdded;

        public NewGroupPage()
        {
            InitializeComponent();
        }

        private void OnAddGroupButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(GroupNameEntry.Text) || string.IsNullOrEmpty(GroupSizeEntry.Text))
                {
                    Logging.logger.Warning("Group name or size entry is empty.");
                    DisplayAlert("Error", "Please fill all the fields", "OK");
                    Logging.logger.Error("Not all fields selected");
                    return;
                }

                var groupName = GroupNameEntry.Text;
                var groupSize = int.Parse(GroupSizeEntry.Text);

                var newGroup = new Group { Name = groupName, Size = groupSize };
                GroupAdded?.Invoke(newGroup);
                Logging.logger.Information("Group added");
            }
            catch (Exception ex)
            {
                Logging.logger.Error(ex, "Error parsing group size.");
                DisplayAlert("Error", "Only enter Numbers", "OK");
                return;
            }

            Navigation.PopAsync();
        }
    }

    public class Group
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public double HasToPay { get; set; }

        [JsonIgnore]
        public Picker picker { get; set; }

        public int SelectedIndex { get; set; }

        public Group()
        {
            Name = "";
            Size = 0;
            HasToPay = 0;
            picker = new Picker();
        }
    }
}
