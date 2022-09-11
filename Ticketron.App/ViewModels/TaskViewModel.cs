using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Ticketron.DB.Models;

namespace Ticketron.App.ViewModels;

public class TaskViewModel : ObservableObject
{
    public Task Model { get; }

    public TaskViewModel(TaskGroupViewModel taskGroup)
        : this(new Task(taskGroup.Model), taskGroup)
    {
    }

    public TaskViewModel(Task task, TaskGroupViewModel taskGroup)
    {
        Model = task;
        _taskGroup = taskGroup;
    }

    public string Title
    {
        get => Model.Title;
        set => SetProperty(Model.Title, value, Model, (task, title) => task.Title = title);
    }

    public bool Done
    {
        get => Model.Done;
        set => SetProperty(Model.Done, value, Model, (task, done) => task.Done = done);
    }

    public DateTime? ScheduledFor
    {
        get => Model.ScheduledFor;
        set => SetProperty(Model.ScheduledFor, value, Model, (task, scheduledFor) => task.ScheduledFor = scheduledFor);
    }

    private TaskGroupViewModel _taskGroup;

    public TaskGroupViewModel TaskGroup
    {
        get => _taskGroup;
        set
        {
            _taskGroup = value;
            SetProperty(Model.GroupId, value.Model.Id, Model, (task, groupId) => task.GroupId = groupId);
        }
    }
}