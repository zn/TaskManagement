using ApplicationCore;
using ApplicationCore.Entities;
using AutoMapper;
using System;
using Web.ViewModels;

namespace Web.Mappings
{
    public class TaskObjectMapperProfile : Profile
    {
        public TaskObjectMapperProfile()
        {
            CreateMap<TaskObject, TaskDetailsViewModel>()
                .ReverseMap();

            CreateMap<TaskObject, DeleteTaskViewModel>()
                .ReverseMap();

            CreateMap<TaskObject, TreeNodeViewModel>()
                .ForMember(taskObj => taskObj.Parent, options => options.Ignore())
                .ConstructUsing(taskObj => new TreeNodeViewModel
                {
                    Id = taskObj.Id,
                    Parent = taskObj.ParentId != null ? taskObj.ParentId.ToString() : "#",
                    Text = taskObj.Title
                });

            CreateMap<CreateTaskViewModel, TaskObject>()
                .ForMember(
                    taskObj => taskObj.Status,
                    options => options.MapFrom(src => Status.Assigned))
                .ForMember(
                    taskObj => taskObj.RegisterDate,
                    options => options.MapFrom(src => DateTime.Now))
                .ForMember(
                    taskObj => taskObj.CompletionDate,
                    options => options.MapFrom(vm => vm.CompletionDate
                                                       .AddHours(vm.CompletionTime.Hour)
                                                       .AddMinutes(vm.CompletionTime.Minute)));

            CreateMap<UpdateTaskViewModel, TaskObject>()
                .ForMember(
                    taskObj => taskObj.RegisterDate,
                    options => options.MapFrom(src => DateTime.Now))
                .ForMember(
                    taskObj => taskObj.CompletionDate,
                    options => options.MapFrom(vm => vm.CompletionDate
                                                       .AddHours(vm.CompletionTime.Hour)
                                                       .AddMinutes(vm.CompletionTime.Minute)));

            CreateMap<TaskObject, UpdateTaskViewModel>()
                .ForMember(
                    vm => vm.CompletionTime,
                    options => options.MapFrom(taskObj => taskObj.CompletionDate));
        }
    }
}
