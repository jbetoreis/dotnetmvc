using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Models;

namespace Todo.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("/")]
        public IActionResult List([FromServices] DataContext ctx)
            => Ok(ctx.Todos.AsNoTracking().ToList());

        [HttpGet("/{id:int}")]
        public IActionResult Get(
            [FromRoute] int id,
            [FromServices] DataContext ctx
        )
        {
            var todo = ctx.Todos.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (todo == null) return NotFound();
            return Ok(todo);
        }

        [HttpPost("/")]
        public IActionResult Post(
            [FromBody] TodoModel todo,
            [FromServices] DataContext ctx
        )
        {
            ctx.Todos.Add(todo);
            ctx.SaveChanges();
            return Created($"/{todo.Id}", todo);
        }

        [HttpPut("/{id:int}")]
        public IActionResult Put(
            [FromBody] TodoModel todo,
            [FromRoute] int id,
            [FromServices] DataContext ctx
        )
        {
            var targetTodo = ctx.Todos.FirstOrDefault(x => x.Id == id);
            if (targetTodo == null) return NotFound();
            targetTodo.Title = todo.Title;
            targetTodo.Done = todo.Done;
            ctx.Todos.Update(targetTodo);
            ctx.SaveChanges();
            return Ok(targetTodo);
        }

        [HttpDelete("/{id:int}")]
        public IActionResult Delete(
            [FromRoute] int id,
            [FromServices] DataContext ctx
        )
        {
            var todo = ctx.Todos.FirstOrDefault(x => x.Id == id);
            if (todo == null) return NotFound();
            ctx.Todos.Remove(todo);
            ctx.SaveChanges();
            return Ok(todo);
        }
    }
}