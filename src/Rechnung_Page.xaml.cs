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
        private string Name { get; set; }

        public Rechnung_Page(string name)
        {
            Name = name;
            InitializeComponent();
            Logging.logger.Information("Rechnung Page opened");
            string exepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                groups = LoadgroupsFromJson(exepath + $"/groups/{Name}_group.json");
                UpdateUI();
                Logging.logger.Information("Loaded groups from json");
            }
            catch
            {
                string coverFilepath = System.IO.Path.Combine(exepath, "groups");
                System.IO.Directory.CreateDirectory(coverFilepath);
                SaveJsonToFile("", exepath + $"/groups/{Name}_group.json");
                Logging.logger.Information("No groups found created new directory");
            }
            try
            {

                Articels = LoadarticelsFromJson(exepath + $"/artikel/{Name}_artikel.json");
            }
            catch
            {
                string artikelFilepath = System.IO.Path.Combine(exepath, "artikel");
                System.IO.Directory.CreateDirectory(artikelFilepath);
                SaveJsonToFile("", exepath + $"/artikel/{Name}_artikel.json");
                Logging.logger.Information("No Artikels found created new directory");

            }
        }

        public void AddGroupToList(Group group)
        {
            groups.Add(group);
            Logging.logger.Information("Added group to list");

            // Aktualisieren Sie die Benutzeroberfl‰che, um die neue Gruppe anzuzeigen
            UpdateUI();
        }

        private void UpdateUI()
        {
            GroupStackLayout.Children.Clear();
            string jsonvacationgroups = JsonSerializer.Serialize(groups);
            string exepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            SaveJsonToFile(jsonvacationgroups, exepath + $"/groups/{Name}_group.json");

            foreach (var group in groups)
            {
                var frame = new Frame { BorderColor = Microsoft.Maui.Graphics.Colors.LightGray, CornerRadius = 5, Padding = 10, Margin = 10 };
                var grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                var nameLabel = new Label { Text = group.Name };
                var sizeLabel = new Label { Text = $"Personen: {group.Size}, Zu zahlender Betrag: {group.HasToPay}Ä", HorizontalOptions = LayoutOptions.End };

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
                if (newBillPage.artikel.Name != "" && newBillPage.artikel.Price != 0) 
                {
                    Articels.Add(newBillPage.artikel); 
                    Logging.logger.Information("Added Articel to list");
                    string jsonartikel = JsonSerializer.Serialize(Articels);
                    string exepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    SaveJsonToFile(jsonartikel, exepath + $"/artikel/{Name}_artikel.json");

                }


                UpdateUI();
            };
            await Navigation.PushAsync(newBillPage);
        }
        public async void Delete_Group(object sender, EventArgs e)
        {
            var deleteGroupPage = new DeleteGroupPage(groups);
            deleteGroupPage.Disappearing += (s, args) =>
            {
                groups = deleteGroupPage.Groups;
                UpdateUI();
                string jsonvacationgroups = JsonSerializer.Serialize(groups);
                string exepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                SaveJsonToFile(jsonvacationgroups, exepath + $"/groups/{Name}_group.json");
            };
            await Navigation.PushAsync(deleteGroupPage);
        }

        public async void Delete_Artikel(object sender, EventArgs e)
        {
            var deleteArtikelPage = new DeleteArtikelPage(Articels);
            deleteArtikelPage.Disappearing += (s, args) =>
            {
                if (deleteArtikelPage.Artikels != null)
                {
                    Articels = deleteArtikelPage.Artikels.ToList();
                    UpdateUI();
                    string jsonvacationarticels = JsonSerializer.Serialize(groups);
                    string exepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    SaveJsonToFile(jsonvacationarticels, exepath + $"/artikel/{Name}_artikel.json");
                };
            };
            await Navigation.PushAsync(deleteArtikelPage);
        }


        public async void Show_Bill(object sender, EventArgs e)
        {
            var rechnungAnsehenPage = new RechnungAnsehenPage(Name);
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
            Logging.logger.Information("Saved to json");
        }
        public void calcDebts()
        {
            List<(string groupName, double debt)> debts = new List<(string groupName, double debt)>();

            foreach (var group in groups)
            {
                
            }
        }
        static List<Artikel> LoadarticelsFromJson(string path)
        {
            List<Artikel> Articels = null;
            using (StreamReader stream = new StreamReader(path))
            {
                string serializedData = stream.ReadToEnd();
                Articels = JsonSerializer.Deserialize<List<Artikel>>(serializedData);
            }
            Logging.logger.Information("Loaded Articels from json");

            return Articels;
        }
        public async void Rechnung_Schlieﬂen(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

    }
}