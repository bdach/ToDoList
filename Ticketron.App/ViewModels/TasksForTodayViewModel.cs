using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Ticketron.App.Persistence;
using Ticketron.DB.Models;

namespace Ticketron.App.ViewModels;

public class TasksForTodayViewModel
{
    public ReadOnlyObservableCollection<TodayPageTaskGrouping> TasksForToday { get; }

    private readonly TasksForTodayPersistenceManager _persistenceManager;

    private readonly IDictionary<int, TaskGroupViewModel> _taskGroups;

    private readonly ObservableCollection<TodayPageTaskGrouping> _tasksForToday;
    private IEnumerable<TaskViewModel> AllTasks => _tasksForToday.SelectMany(group => group);

    public TasksForTodayViewModel(ICollection<TaskGroup> taskGroups, TasksForToday tasksForToday, TasksForTodayPersistenceManager persistenceManager)
    {
        _persistenceManager = persistenceManager;

        _taskGroups = taskGroups.ToDictionary(group => group.Id, group => new TaskGroupViewModel(group));

        _tasksForToday = new ObservableCollection<TodayPageTaskGrouping>();
        TasksForToday = new ReadOnlyObservableCollection<TodayPageTaskGrouping>(_tasksForToday);

        RegroupTasks(tasksForToday);
    }

    private void RegroupTasks(TasksForToday tasksForToday)
    {
        foreach (var task in AllTasks)
            task.PropertyChanged -= TaskPropertyChanged;

        _tasksForToday.Clear();

        // below I am assuming that the task groups are in their corresponding dictionaries.
        // this is intentional. if they ever aren't, I'm in big trouble and need to fix my app.

        _tasksForToday.Add(
            new TodayPageTaskGrouping(
                "Overdue",
                tasksForToday.OverdueTasks.Select(task => new TaskViewModel(task, _taskGroups[task.GroupId]))));
        _tasksForToday.Add(
            new TodayPageTaskGrouping(
                "Scheduled for today",
                tasksForToday.TasksScheduledForToday.Select(task => new TaskViewModel(task, _taskGroups[task.GroupId]))));
        _tasksForToday.Add(
            new TodayPageTaskGrouping(
                "Done today",
                tasksForToday.TasksDoneToday.Select(task => new TaskViewModel(task, _taskGroups[task.GroupId]))));

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