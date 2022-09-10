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

    public ReadOnlyObservableCollection<TaskViewModel> AllTasks { get; }
    public ReadOnlyObservableCollection<TaskGrouping> GroupedTasks { get; }

    private readonly TaskListPersistenceManager _persistenceManager;

    private readonly ObservableCollection<TaskViewModel> _allTasks;
    private readonly ObservableCollection<TaskGrouping> _groupedTasks;

    public TaskListViewModel(TaskGroup group, IEnumerable<Task> tasks, TaskListPersistenceManager persistenceManager)
    {
        Group = new TaskGroupViewModel(group);
        _persistenceManager = persistenceManager;

        _allTasks = new ObservableCollection<TaskViewModel>();
        AllTasks = new ReadOnlyObservableCollection<TaskViewModel>(_allTasks);

        foreach (var task in tasks)
        {
            var viewModel = new TaskViewModel(task);
            viewModel.PropertyChanged += TaskPropertyChanged;
            _allTasks.Add(viewModel);
        }

        _allTasks.CollectionChanged += TaskCollectionChanged;

        _groupedTasks = new ObservableCollection<TaskGrouping>();
        GroupedTasks = new ReadOnlyObservableCollection<TaskGrouping>(_groupedTasks);
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
        _groupedTasks.Clear();

        _groupedTasks.Add(new TaskGrouping(false, AllTasks.Where(task => !task.Done)));
        _groupedTasks.Add(new TaskGrouping(true, AllTasks.Where(task => task.Done)));
    }

    public async System.Threading.Tasks.Task AddTask(TaskViewModel task)
    {
        await _persistenceManager.CreateAsync(task);
        _allTasks.Add(task);
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