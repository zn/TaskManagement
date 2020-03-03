using ApplicationCore;
using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class TaskDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Executors { get; set; }
        public DateTime RegisterDate { get; set; }
        public Status Status { get; set; }
        public DateTime CompletionDate { get; set; }
        public IEnumerable<TaskObject> Children { get; set; }
        public bool UpdateTree { get; set; }
        public int LabourIntensity => Children.Count() + 1;
        public TimeSpan LeadTime => CompletionDate - RegisterDate;
        public bool IsExpired => CompletionDate < DateTime.Now;
    }
}
