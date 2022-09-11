using System;
using System.Threading.Tasks;
using Ticketron.App.ViewModels;
using Ticketron.DB.Models;
using Ticketron.DB.Repositories;
using Task = System.Threading.Tasks.Task;

namespace Ticketron.App.Persistence;

public class TasksForTodayPersistenceManager
{
    private readonly ITasksForTodayRepository _tasksForTodayRepository;
    private readonly ITaskRepository _taskRepository;

    public TasksForTodayPersistenceManager(
        ITasksForTodayRepository tasksForTodayRepository,
        ITaskRepository taskRepository)
    {
        _tasksForTodayRepository = tasksForTodayRepository;
        _taskRepository = taskRepository;
    }

    public async Task<TasksForTodayViewModel> LoadState()
    {
        var tasksForToday = await GetTasksForTodayAsync();
        return new TasksForTodayViewModel(tasksForToday, this);
    }

    public Task<TasksForToday> GetTasksForTodayAsync() =>
        _tasksForTodayRepository.GetTasksForTodayAsync(DateTime.Today);

    public Task CreateAsync(TaskViewModel taskViewModel) => _taskRepository.CreateAsync(taskViewModel.Model);

    public Task UpdateAsync(TaskViewModel taskViewModel) => _taskRepository.UpdateAsync(taskViewModel.Model);

    public Task DeleteAsync(TaskViewModel taskViewModel) => _taskRepository.DeleteAsync(taskViewModel.Model);
}