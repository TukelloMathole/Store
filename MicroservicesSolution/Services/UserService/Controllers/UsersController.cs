using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // Dummy data store (In a real application, you'd use a database)
        private static List<User> users = new List<User>
        {
            new User { Id = 1, Name = "Alice", Email = "alice@example.com" },
            new User { Id = 2, Name = "Bob", Email = "bob@example.com" }
        };

        // GET: api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(users); // Returns a list of users
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = users.Find(u => u.Id == id);
            if (user == null)
            {
                return NotFound(); // Return 404 if user not found
            }
            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public ActionResult<User> CreateUser(User newUser)
        {
            newUser.Id = users.Count + 1; // Generate a new ID
            users.Add(newUser); // Add user to the dummy store
            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser); // Return 201 Created
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User updatedUser)
        {
            var user = users.Find(u => u.Id == id);
            if (user == null)
            {
                return NotFound(); // Return 404 if user not found
            }
            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            return NoContent(); // Return 204 No Content on success
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = users.Find(u => u.Id == id);
            if (user == null)
            {
                return NotFound(); // Return 404 if user not found
            }
            users.Remove(user); // Remove user from the list
            return NoContent(); // Return 204 No Content on success
        }
    }

    // Dummy User Model (In a real application, you'd have a separate model class)
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
