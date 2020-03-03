using ApplicationCore;
using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class TaskRepository : EfRepository<TaskObject>, ITaskRepository
    {
        public TaskRepository(ApplicationContext context)
            : base(context)
        {
        }

        public async Task<TaskObject> GetByIdWithChildren(int id)
        {
            return await context.Tasks.Include(t => t.Children)
                                      .SingleOrDefaultAsync(t => t.Id == id);
        }

        public override async Task Update(TaskObject newEntity)
        {
            var oldEntity = await context.Tasks.AsNoTracking()
                                         .SingleOrDefaultAsync(t => t.Id == newEntity.Id);
            if (oldEntity == null)
                throw new TaskRepositoryException(nameof(newEntity.Id), "Bad ID!");
            newEntity.ParentId = oldEntity.ParentId;

            // if status is not modified, then we don't have to think about children
            if (oldEntity.Status == newEntity.Status)
            {
                await base.Update(newEntity);
                return;
            }

            // if the requirement FR005 is not met
            if (!canChangeStatus(oldEntity.Status, newEntity.Status))
                throw new TaskRepositoryException(nameof(newEntity.Status), "Cannot change status!");

            context.Attach(newEntity);

            if (newEntity.Status == Status.Completed)
            {
                // try to complete all the subtasks
                if (await canCompleteSubtasks(newEntity))
                    completeSubtasks(newEntity);
                else
                {
                    context.Entry(newEntity).State = EntityState.Detached;
                    throw new TaskRepositoryException(nameof(newEntity.Status), "Cannot complete one or more subtasks!");
                }
            }

            await base.Update(newEntity);
        }

        // recursively check all subtasks
        private async Task<bool> canCompleteSubtasks(TaskObject parent)
        {
            if (!canChangeStatus(parent.Status, Status.Completed))
                return false;

            await context.Entry(parent).Collection(t => t.Children).LoadAsync(); // loading children
            foreach (var child in parent.Children)
            {
                if (!await canCompleteSubtasks(child))
                    return false;
            }
            return true;
        }

        // recursively marks all subtasks completed
        private void completeSubtasks(TaskObject parent)
        {
            parent.Status = Status.Completed;

            foreach (var child in parent.Children)
            {
                completeSubtasks(child);
            }
        }

        // The status "Completed" can be assigned only after the status "Running".
        // The status "Paused" can be assigned only after the status "Running".
        private bool canChangeStatus(Status oldStatus, Status newStatus)
        {
            if (oldStatus == newStatus) return true;

            if (newStatus == Status.Paused || newStatus == Status.Completed)
            {
                return oldStatus == Status.Running;
            }
            return true;
        }
    }
}
