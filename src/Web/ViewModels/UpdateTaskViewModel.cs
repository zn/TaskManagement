using ApplicationCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class UpdateTaskViewModel : TaskBaseViewModel
    {
        public int Id { get; set; }

        [Required]
        public Status Status { get; set; }
        public SelectList StatusSelectList { get; set; } = new SelectList(Enum.GetValues(typeof(Status)));
    }
}
