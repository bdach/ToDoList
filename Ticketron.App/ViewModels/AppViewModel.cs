using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Ticketron.App.Persistence;
using Ticketron.DB.Models;

namespace Ticketron.App.ViewModels;

public class AppViewModel
{
    /// <summary>
    /// Contains all available task groups.
    /// </summary>
    public ObservableCollection<TaskGroupViewModel> TaskGroups { get; } = new ObservableCollection<TaskGroupViewModel>();

    private readonly AppStatePersistenceManager _persistenceManager;

    public AppViewModel(ICollection<TaskGroup> taskGroups, AppStatePersistenceManager persistenceManager)
    {
        _persistenceManager = persistenceManager;

        foreach (var taskGroup in taskGroups)
        {
            var viewModel = new TaskGroupViewModel(taskGroup);
            viewModel.PropertyChanged += TaskGroupChanged;
            TaskGroups.Add(viewModel);
        }

        TaskGroups.CollectionChanged += TaskGroupCollectionChanged;
    }

    private async void TaskGroupCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                var newGroups = e.NewItems!.Cast<TaskGroupViewModel>().ToList();

                foreach (var group in newGroups)
                    group.PropertyChanged += TaskGroupChanged;

                await _persistenceManager.CreateAsync(newGroups);
                break;

            case NotifyCollectionChangedAction.Remove:
                var oldGroups = e.OldItems!.Cast<TaskGroupViewModel>().ToList();

                foreach (var group in oldGroups)
                    group.PropertyChanged -= TaskGroupChanged;

                await _persistenceManager.DeleteAsync(oldGroups);
                break;
        }
    }

    private void TaskGroupChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is not TaskGroupViewModel taskGroup)
            return;

        _persistenceManager.UpdateAsync(taskGroup);
    }
}