using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.App.ViewModels;
using ToDoList.DB.Repositories;

namespace ToDoList.App.Persistence;

public class AppStatePersistenceManager
{
    private readonly ITaskGroupRepository _taskGroupRepository;
    private readonly ITaskLogRepository _taskLogRepository;

    public AppStatePersistenceManager(
        ITaskGroupRepository taskGroupRepository,
        ITaskLogRepository taskLogRepository)
    {
        _taskGroupRepository = taskGroupRepository;
        _taskLogRepository = taskLogRepository;
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

    public Task CreateAsync(TaskLogEntryViewModel logEntry)
        => _taskLogRepository.CreateAsync(logEntry.Model);

    public Task UpdateAsync(TaskLogEntryViewModel logEntry)
        => _taskLogRepository.UpdateAsync(logEntry.Model);
}