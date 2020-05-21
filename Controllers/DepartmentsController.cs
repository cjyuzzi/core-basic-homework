using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using homework.Models;
using System;

namespace homework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ContosoUniversityContext db;

        public DepartmentsController(ContosoUniversityContext db)
        {
            this.db = db;
        }

        // GET api/departments
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartmentsAsync()
        {
            return await db.Department.Where(d => d.IsDeleted != true).ToListAsync();
        }

        // GET api/departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartmentByIdAsync(int id)
        {
            var department = await db.Department.FirstOrDefaultAsync(d => d.IsDeleted != true && d.DepartmentId == id);

            if (department is null)
                return NoContent();

            return department;
        }

        // POST api/departments
        [HttpPost("")]
        public async Task<IActionResult> PostDepartmentAsync(Department department)
        {
            var name = new SqlParameter("name", department.Name);
            var budget = new SqlParameter("budget", department.Budget);
            var startDate = new SqlParameter("startDate", department.StartDate);
            var instructorId = new SqlParameter("instructorId", department.InstructorId);

            department.DepartmentId = await db.Department
               .FromSqlRaw($"EXECUTE Department_Insert @name , @budget , @startDate , @instructorId", name, budget, startDate, instructorId)
               .Select(d => d.DepartmentId)
               .ToListAsync()
               .ContinueWith<int>(list => list.Result.FirstOrDefault());

            return Created($"/api/departments/{department.DepartmentId}", department);
        }

        // PUT api/departments/5
        [HttpPut("{id}")]
        public async Task PutDepartmentAsync(int id, Department department)
        {
            using var tran = await db.Database.BeginTransactionAsync();

            try
            {
                var tasks = new List<Task>();

                tasks.Add(db.Database.ExecuteSqlInterpolatedAsync($"EXECUTE [dbo].[Department_Update] {id} , {department.Name} , {department.Budget} , {department.StartDate} , {department.InstructorId} , {department.RowVersion}"));
                tasks.Add(UpdateDepartmentDateModifiedAsync(id));

                await Task.WhenAll(tasks);

                await tran.CommitAsync();
            }
            catch (System.Exception ex)
            {
                await tran.RollbackAsync();
                StatusCode(500, ex.Message);
            }
        }

        private async Task UpdateDepartmentDateModifiedAsync(int id)
        {
            var department = await db.Department.FindAsync(id);
            department.DateModified = DateTime.Now;
            await db.SaveChangesAsync();
        }

        // DELETE api/departments/5
        [HttpDelete("{id}")]
        public async Task DeleteDepartmentByIdAsync(int id)
        {
            var departmentToDelete = await db.Department.FindAsync(id);
            // await db.Database.ExecuteSqlInterpolatedAsync($"EXECUTE [dbo].[Department_Delete] {departmentToDelete.DepartmentId} , {departmentToDelete.RowVersion}");
            departmentToDelete.IsDeleted = true;
            await db.SaveChangesAsync();
        }

        // GET api/departments/DepartmentCourseCount
        [HttpGet("DepartmentCourseCount")]
        public async Task<ActionResult<IEnumerable<VwDepartmentCourseCount>>> GetDepartmentCourseCountAsync()
        {
            return await db.VwDepartmentCourseCount
                .FromSqlRaw("SELECT * FROM [vwDepartmentCourseCount]")
                .ToListAsync();
        }

    }
}