using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITaskRepository repository;
        public HomeController(ITaskRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult Index() => Ok();
    }
}
