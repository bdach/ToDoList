using System;
using System.Threading.Tasks;
using Ticketron.App.ViewModels;
using Ticketron.DB.Models;
using Ticketron.DB.Repositories;
using Task = System.Threading.Tasks.Task;

namespace Ticketron.App.Persistence;

public class TasksForTodayPersistenceManager
{
    private readonly ITaskGroupRepository _taskGroupRepository;
    private readonly ITasksForTodayRepository _tasksForTodayRepository;
    private readonly ITaskRepository _taskRepository;

    public TasksForTodayPersistenceManager(
        ITaskGroupRepository taskGroupRepository,
        ITasksForTodayRepository tasksForTodayRepository,
        ITaskRepository taskRepository)
    {
        _taskGroupRepository = taskGroupRepository;
        _tasksForTodayRepository = tasksForTodayRepository;
        _taskRepository = taskRepository;
    }

    public async Task<TasksForTodayViewModel> LoadState()
    {
        var taskGroups = await _taskGroupRepository.GetAllAsync();
        var tasksForToday = await GetTasksForTodayAsync();
        return new TasksForTodayViewModel(taskGroups, tasksForToday, this);
    }

    public Task<TasksForToday> GetTasksForTodayAsync() =>
        _tasksForTodayRepository.GetTasksForTodayAsync(DateTime.Today);

    public Task CreateAsync(TaskViewModel taskViewModel) => _taskRepository.CreateAsync(taskViewModel.Model);

    public Task UpdateAsync(TaskViewModel taskViewModel) => _taskRepository.UpdateAsync(taskViewModel.Model);

    public Task DeleteAsync(TaskViewModel taskViewModel) => _taskRepository.DeleteAsync(taskViewModel.Model);
}