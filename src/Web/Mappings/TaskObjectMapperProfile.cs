using ApplicationCore.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Mappings
{
    public class TaskObjectMapperProfile : Profile
    {
        public TaskObjectMapperProfile()
        {
            CreateMap<TaskObject, TaskDetailsViewModel>()
                .ReverseMap();

            CreateMap<TaskObject, TreeNodeViewModel>()
                .ForMember(taskObj => taskObj.Parent, options => options.Ignore())
                .ConstructUsing(taskObj => new TreeNodeViewModel
                {
                    Id = taskObj.Id,
                    Parent = taskObj.ParentId != null ? taskObj.ParentId.ToString() : "#",
                    Text = taskObj.Title
                });
        }
    }
}
