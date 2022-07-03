using System.Net.Http.Json;
using WorkWithData;
using WorkWithData.Models;

var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("https://bsa-dotnet.azurewebsites.net");

var projects = await httpClient.GetFromJsonAsync<IEnumerable<Project>>("/api/Projects");
var tasks = await httpClient.GetFromJsonAsync<IEnumerable<TaskDTO>>("/api/Tasks");
var teams = await httpClient.GetFromJsonAsync<IEnumerable<Team>>("/api/Teams");
var users = await httpClient.GetFromJsonAsync<IEnumerable<User>>("/api/Users");

users =
    from user in users
    join project in projects on user.Id equals project.AuthorId into puGroup
    join task in tasks on user.Id equals task.PerformerId into tuGroup
    select user with 
        { 
        Projects = puGroup.Select(x => x with { TasksDTO = tasks.Where(y => y.ProjectId ==x.Id)}) ,
        Tasks = tuGroup};


teams =
    from team in teams
    join user in users on team.Id equals user.TeamId into utGroup
    select team with { Users = utGroup };

tasks =
    from task in tasks
    join user in users on task.PerformerId equals user.Id
    select task with { Performer = user };

var result =
    from project in projects
        join user in users on project.AuthorId equals user.Id
        join task in tasks on project.Id equals task.ProjectId into projectsTasksGroup
        join team in teams on project.TeamId equals team.Id
    select new Project
    {
        Id = project.Id,
        AuthorId = user.Id,
        TeamId = team.Id,
        Name = project.Name,
        Description = project.Description,
        Deadline = project.Deadline,
        CreatedAt = project.CreatedAt,
        Author = user,
        TasksDTO = projectsTasksGroup,
        Team = team
    };

var usersNotInTeam = users.Where(x => x.TeamId == null);

      
    

var queryService = new LinqQueryService(result);
var taskResult = queryService.Task4();





