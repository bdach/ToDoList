using CommunityToolkit.Mvvm.ComponentModel;
using Ticketron.DB.Models;

namespace Ticketron.App.ViewModels;

public class TaskViewModel : ObservableObject
{
    public Task Model { get; }

    public TaskViewModel(Task task)
    {
        Model = task;
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
}