using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class TreeNodeViewModel
    {
        public int Id { get; set; }
        public string Parent { get; set; }
        public string Text { get; set; }
    }
}
