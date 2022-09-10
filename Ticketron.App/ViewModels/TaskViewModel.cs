using CommunityToolkit.Mvvm.ComponentModel;
using Ticketron.DB.Models;

namespace Ticketron.App.ViewModels;

public class TaskViewModel : ObservableObject
{
    private readonly Task _task;

    public TaskViewModel(Task task)
    {
        _task = task;
    }

    public string Title
    {
        get => _task.Title;
        set => SetProperty(_task.Title, value, _task, (task, title) => task.Title = title);
    }

    public bool Done
    {
        get => _task.Done;
        set => SetProperty(_task.Done, value, _task, (task, done) => task.Done = done);
    }
}