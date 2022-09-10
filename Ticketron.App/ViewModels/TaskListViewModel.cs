using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Ticketron.App.Persistence;
using Ticketron.DB.Models;

namespace Ticketron.App.ViewModels;

public class TaskListViewModel
{
    public TaskGroupViewModel Group { get; set; }

    public ObservableCollection<TaskViewModel> AllTasks { get; }
    public ObservableCollection<TaskGrouping> GroupedTasks { get; }

    private readonly TaskListPersistenceManager _persistenceManager;

    public TaskListViewModel(TaskGroup group, IEnumerable<Task> tasks, TaskListPersistenceManager persistenceManager)
    {
        Group = new TaskGroupViewModel(group);
        _persistenceManager = persistenceManager;

        AllTasks = new ObservableCollection<TaskViewModel>();
        foreach (var task in tasks)
        {
            var viewModel = new TaskViewModel(task);
            viewModel.PropertyChanged += TaskPropertyChanged;
            AllTasks.Add(viewModel);
        }

        AllTasks.CollectionChanged += TaskCollectionChanged;

        GroupedTasks = new ObservableCollection<TaskGrouping>();
        RegroupTasks();
    }

    private async void TaskPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is not TaskViewModel taskViewModel)
            return;

        await _persistenceManager.UpdateAsync(taskViewModel);

        if (e.PropertyName == nameof(Task.Done))
            RegroupTasks();
    }

    private void TaskCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (var item in e.NewItems!.Cast<TaskViewModel>())
                    item.PropertyChanged += TaskPropertyChanged;
                break;

            case NotifyCollectionChangedAction.Remove:
                foreach (var item in e.OldItems!.Cast<TaskViewModel>())
                    item.PropertyChanged -= TaskPropertyChanged;
                break;
        }

        RegroupTasks();
    }

    private void RegroupTasks()
    {
        GroupedTasks.Clear();

        GroupedTasks.Add(new TaskGrouping(false, AllTasks.Where(task => !task.Done)));
        GroupedTasks.Add(new TaskGrouping(true, AllTasks.Where(task => task.Done)));
    }
}

public class TaskGrouping : List<TaskViewModel>
{
    public bool Done { get; }

    public TaskGrouping(bool done, IEnumerable<TaskViewModel> items)
        : base(items)
    {
        Done = done;
    }

    public string Header => Done ? "Done" : "To do";
}