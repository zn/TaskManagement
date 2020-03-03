using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
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

        [HttpGet]
        public IActionResult Create(int? id)
        {
            var viewModel = new CreateTaskViewModel() { ParentId = id };
            return PartialView(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                var taskObj = mapper.Map<TaskObject>(model);
                taskObj = await repository.Add(taskObj);
                return RedirectToAction(nameof(Details), new { id = taskObj.Id, updateTree = true });
            }
            return PartialView(model);
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var taskObj = await repository.GetById(id);
            var viewModel = mapper.Map<UpdateTaskViewModel>(taskObj);
            return PartialView(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                var taskObj = mapper.Map<TaskObject>(model);
                try
                {
                    await repository.Update(taskObj);
                }
                catch (TaskRepositoryException ex)
                {
                    ModelState.AddModelError(ex.Key, ex.Message);
                    return PartialView(model);
                }
                return RedirectToAction(nameof(Details), new { id = taskObj.Id, updateTree = true });
            }
            return PartialView(model);
        }

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
