using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

namespace ListBox.CustomStyle;

public class MainWindowViewModel : ObservableObject
{
    public ObservableCollection<Item> Items { get; } = new();

    public MainWindowViewModel()
    {
        Items.Add(new Item
        {
            Name = "Reese",
            Technology = "Web Design",
            Description = "Create a new website / To advertise my business services. To self products and other cool things that are really long text.",
            Location = "Clarksville, TN, 37042",
            Reach = "Nationwide",
            NumberOfCredits = 28,
            PostedAt = TimeSpan.FromMinutes(2)
        });
        Items.Add(new Item
        {
            Name = "Pedro",
            Technology = "Web Development",
            Description = "Additional details / A new web site / Small business / Painter / Google / As requested, this text is extra long too.",
            Location = "Dallas, TX, 75228",
            Reach = "Nationwide",
            NumberOfCredits = 24,
            PostedAt = TimeSpan.FromMinutes(13),
            ShowAdditionalDetails = true
        });
        Items.Add(new Item
        {
            Name = "Leighann",
            Technology = "Web Development",
            Description = "Additional details / A new website / Small business / Blog, Online store, Social media awesomeness with cool graphics.",
            Location = "Payette, ID, 83661",
            Reach = "Nationwide",
            NumberOfCredits = 24,
            PostedAt = TimeSpan.FromMinutes(14),
            ShowAdditionalDetails = true,
            ShowAsModified = true
        });
    }
}

public class Item
{
    public string? Name { get; set; }
    public string? Technology { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? Reach { get; set; }
    public int NumberOfCredits { get; set; }
    public TimeSpan PostedAt { get; set; }
    public bool ShowAdditionalDetails { get; set; }
    public bool ShowAsModified { get; set; }
}
