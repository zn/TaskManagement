using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class CreateTaskViewModel : TaskBaseViewModel
    {
        public int? ParentId { get; set; }
    }
}
