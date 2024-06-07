using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace LomaPro
{
    public partial class Rechnung_Page : ContentPage
    {
        // Liste der Gruppen
        private List<Group> groups = new List<Group>();

        public Rechnung_Page()
        {
            InitializeComponent();
        }

        public void AddGroupToList(Group group)
        {
            // Fügen Sie die Gruppe zur Liste hinzu
            groups.Add(group);

            // Aktualisieren Sie die Benutzeroberfläche, um die neue Gruppe anzuzeigen
            UpdateUI();
        }

        private void UpdateUI()
        {
            // Leeren Sie das StackLayout
            GroupStackLayout.Children.Clear();

            // Fügen Sie für jede Gruppe in der Liste ein Label zum StackLayout hinzu
            foreach (var group in groups)
            {
                var frame = new Frame { BorderColor = Microsoft.Maui.Graphics.Colors.LightGray, CornerRadius = 5, Padding = 10, Margin = 10 };
                var grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                var nameLabel = new Label { Text = group.Name };
                var sizeLabel = new Label { Text = $"Personen: {group.Size}, Zu zahlender Betrag: {group.HasToPay}€", HorizontalOptions = LayoutOptions.End };

                grid.Children.Add(nameLabel);
                Grid.SetColumn(nameLabel, 0);

                grid.Children.Add(sizeLabel);
                Grid.SetColumn(sizeLabel, 1);

                frame.Content = grid;

                GroupStackLayout.Children.Add(frame);
            }
        }


        public async void Add_Group(object sender, EventArgs e)
        {
            // Erstellen Sie eine neue Seite, auf der der Benutzer die Details der Gruppe eingeben kann
            var newGroupPage = new NewGroupPage();

            // Fügen Sie einen Event-Handler hinzu, der aufgerufen wird, wenn die Gruppendetails eingegeben wurden
            newGroupPage.GroupAdded += AddGroupToList;

            // Öffnen Sie die neue Seite
            await Navigation.PushAsync(newGroupPage);
        }
        public async void Add_Bill(object sender, EventArgs e)
        {
            var newBillPage = new NewBillPage();
            newBillPage.UpdateUI(groups);
            newBillPage.Disappearing += async (s, args) =>
            {
                groups = newBillPage.groups;
                UpdateUI();
            };
            await Navigation.PushAsync(newBillPage);
        }
    }
}
