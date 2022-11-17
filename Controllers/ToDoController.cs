using DependencyInjectionDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace DependencyInjectionDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : ControllerBase
    {

        private readonly ILogger<ToDoController> _logger;
        private readonly ToDoContext _context;

        public ToDoController(ILogger<ToDoController> logger, ToDoContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("ById")]
        [ProducesResponseType(typeof(ToDo), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult GetById(int id)
        {
            try
            {
                var todo = _context.ToDos?.Find(id);
                if (todo == null)
                {
                    return NotFound("The requested resource was not found");
                }
                return Ok(todo);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }

        }

        [HttpGet]
        [Route("All")]
        [ProducesResponseType(typeof(List<ToDo>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll()
        {
            try
            {
                if (_context.ToDos == null || !_context.ToDos.Any())
                    return NotFound("No ToDos found in the database");
                return Ok(_context.ToDos?.ToList());
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
            
        }

        [HttpDelete]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(int id)
        {
            try
            {
                var todo = _context.ToDos?.Find(id);
                if (todo == null)
                {
                    return NotFound($"Todo with id {id} was not found");
                }

                _context.ToDos?.Remove(todo);
                var result = _context.SaveChanges();
                if (result >= 1)
                {
                    return Ok("Delete operation was successful");
                }
                return Problem("Delete was not successful. Please try again");
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
            
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult Add(ToDo todoToAdd)
        {
            //what if they provide an id?
            if (todoToAdd.Id != 0)
            {
                return BadRequest("Id was provided but not needed");
            }
            //1.) Fix it and don't tell and just do it
            //2.) Tell them to fix it
            try
            {
                //todoToAdd.Id = 0;
                _context.ToDos?.Add(todoToAdd);
                var result = _context.SaveChanges();
                if (result >= 1)
                {
                    return Ok($"Todo {todoToAdd.Name} added successfully");
                }
                return Problem("Add was not successful. Please try again");
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult Put(ToDo todoToEdit)
        {
            if (todoToEdit.Id < 1)
            {
                return BadRequest("Please provide a valid id");
            }

            try
            {
                var todo = _context.ToDos?.Find(todoToEdit.Id);
                if (todo == null)
                    return NotFound("The todo was not found");

                todo.Name = todoToEdit.Name;
                todo.DueDate = todoToEdit.DueDate;
                _context.ToDos?.Update(todo);
                var result = _context.SaveChanges();
                if (result >= 1)
                {
                    return Ok("Todo edited successfully");
                }
                return Problem("Edit was not successful. Please try again");
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }
    }
}