using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Ticketron.App.Persistence;
using Ticketron.DB.Models;

namespace Ticketron.App.ViewModels;

public class TasksForTodayViewModel
{
    public ReadOnlyObservableCollection<TodayPageTaskGrouping> GroupedTasks { get; }

    private readonly TasksForTodayPersistenceManager _persistenceManager;

    private readonly ObservableCollection<TodayPageTaskGrouping> _groupedTasks;
    private IEnumerable<TaskViewModel> AllTasks => _groupedTasks.SelectMany(group => group);

    public TasksForTodayViewModel(TasksForToday tasksForToday, TasksForTodayPersistenceManager persistenceManager)
    {
        _persistenceManager = persistenceManager;

        _groupedTasks = new ObservableCollection<TodayPageTaskGrouping>();
        GroupedTasks = new ReadOnlyObservableCollection<TodayPageTaskGrouping>(_groupedTasks);

        RegroupTasks(tasksForToday);
    }

    private void RegroupTasks(TasksForToday tasksForToday)
    {
        foreach (var task in AllTasks)
            task.PropertyChanged -= TaskPropertyChanged;

        _groupedTasks.Clear();

        _groupedTasks.Add(
            new TodayPageTaskGrouping(
                "Overdue",
                tasksForToday.OverdueTasks.Select(task => new TaskViewModel(task))));
        _groupedTasks.Add(
            new TodayPageTaskGrouping(
                "Scheduled for today",
                tasksForToday.TasksScheduledForToday.Select(task => new TaskViewModel(task))));
        _groupedTasks.Add(
            new TodayPageTaskGrouping(
                "Done today",
                tasksForToday.TasksDoneToday.Select(task => new TaskViewModel(task))));

        foreach (var task in AllTasks)
            task.PropertyChanged += TaskPropertyChanged;
    }

    private async void TaskPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is not TaskViewModel taskViewModel)
            return;

        await _persistenceManager.UpdateAsync(taskViewModel);

        if (e.PropertyName is nameof(Task.Done) or nameof(Task.ScheduledFor))
        {
            var regroupedTasks = await _persistenceManager.GetTasksForTodayAsync();
            RegroupTasks(regroupedTasks);
        }
    }
}

public class TodayPageTaskGrouping : List<TaskViewModel>
{
    public string Header { get; }

    public TodayPageTaskGrouping(string header, IEnumerable<TaskViewModel> items)
        : base(items)
    {
        Header = header;
    }
}