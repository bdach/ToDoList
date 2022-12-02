using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using ToDoList.App.Persistence;
using ToDoList.DB.Models;

namespace ToDoList.App.ViewModels;

public class TaskGroupPageViewModel
{
    public TaskGroupViewModel Group { get; set; }

    public ReadOnlyObservableCollection<TaskViewModel> AllTasks { get; }
    public ReadOnlyObservableCollection<TaskGroupPageGrouping> GroupedTasks { get; }

    private readonly TaskGroupPagePersistenceManager _persistenceManager;

    private readonly ObservableCollection<TaskViewModel> _allTasks;
    private readonly ObservableCollection<TaskGroupPageGrouping> _groupedTasks;

    public TaskGroupPageViewModel(TaskGroup group, IEnumerable<Task> tasks, TaskGroupPagePersistenceManager persistenceManager)
    {
        Group = new TaskGroupViewModel(group);
        _persistenceManager = persistenceManager;

        _allTasks = new ObservableCollection<TaskViewModel>();
        AllTasks = new ReadOnlyObservableCollection<TaskViewModel>(_allTasks);

        foreach (var task in tasks)
        {
            var viewModel = new TaskViewModel(task, Group);
            viewModel.PropertyChanged += TaskPropertyChanged;
            _allTasks.Add(viewModel);
        }

        _allTasks.CollectionChanged += TaskCollectionChanged;

        _groupedTasks = new ObservableCollection<TaskGroupPageGrouping>();
        GroupedTasks = new ReadOnlyObservableCollection<TaskGroupPageGrouping>(_groupedTasks);
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

        _groupedTasks.Add(new TaskGroupPageGrouping(false, AllTasks.Where(task => !task.Done)));
        _groupedTasks.Add(new TaskGroupPageGrouping(true, AllTasks.Where(task => task.Done)));
    }

    public async System.Threading.Tasks.Task AddTask(TaskViewModel task)
    {
        await _persistenceManager.CreateAsync(task);
        _allTasks.Add(task);
    }

    public async System.Threading.Tasks.Task DeleteTask(TaskViewModel task)
    {
        await _persistenceManager.DeleteAsync(task);
        _allTasks.Remove(task);
    }
}

public class TaskGroupPageGrouping : List<TaskViewModel>
{
    public bool Done { get; }

    public TaskGroupPageGrouping(bool done, IEnumerable<TaskViewModel> items)
        : base(items)
    {
        Done = done;
    }

    public string Header => Done ? "Done" : "To do";
}