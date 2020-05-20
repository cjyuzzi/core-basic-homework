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
    public class EnrollmentsController : ControllerBase
    {
        private readonly ContosoUniversityContext db;

        public EnrollmentsController(ContosoUniversityContext db)
        {
            this.db = db;
        }

        // GET api/enrollments
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetEnrollmentsAsync()
        {
            return await db.Enrollment.ToListAsync();
        }

        // GET api/enrollments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Enrollment>> GetEnrollmentByIdAsync(int id)
        {
            return await db.Enrollment.FindAsync(id);
        }

        // POST api/enrollments
        [HttpPost("")]
        public async Task<IActionResult> PostEnrollmentAsync(Enrollment enrollment)
        {
            db.Enrollment.Add(enrollment);
            await db.SaveChangesAsync();
            return Created($"/api/enrollments/{enrollment.EnrollmentId}", enrollment);
        }

        // PUT api/enrollments/5
        [HttpPut("{id}")]
        public async Task PutEnrollmentAsync(int id, Enrollment enrollment)
        {
            enrollment.EnrollmentId = id;
            db.Enrollment.Update(enrollment);
            await db.SaveChangesAsync();

        }

        // DELETE api/enrollments/5
        [HttpDelete("{id}")]
        public async Task DeleteEnrollmentByIdAsync(int id)
        {
            var enrollmentToDelete = await db.Enrollment.FindAsync(id);
            db.Enrollment.Remove(enrollmentToDelete);
            await db.SaveChangesAsync();
        }
    }
}