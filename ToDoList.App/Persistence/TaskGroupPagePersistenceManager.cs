using System.Threading.Tasks;
using ToDoList.App.ViewModels;
using ToDoList.DB.Repositories;

namespace ToDoList.App.Persistence;

public class TaskGroupPagePersistenceManager
{
    private readonly ITaskGroupRepository _taskGroupRepository;
    private readonly ITaskRepository _taskRepository;

    public TaskGroupPagePersistenceManager(
        ITaskGroupRepository taskGroupRepository,
        ITaskRepository taskRepository)
    {
        _taskGroupRepository = taskGroupRepository;
        _taskRepository = taskRepository;
    }

    public async Task<TaskGroupPageViewModel> LoadState(int taskGroupId)
    {
        var taskGroup = await _taskGroupRepository.GetAsync(taskGroupId);
        var tasks = await _taskRepository.GetForGroupAsync(taskGroup);

        return new TaskGroupPageViewModel(taskGroup, tasks, this);
    }

    public Task CreateAsync(TaskViewModel taskViewModel) => _taskRepository.CreateAsync(taskViewModel.Model);

    public Task UpdateAsync(TaskViewModel taskViewModel) => _taskRepository.UpdateAsync(taskViewModel.Model);

    public Task DeleteAsync(TaskViewModel taskViewModel) => _taskRepository.DeleteAsync(taskViewModel.Model);
}