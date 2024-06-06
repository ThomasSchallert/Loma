using Microsoft.Maui.Controls;
using System;

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
                    DisplayAlert("Error", "Please fill all the fields", "OK");
                    return;
                }
                var groupName = GroupNameEntry.Text;

                var groupSize = int.Parse(GroupSizeEntry.Text);

                var newGroup = new Group { Name = groupName, Size = groupSize };
                GroupAdded?.Invoke(newGroup);
            }
            catch (Exception)
            {
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

        public Picker picker { get; set; }
    }
}
