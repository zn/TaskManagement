using ApplicationCore.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationContextSeed
    {
        public static async Task SeedAsync(ApplicationContext context, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger<ApplicationContextSeed>();
            await context.Database.EnsureCreatedAsync();

            if (context.Tasks.Any())
            {
                return;
            }

            var parentTask = new TaskObject
            {
                Title = "Title #1",
                Description = "This is description",
                Executors = "Alex",
                CompletionDate = DateTime.Now + TimeSpan.FromDays(1),
                RegisterDate = DateTime.Now
            };
            parentTask = (await context.Tasks.AddAsync(parentTask)).Entity;
            await context.SaveChangesAsync();

            var subtask = new TaskObject
            {
                Title = "Title #2",
                Description = "This is description of the subtask",
                Executors = "Alex",
                CompletionDate = DateTime.Now + TimeSpan.FromDays(1),
                RegisterDate = DateTime.Now,
                ParentId = parentTask.Id
            };
            subtask = (await context.Tasks.AddAsync(subtask)).Entity;
            await context.SaveChangesAsync();

            var subsubtask = new TaskObject
            {
                Title = "Title #3",
                Description = "This is description of the subtask of the subtask",
                Executors = "Alex",
                CompletionDate = DateTime.Now + TimeSpan.FromDays(1),
                RegisterDate = DateTime.Now,
                ParentId = subtask.Id
            };

            await context.Tasks.AddAsync(subsubtask);

            await context.SaveChangesAsync();
            logger.LogInformation("The database seeded successfully");
        }
    }
}
