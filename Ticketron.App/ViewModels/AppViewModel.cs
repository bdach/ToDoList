using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Ticketron.App.Persistence;
using Ticketron.DB.Models;

namespace Ticketron.App.ViewModels;

public class AppViewModel : ObservableObject
{
    /// <summary>
    /// Contains all available task groups.
    /// </summary>
    public ObservableCollection<TaskGroupViewModel> TaskGroups { get; } = new ObservableCollection<TaskGroupViewModel>();

    private TaskLogEntryViewModel? _currentLogEntry;

    /// <summary>
    /// Exposes the app-global current log entry.
    /// A log entry being present here indicates that the user is working on a task
    /// and therefore should not be able to start another one.
    /// </summary>
    public TaskLogEntryViewModel? CurrentLogEntry
    {
        get => _currentLogEntry;
        set => SetProperty(ref _currentLogEntry, value);
    }

    private readonly AppStatePersistenceManager _persistenceManager;
    private readonly object _syncRoot = new object();

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

    public async System.Threading.Tasks.Task StartWorkingOnAsync(TaskViewModel task)
    {
        TaskLogEntryViewModel logEntry;

        lock (_syncRoot)
        {
            if (CurrentLogEntry != null)
                throw new InvalidOperationException(
                    "Cannot start working on a task while another is being worked on already");

            logEntry = new TaskLogEntryViewModel(task);
            CurrentLogEntry = logEntry;
        }

        await _persistenceManager.CreateAsync(logEntry);
    }

    public async System.Threading.Tasks.Task EndWorkingOnCurrentTaskAsync()
    {
        TaskLogEntryViewModel logEntry;

        lock (_syncRoot)
        {
            if (CurrentLogEntry == null)
                return;

            logEntry = CurrentLogEntry;
            CurrentLogEntry = null;
        }

        logEntry.End = DateTime.Now;
        await _persistenceManager.UpdateAsync(logEntry);
    }
}