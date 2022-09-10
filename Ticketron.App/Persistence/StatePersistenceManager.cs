using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Ticketron.App.ViewModels;
using Ticketron.DB.Repositories;

namespace Ticketron.App.Persistence;

public class StatePersistenceManager
{
    private readonly ITaskGroupRepository _taskGroupRepository;

    public StatePersistenceManager(
        ITaskGroupRepository taskGroupRepository)
    {
        _taskGroupRepository = taskGroupRepository;
    }

    public async Task<AppViewModel> LoadAppState()
    {
        var state = new AppViewModel();

        var taskGroups = await _taskGroupRepository.GetAllAsync();
        foreach (var taskGroup in taskGroups)
        {
            var viewModel = new TaskGroupViewModel(taskGroup);
            viewModel.PropertyChanged += TaskGroupChanged;
            state.TaskGroups.Add(viewModel);
        }

        state.TaskGroups.CollectionChanged += TaskGroupCollectionChanged;

        return state;
    }

    private async void TaskGroupCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                var newGroups = e.NewItems!.Cast<TaskGroupViewModel>().ToList();

                foreach (var group in newGroups)
                    group.PropertyChanged += TaskGroupChanged;

                await _taskGroupRepository.CreateAsync(newGroups.Select(viewModel => viewModel.Model).ToArray());
                break;

            case NotifyCollectionChangedAction.Remove:
                var oldGroups = e.OldItems!.Cast<TaskGroupViewModel>().ToList();

                foreach (var group in oldGroups)
                    group.PropertyChanged -= TaskGroupChanged;

                await _taskGroupRepository.DeleteAsync(oldGroups.Select(viewModel => viewModel.Model).ToArray());
                break;
        }
    }

    private void TaskGroupChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is not TaskGroupViewModel taskGroup)
            return;

        _taskGroupRepository.UpdateAsync(taskGroup.Model);
    }
}