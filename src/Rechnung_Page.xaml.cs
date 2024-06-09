using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace LomaPro
{
    public partial class Rechnung_Page : ContentPage
    {
        // Liste der Gruppen
        private List<Group> groups = new List<Group>();

        public Rechnung_Page()
        {
            InitializeComponent();
            string exepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {

                groups = LoadgroupsFromJson(exepath + "/groups/groups.json");
                UpdateUI();
                Logging.logger.Information("Loaded groups from json");
            }
            catch
            {
                string coverFilepath = System.IO.Path.Combine(exepath, "groups");
                System.IO.Directory.CreateDirectory(coverFilepath);
                SaveJsonToFile("", exepath + "/groups/groups.json");
                Logging.logger.Information("No groups found created new directory");
                
            }
        }

        public void AddGroupToList(Group group)
        {
            groups.Add(group);
            UpdateUI();
        }

        private void UpdateUI()
        {
            GroupStackLayout.Children.Clear();
            string jsonvacationgroups = JsonSerializer.Serialize(groups);
            string exepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            SaveJsonToFile(jsonvacationgroups, exepath + "/groups/groups.json");

            foreach (var group in groups)
            {
                var frame = new Frame { BorderColor = Microsoft.Maui.Graphics.Colors.LightGray, CornerRadius = 5, Padding = 10, Margin = 10 };
                var grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                var nameLabel = new Label { Text = group.Name };
                var sizeLabel = new Label { Text = $"Personen: {group.Size}, Zu zahlender Betrag: {group.HasToPay}�", HorizontalOptions = LayoutOptions.End };

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
            var newGroupPage = new NewGroupPage();

            newGroupPage.GroupAdded += AddGroupToList;

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
        static List<Group> LoadgroupsFromJson(string path)
        {
            List<Group> groups = null;
            using (StreamReader stream = new StreamReader(path))
            {
                string serializedData = stream.ReadToEnd();
                groups = JsonSerializer.Deserialize<List<Group>>(serializedData);
            }

            return groups;
        }
        static void SaveJsonToFile(string jsonString, string path)
        {
            using (StreamWriter stream = new StreamWriter(path, append: false))
            {
                stream.WriteLine(jsonString);
            }
            Logging.logger.Information("Saved groups to json");
        }
        public void calcDebts()
        {
            List<(string groupName, double debt)> debts = new List<(string groupName, double debt)>();

            foreach (var group in groups)
            {
                
            }
        }

    }
}