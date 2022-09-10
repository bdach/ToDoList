using System.Threading.Tasks;
using Ticketron.App.ViewModels;
using Ticketron.DB.Repositories;

namespace Ticketron.App.Persistence;

public class TaskListPersistenceManager
{
    private readonly ITaskGroupRepository _taskGroupRepository;
    private readonly ITaskRepository _taskRepository;

    public TaskListPersistenceManager(
        ITaskGroupRepository taskGroupRepository,
        ITaskRepository taskRepository)
    {
        _taskGroupRepository = taskGroupRepository;
        _taskRepository = taskRepository;
    }

    public async Task<TaskListViewModel> LoadState(int taskGroupId)
    {
        var taskGroup = await _taskGroupRepository.GetAsync(taskGroupId);
        var tasks = await _taskRepository.GetForGroupAsync(taskGroup);

        return new TaskListViewModel(taskGroup, tasks);
    }
}