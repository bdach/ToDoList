using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using ToDoList.App.ViewModels;
using ToDoList.App.Views.DailyLog;
using ToDoList.App.Views.Settings;
using ToDoList.App.Views.Tasks;

namespace ToDoList.App.Views
{
    public sealed partial class AppNavigationView : UserControl
    {
        public ObservableCollection<AppNavigationItem> NavigationItems { get; } = new ObservableCollection<AppNavigationItem>();

        public AppNavigationView()
        {
            this.InitializeComponent();

            NavigationItems.Add(new TopLevelNavigationItem("Today", Symbol.GoToToday, typeof(TodayPage)));
            NavigationItems.Add(new TopLevelNavigationItem("Daily log", Symbol.Clock, typeof(DailyLogPage)));
            NavigationItems.Add(new Separator());
            NavigationItems.Add(new Header { Name = "Lists" });

            RecreateNavigationItems();

            App.Current.State.TaskGroups.CollectionChanged += (_, _) => RecreateNavigationItems();

            NavigationView.SelectedItem = NavigationItems.First();
        }

        private void OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                sender.Header = "Settings";
                ContentFrame.Navigate(typeof(SettingsPage));
                return;
            }

            switch (sender.SelectedItem)
            {
                case TopLevelNavigationItem topLevelItem:
                    sender.Header = topLevelItem.Name;
                    ContentFrame.Navigate(topLevelItem.PageType);
                    break;

                case TaskGroupNavigationItem taskGroupItem:
                    var selectedTaskGroup = taskGroupItem.TaskGroup;
                    sender.Header = $"{selectedTaskGroup.Icon} {selectedTaskGroup.Name}";
                    ContentFrame.Navigate(typeof(TaskGroupPage), selectedTaskGroup.Model.Id);
                    break;
            }
        }

        private void RecreateNavigationItems()
        {
            for (int i = NavigationItems.Count - 1; i > 0; i--)
            {
                if (NavigationItems[i] is TaskGroupNavigationItem)
                    NavigationItems.RemoveAt(i);
            }

            foreach (var taskGroup in App.Current.State.TaskGroups)
                NavigationItems.Add(new TaskGroupNavigationItem(taskGroup));
        }
    }

    public class AppNavigationItem
    {
    }

    public class Separator : AppNavigationItem
    {
    }

    public class TopLevelNavigationItem : AppNavigationItem
    {
        public string Name { get; }
        public Symbol Symbol { get; }
        public Type PageType { get; }

        public TopLevelNavigationItem(string name, Symbol symbol, Type pageType)
        {
            Name = name;
            Symbol = symbol;
            PageType = pageType;
        }
    }

    public class Header : AppNavigationItem
    {
        public string Name { get; init; } = string.Empty;
    }

    public class TaskGroupNavigationItem : AppNavigationItem
    {
        public TaskGroupViewModel TaskGroup { get; }

        public TaskGroupNavigationItem(TaskGroupViewModel taskGroup)
        {
            TaskGroup = taskGroup;
        }
    }

    public class NavigationViewItemTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item)
            => SelectTemplateForItem(item);

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
            => SelectTemplateForItem(item);

        private DataTemplate SelectTemplateForItem(object item) => item switch
        {
            TopLevelNavigationItem => TopLevelItemTemplate,
            Header => HeaderTemplate,
            TaskGroupNavigationItem => TaskGroupTemplate,
            _ => SeparatorTemplate
        };

        private static readonly DataTemplate TopLevelItemTemplate = (DataTemplate)XamlReader.Load(
            @"<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>
                    <NavigationViewItem Content='{Binding Name}'>
                        <NavigationViewItem.Icon>
                            <SymbolIcon Symbol='{Binding Symbol}' />
                        </NavigationViewItem.Icon>
                    </NavigationViewItem>
                </DataTemplate>");

        private static readonly DataTemplate HeaderTemplate = (DataTemplate)XamlReader.Load(
            @"<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>
                    <NavigationViewItemHeader Content='{Binding Name}' />
                </DataTemplate>");

        private static readonly DataTemplate SeparatorTemplate = (DataTemplate)XamlReader.Load(
            @"<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>
                    <NavigationViewItemSeparator />
                </DataTemplate>");

        private static readonly DataTemplate TaskGroupTemplate = (DataTemplate)XamlReader.Load(
            @"<DataTemplate
                    xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>
                    <NavigationViewItem
                        Content='{Binding TaskGroup.Name, Mode=OneWay}'>
                        <NavigationViewItem.Icon>
                            <FontIcon Glyph='{Binding TaskGroup.Icon, Mode=OneWay}' />
                        </NavigationViewItem.Icon>
                    </NavigationViewItem>
                </DataTemplate>");
    }
}
