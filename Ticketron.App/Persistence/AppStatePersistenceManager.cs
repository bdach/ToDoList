using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Ticketron.App.ViewModels;
using Ticketron.DB.Repositories;

namespace Ticketron.App.Persistence;

public class AppStatePersistenceManager
{
    private readonly ITaskGroupRepository _taskGroupRepository;

    public AppStatePersistenceManager(
        ITaskGroupRepository taskGroupRepository)
    {
        _taskGroupRepository = taskGroupRepository;
    }

    public async Task<AppViewModel> LoadAppState()
    {
        var taskGroups = await _taskGroupRepository.GetAllAsync();
        return new AppViewModel(taskGroups, this);
    }

    public Task CreateAsync(IEnumerable<TaskGroupViewModel> taskGroups) =>
        _taskGroupRepository.CreateAsync(taskGroups.Select(group => group.Model).ToArray());

    public Task UpdateAsync(TaskGroupViewModel taskGroup) =>
        _taskGroupRepository.UpdateAsync(taskGroup.Model);

    public Task DeleteAsync(IEnumerable<TaskGroupViewModel> taskGroups) =>
        _taskGroupRepository.DeleteAsync(taskGroups.Select(group => group.Model).ToArray());
}