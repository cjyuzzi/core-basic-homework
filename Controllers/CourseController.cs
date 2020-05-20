using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using homework.Models;
using Microsoft.EntityFrameworkCore;

namespace homework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ContosoUniversityContext db;

        public CourseController(ContosoUniversityContext db)
        {
            this.db = db;
        }

        // GET api/course
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await db.Course.ToListAsync();
        }

        // GET api/course/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourseByIdAsync(int id)
        {
            return await db.Course.FindAsync(id);
        }

        // POST api/course
        [HttpPost("")]
        public async Task<IActionResult> PostCourseAsync(Course course)
        {
            db.Course.Add(course);
            await db.SaveChangesAsync();
            return Created($"/api/course/{course.CourseId}", course);
        }

        // PUT api/course/5
        [HttpPut("{id}")]
        public async Task PutCourseAsync(int id, Course course)
        {
            db.Course.Update(course);
            await db.SaveChangesAsync();
        }

        // DELETE api/course/5
        [HttpDelete("{id}")]
        public async Task DeleteCourseByIdAsync(int id)
        {
            var toDelete = await db.Course.FindAsync(id);
            db.Course.Remove(toDelete);
            await db.SaveChangesAsync();
        }
    }
}