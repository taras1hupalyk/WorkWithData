using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WorkWithData.Enums;
using WorkWithData.Models;

namespace WorkWithData
{
    public class LinqQueryService
    {
        private readonly IEnumerable<Project> _projects;

        public LinqQueryService(IEnumerable<Project> projects)
        {
            _projects = projects;
        }

        public  IEnumerable<(Project Key, int taskNumber)> Task1(int id)
        {
             var result = 
                from project in _projects
                from task in project.TasksDTO
                    where task.PerformerId == id
                group task by project into projectTaskGroup
                let taskNumber = projectTaskGroup.Count()
                orderby taskNumber descending
                select ( projectTaskGroup.Key, taskNumber);
            return result;
        }


        public IEnumerable<TaskDTO> Task2(int id)
        {
            var result = 
                from project in _projects
                from task in project.TasksDTO
                    where task.PerformerId == id && task.Name.Length < 45
                select task;
            return result;
        }

        public IEnumerable<(int,string?)> Task3(int id)
        {
            var result =
                from project in _projects
                from task in project.TasksDTO
                where task.PerformerId == id &&
                    task.FinishedAt?.Year == DateTime.Now.Year
                select (task.Id, task.Name);
            return result;
        }

        public List<(int Id, string? Name, List<IGrouping<int?, User>>)> Task4()
        {
            var result = _projects.Select(x => x.Team)
                .Select(y => (y.Id, y.Name, y.Users
                .Where(user => DateTime.Now.Year - user.BirthDay.Year > 10)
                .OrderByDescending(a => a.RegisteredAt)
                .GroupBy(user => user.TeamId).ToList())).ToList();
            return result;
        }

        public IEnumerable<(User Key, IOrderedEnumerable<TaskDTO>)> Task5(IEnumerable<User> usersNotInTeam)
        {
            var result =
                from project in _projects
                from task in project.TasksDTO
                group task by task.Performer into taskByUserGroup
                orderby taskByUserGroup.Key.FirstName ascending
                select (
                taskByUserGroup.Key,
                taskByUserGroup.ToList().OrderByDescending(x => x.Name.Length));

            return result;
        }


        public (User user, Project lastUserProject, int, int, TaskDTO) Task6(int id)
        {
            var result =
                from project in _projects
                from user in project.Team.Users
                where user.Id == id
                let lastUserProject = user.Projects.OrderBy(pr => pr.CreatedAt).LastOrDefault()
                select (
                    user,
                    lastUserProject,
                    lastUserProject.TasksDTO.Count(),
                    user.Tasks.Where(x => x.State == TaskState.InProgress ||
                        x.State == TaskState.Canceled).Count(),
                    user.Tasks.OrderByDescending(x => x.FinishedAt - x.CreatedAt).FirstOrDefault()
                );

            return result.FirstOrDefault();
        }


        public IEnumerable<(Project project, TaskDTO, TaskDTO, int)> Task7()
        {
            var result =
                from project in _projects
                select (
                project,
                project.TasksDTO
                    .OrderByDescending(x => x.Description).FirstOrDefault(),

                project.TasksDTO
                    .OrderBy(x => x.Name).FirstOrDefault(),

                project.Team.Users
                    .Where(x => project?.Description?.Length > 20 || project.TasksDTO.Count() < 3).Count()
                );

            return result;
        }
    }
}
