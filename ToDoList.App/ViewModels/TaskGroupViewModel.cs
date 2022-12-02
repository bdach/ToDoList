using CommunityToolkit.Mvvm.ComponentModel;
using ToDoList.DB.Models;

namespace ToDoList.App.ViewModels;

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

    public override string ToString() => $"{Icon} {Name}";
}