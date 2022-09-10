using CommunityToolkit.Mvvm.ComponentModel;
using Ticketron.DB.Models;

namespace Ticketron.App.ViewModels;

public class TaskGroupViewModel : ObservableObject
{
    public TaskGroup Model { get; set; }

    public TaskGroupViewModel()
        : this(new TaskGroup())
    {
    }

    public TaskGroupViewModel(TaskGroup group)
    {
        Model = group;
    }

    public string Icon
    {
        get => Model.Icon;
        set => SetProperty(Model.Icon, value, Model, (model, icon) => model.Icon = icon);
    }

    public string Name
    {
        get => Model.Name;
        set => SetProperty(Model.Name, value, Model, (model, name) => model.Name = name);
    }
}