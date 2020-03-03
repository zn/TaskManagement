using ApplicationCore;
using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace UnitTests
{
    public class TaskRepositoryTests
    {
        ITaskRepository repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "tasks")
                .Options;

            var context = new ApplicationContext(options);
            repository = new TaskRepository(context);
        }

        [Test]
        // “естирует услови€ изменени€ статуса
        public void CanChangeStatusTest()
        {
            MethodInfo method = typeof(TaskRepository).GetMethod("canChangeStatus", BindingFlags.NonPublic | BindingFlags.Instance);
            Func<Status, Status, bool> canChangeStatus = delegate (Status oldStatus, Status newStatus)
            {
                return (bool)method.Invoke(repository, new object[] { oldStatus, newStatus });
            };

            bool assignedCompleted = canChangeStatus(Status.Assigned, Status.Completed);
            bool assignedPaused = canChangeStatus(Status.Assigned, Status.Paused);
            bool pausedCompleted = canChangeStatus(Status.Paused, Status.Completed);
            bool runningCompleted = canChangeStatus(Status.Running, Status.Completed);
            bool runningPaused = canChangeStatus(Status.Running, Status.Paused);

            Assert.IsFalse(assignedCompleted);
            Assert.IsFalse(assignedPaused);
            Assert.IsFalse(pausedCompleted);
            Assert.IsTrue(runningCompleted);
            Assert.IsTrue(runningPaused);
        }

        [Test]
        // “естирует обновление статуса у всего поддерева
        public async Task UpdateTaskTest()
        {
            var taskObj1 = new TaskObject
            {
                Id = 1,
                Status = Status.Running
            };
            var taskObj2 = new TaskObject
            {
                Id = 2,
                Status = Status.Running,
                ParentId = 1
            };
            var taskObj3 = new TaskObject
            {
                Id = 3,
                Status = Status.Assigned,
                ParentId = 2
            };

            await repository.Add(taskObj1);
            await repository.Add(taskObj2);
            await repository.Add(taskObj3);

            taskObj1.Status = Status.Completed;
            Assert.ThrowsAsync<TaskRepositoryException>(async () => await repository.Update(taskObj1)); // т.к. у третьей задачи статус Assigned

            taskObj3.Status = Status.Running;
            await repository.Update(taskObj3);
            await repository.Update(taskObj1); // должны обновитьс€ статусы у всех подзадач

            var tasks = await repository.GetAll();
            Assert.IsTrue(tasks.All(t => t.Status == Status.Completed));
        }
    }
}