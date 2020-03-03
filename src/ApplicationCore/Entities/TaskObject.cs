using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class TaskObject : BaseEntity
    {
        public string Title { get; set; }
        public Status Status { get; set; }
        public string Description { get; set; }
        public string Executors { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime CompletionDate { get; set; }

        public int LabourIntensity { get; set; } // Плановая трудоёмкость задачи
        public TimeSpan LeadTime // Фактическое время выполнения
        {
            get { return CompletionDate - RegisterDate; }
        }

        public int? ParentId { get; set; }
        public TaskObject Parent { get; set; }
        public ICollection<TaskObject> Children { get; set; }
    }
}
