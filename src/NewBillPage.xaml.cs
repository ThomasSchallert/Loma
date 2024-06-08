using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace LomaPro;

public partial class NewBillPage : ContentPage
{
    public List<Group> groups = new List<Group>();

    public NewBillPage()
    {
        InitializeComponent();
    }

    public void UpdateUI(List<Group> groups)
    {
        this.groups = groups;
        GroupStackLayout.Children.Clear();

        foreach (var group in groups)
        {
            var frame = new Frame
            {
                BorderColor = Microsoft.Maui.Graphics.Colors.LightGray,
                CornerRadius = 5,
                Padding = 10,
                Margin = 10
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var nameLabel = new Label { Text = group.Name };

            var picker = new Picker
            {
                Title = "Beteiligte Personen",
                ItemsSource = Enumerable.Range(0, group.Size + 1).Select(i => i.ToString()).ToList(),
                HorizontalOptions = LayoutOptions.End,
                SelectedIndex = 0
            };
            group.picker = picker;
            grid.Children.Add(nameLabel);
            Grid.SetColumn(nameLabel, 0);

            grid.Children.Add(picker);
            Grid.SetColumn(picker, 2);

            frame.Content = grid;

            GroupStackLayout.Children.Add(frame);
        }

    }

    private async void OnOkClicked(object sender, EventArgs e)
    {
        double gesamtpreis = 0;
        int howmany = 0;
        try
        {
            gesamtpreis = Convert.ToDouble(BetragEntry.Text);        }
        catch (Exception ex)
        {
            Logging.logger.Error(ex, "Invalid datatype.");
            await DisplayAlert("Error", "Please enter a valid number", "OK");
            return;
        }

        foreach (var group in groups)
        {
            group.SelectedIndex = group.picker.SelectedIndex; // Speichere den ausgewählten Index
            howmany += group.picker.SelectedIndex;
        }

        if (howmany == 0)
        {
            Logging.logger.Warning("No persons selected.");
            await DisplayAlert("Error", "Please select at least one person", "OK");
            return;
        }

        double personhastopay = gesamtpreis / howmany;
        double payed = 0;
        foreach (var group in groups)
        {
            group.HasToPay += Math.Round((group.picker.SelectedIndex) * personhastopay, 2);
            int index = group.picker.SelectedIndex;
            payed += Math.Round((group.picker.SelectedIndex) * personhastopay, 2);
        }

        if (payed != gesamtpreis)
        {
            Random random = new Random();
            groups[random.Next(groups.Count)].HasToPay += gesamtpreis - payed;
            Logging.logger.Warning("Adjustment made for rounding differences.");
        }

        await Navigation.PopAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
