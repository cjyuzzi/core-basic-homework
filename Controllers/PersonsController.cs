using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using homework.Models;
using System;
using System.Linq;

namespace homework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly ContosoUniversityContext db;

        public PersonsController(ContosoUniversityContext db)
        {
            this.db = db;
        }

        // GET api/person
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersonsAsync()
        {
            return await db.Person.Where(p => p.IsDeleted != true).ToListAsync();
        }

        // GET api/person/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPersonByIdAsync(int id)
        {
            var person = await db.Person.FirstOrDefaultAsync(p => p.IsDeleted != true && p.Id == id);

            if (person is null)
            {
                return NoContent();
            }
            return person;
        }

        // POST api/person
        [HttpPost("")]
        public async Task<IActionResult> PostPersonAsync(Person person)
        {
            db.Person.Add(person);
            await db.SaveChangesAsync();
            return Created($"/api/person/{person.Id}", person);
        }

        // PUT api/person/5
        [HttpPut("{id}")]
        public async Task PutPersonAsync(int id, Person person)
        {
            person.Id = id;
            person.DateModified = DateTime.Now;
            db.Person.Update(person);
            await db.SaveChangesAsync();
        }

        // DELETE api/person/5
        [HttpDelete("{id}")]
        public async Task DeletePersonByIdAsync(int id)
        {
            var personToDelete = await db.Person.FindAsync(id);
            personToDelete.IsDeleted = true;
            await db.SaveChangesAsync();
        }
    }
}