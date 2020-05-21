using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using homework.Models;
using System;
using System.Linq;

namespace homework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ContosoUniversityContext db;

        public CoursesController(ContosoUniversityContext db)
        {
            this.db = db;
        }

        // GET api/courses
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await db.Course.Where(c => c.IsDeleted != true).ToListAsync();
        }

        // GET api/courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourseByIdAsync(int id)
        {
            var course = await db.Course.FirstOrDefaultAsync(c => c.IsDeleted != true && c.CourseId == id);
            if (course is null)
                return NoContent();
            return course;
        }

        // POST api/courses
        [HttpPost("")]
        public async Task<IActionResult> PostCourseAsync(Course course)
        {
            db.Course.Add(course);
            await db.SaveChangesAsync();
            return Created($"/api/course/{course.CourseId}", course);
        }

        // PUT api/courses/5
        [HttpPut("{id}")]
        public async Task PutCourseAsync(int id, Course course)
        {
            course.CourseId = id;
            course.DateModified = DateTime.Now;
            db.Course.Update(course);
            await db.SaveChangesAsync();
        }

        // DELETE api/courses/5
        [HttpDelete("{id}")]
        public async Task DeleteCourseByIdAsync(int id)
        {
            var toDelete = await db.Course.FindAsync(id);
            toDelete.IsDeleted = true;
            await db.SaveChangesAsync();
        }

        // GET api/courses/CourseStudents
        [HttpGet("CourseStudents")]
        public async Task<ActionResult<IEnumerable<VwCourseStudents>>> GetCourseStudentsAsync()
        {
            return await db.VwCourseStudents.ToListAsync();
        }

        // GET api/courses/CourseStudentCount
        [HttpGet("CourseStudentCount")]
        public async Task<ActionResult<IEnumerable<VwCourseStudentCount>>> GetCourseStudentCountAsync()
        {
            return await db.VwCourseStudentCount.ToListAsync();
        }
    }
}