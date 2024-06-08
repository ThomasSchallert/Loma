using Microsoft.Maui.Controls;
using Serilog.Core;
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
        private List<Artikel> Articels = new List<Artikel>();

        public Rechnung_Page()
        {
            InitializeComponent();
            Logging.logger.Information("Rechnung Page opened");
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
            // Fügen Sie die Gruppe zur Liste hinzu
            groups.Add(group);
            Logging.logger.Information("Added group to list");

            // Aktualisieren Sie die Benutzeroberfläche, um die neue Gruppe anzuzeigen
            UpdateUI();
        }

        private void UpdateUI()
        {
            // Leeren Sie das StackLayout
            GroupStackLayout.Children.Clear();
            string jsonvacationgroups = JsonSerializer.Serialize(groups);
            string exepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            SaveJsonToFile(jsonvacationgroups, exepath + "/groups/groups.json");

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
            Logging.logger.Information("Updated UI");
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
                if (newBillPage.artikel != null) 
                {
                    Articels.Add(newBillPage.artikel); 
                    Logging.logger.Information("Added Articel to list");
                    }
                
                UpdateUI();
            };
            await Navigation.PushAsync(newBillPage);
        }
        public async void Show_Bill(object sender, EventArgs e)
        {
            var rechnungAnsehenPage = new RechnungAnsehenPage();
            rechnungAnsehenPage.Articels = Articels;
            rechnungAnsehenPage.UpdateUI();
            await Navigation.PushAsync(rechnungAnsehenPage);
        }
        static List<Group> LoadgroupsFromJson(string path)
        {
            List<Group> groups = null;
            using (StreamReader stream = new StreamReader(path))
            {
                string serializedData = stream.ReadToEnd();
                groups = JsonSerializer.Deserialize<List<Group>>(serializedData);
            }
            Logging.logger.Information("Loaded groups from json");

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
    }
}