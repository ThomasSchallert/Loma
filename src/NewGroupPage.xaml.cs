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
            var groupName = GroupNameEntry.Text;
            var groupSize = int.Parse(GroupSizeEntry.Text);

            var newGroup = new Group { Name = groupName, Size = groupSize };

            GroupAdded?.Invoke(newGroup);

            Navigation.PopAsync();
        }
    }

    public class Group
    {
        public string Name { get; set; }
        public int Size { get; set; }
    }
}
