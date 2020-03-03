using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITaskRepository repository;
        private readonly IMapper mapper;
        public HomeController(ITaskRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public IActionResult Index() => View();

        public async Task<JsonResult> GetTree() // for ajax requests
        {
            var items = await repository.GetAll();
            var nodes = mapper.Map<IEnumerable<TreeNodeViewModel>>(items);
            return new JsonResult(nodes);
        }

        public async Task<IActionResult> Details(int id, bool updateTree = false)
        {
            var taskObj = await repository.GetByIdWithChildren(id);
            if (taskObj == null)
                return BadRequest();
            var viewModel = mapper.Map<TaskDetailsViewModel>(taskObj);
            viewModel.UpdateTree = updateTree;
            return PartialView(viewModel);
        }
    }
}
