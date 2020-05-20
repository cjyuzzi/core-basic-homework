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
    public class OfficeAssignmentsController : ControllerBase
    {
        private readonly ContosoUniversityContext db;

        public OfficeAssignmentsController(ContosoUniversityContext db)
        {
            this.db = db;
        }

        // GET api/officeassignments
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<OfficeAssignment>>> GetOfficeAssignmentsAsync()
        {
            return await db.OfficeAssignment.ToListAsync();
        }

        // GET api/officeassignments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OfficeAssignment>> GetOfficeAssignmentByIdAsync(int id)
        {
            return await db.OfficeAssignment.FindAsync(id);
        }

        // POST api/officeassignments
        [HttpPost("")]
        public async Task<IActionResult> PostOfficeAssignmentAsync(OfficeAssignment assignment)
        {
            await db.OfficeAssignment.AddAsync(assignment);
            await db.SaveChangesAsync();
            return Created($"/api/officeassignments/{assignment.InstructorId}", assignment);
        }

        // PUT api/officeassignments/5
        [HttpPut("{id}")]
        public async Task PutOfficeAssignmentAsync(int id, OfficeAssignment assignment)
        {
            assignment.InstructorId = id;
            db.OfficeAssignment.Update(assignment);
            await db.SaveChangesAsync();
        }

        // DELETE api/officeassignments/5
        [HttpDelete("{id}")]
        public async Task DeleteOfficeAssignmentByIdAsync(int id)
        {
            var assignmentToDelete = await db.OfficeAssignment.FindAsync(id);
            db.OfficeAssignment.Remove(assignmentToDelete);
            await db.SaveChangesAsync();
        }
    }
}