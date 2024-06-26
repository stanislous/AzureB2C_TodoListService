﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListService.Models;
using Microsoft.Identity.Web.Resource;

namespace TodoListService.Controllers;

[Authorize(Roles = "Registered")]
[Route("api/[controller]")]
[RequiredScope(ScopeRequiredByApi)]
public class TodoListController : Controller
{
    const string ScopeRequiredByApi = "task.read";
    // In-memory TodoList
    private static readonly Dictionary<int, Todo?> TodoStore = new();
    private readonly IHttpContextAccessor _contextAccessor;

    public TodoListController(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
        string owner = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

        // Pre-populate with sample data
        if (TodoStore.Count == 0)
        {
            TodoStore.Add(1, new Todo { Id = 1, Owner = owner, Title = "Pick up groceries" });
            TodoStore.Add(2, new Todo { Id = 2, Owner = owner, Title = "Finish invoice report" });
        }
    }

    // GET: api/values
    [HttpGet, Authorize(Roles = "Admin")]
    public IEnumerable<Todo?> Get()
    {
        var owner = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        return TodoStore.Values.Where(x => x.Owner == owner);
    }

    // GET: api/values
    [HttpGet("{id}", Name = "Get")]
    public Todo? Get(int id)
    {
        return TodoStore.Values.FirstOrDefault(t => t.Id == id);
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        TodoStore.Remove(id);
    }

    // POST api/values
    [HttpPost]
    public IActionResult Post([FromBody] Todo todo)
    {
        string? owner = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        int id = TodoStore.Values.MaxBy(x => x.Id)!.Id + 1;

        Todo? todonew = new Todo() { Id = id, Owner = owner, Title = todo.Title };
        TodoStore.Add(id, todonew);

        return Ok(todo);
    }

    // PATCH api/values
    [HttpPatch("{id}")]
    public IActionResult Patch(int id, [FromBody] Todo? todo)
    {
        if (id != todo.Id)
        {
            return NotFound();
        }

        if (TodoStore.Values.FirstOrDefault(x => x.Id == id) == null)
        {
            return NotFound();
        }

        TodoStore.Remove(id);
        TodoStore.Add(id, todo);

        return Ok(todo);
    }
}
