using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Ticketron.DB.Models;

namespace Ticketron.App.ViewModels;

public class TaskLogEntryViewModel : ObservableObject
{
    public TaskLogEntry Model { get; }

    public TaskViewModel Task { get; }

    public TaskLogEntryViewModel(TaskViewModel task)
        : this(new TaskLogEntry(task.Model), task)
    {
    }

    public TaskLogEntryViewModel(TaskLogEntry logEntry, TaskViewModel task)
    {
        if (logEntry.TaskId != task.Model.Id)
            throw new ArgumentException("Detected ID mismatch when associating model with viewmodel");

        Model = logEntry;
        Task = task;
    }

    public DateTime Start
    {
        get => Model.Start;
        set => SetProperty(Model.Start, value, Model, (logEntry, start) => logEntry.Start = start);
    }

    public DateTime? End
    {
        get => Model.End;
        set => SetProperty(Model.End, value, Model, (logEntry, end) => logEntry.End = end);
    }

    public string Notes
    {
        get => Model.Notes;
        set => SetProperty(Model.Notes, value, Model, (logEntry, notes) => logEntry.Notes = notes);
    }
}