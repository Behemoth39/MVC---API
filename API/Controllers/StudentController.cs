using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using westcoasteducation.api.Data;
using westcoasteducation.api.Models;
using westcoasteducation.api.ViewModels;

namespace westcoasteducation.api.Controllers;

[ApiController]
[Route("api/v1/students")] // Error handling is not fully implemented
public class StudentController : ControllerBase
{
    public WestCoastEducationContext _context { get; }
    public StudentController(WestCoastEducationContext context)
    {
        _context = context;
    }

    [HttpGet()]
    public async Task<ActionResult> List()
    {
        var result = await _context.Students
        .Include(c => c.Course)
        .Select(s => new StudentDetailListViewModel
        {
            Id = s.Id,
            Age = s.Age,
            FirstName = s.FirstName,
            LastName = s.LastName,
            Email = s.Email,
            Phone = s.Phone,
            Course = s.Course.CourseTitle
        })
        .ToListAsync();

        if (result is null) return BadRequest($"Finns inga studenter");
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var result = await _context.Students
        .Include(c => c.Course)
        .Select(s => new StudentDetailListViewModel
        {
            Id = s.Id,
            Age = s.Age,
            FirstName = s.FirstName,
            LastName = s.LastName,
            Email = s.Email,
            Phone = s.Phone,
            Course = s.Course.CourseTitle
        })
        .SingleOrDefaultAsync(s => s.Id == id);

        if (result is null) return BadRequest($"Finns ingen studenter med id:{id}");
        return Ok(result);
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult> GetByEmail(string email)
    {
        var result = await _context.Students
       .Include(c => c.Course)
       .Select(s => new StudentDetailListViewModel
       {
           Id = s.Id,
           Age = s.Age,
           FirstName = s.FirstName,
           LastName = s.LastName,
           Email = s.Email,
           Phone = s.Phone,
           Course = s.Course.CourseTitle
       })
       .SingleOrDefaultAsync(t => t.Email!.ToUpper().Trim() == email.ToUpper().Trim());

        if (result is null) return BadRequest($"Finns ingen studenter med email {email}");
        return Ok(result);
    }

    [HttpPost()]
    public async Task<ActionResult> AddStudent(StudentAddUpdateViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest("Information saknas, kontrollera så att allt stämmer");

        var exists = await _context.Students.SingleOrDefaultAsync(s => s.Email!.ToUpper().Trim() == model.Email!.ToUpper().Trim());

        if (exists is not null) return BadRequest($"En student med email {model.Email} finns redan");

        var student = new StudentModel
        {
            Age = model.Age,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Phone = model.Phone
        };

        await _context.Students.AddAsync(student);
        if (await _context.SaveChangesAsync() > 0)
        {
            return CreatedAtAction(nameof(GetById),
            new { id = student.Id },
            new { id = student.Id, Name = student.FirstName + "" + student.LastName });
        }

        return StatusCode(500, "Internal Server Error");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateStudent(int id, StudentAddUpdateViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest("Information saknas");

        var student = await _context.Students.FindAsync(id);
        if (student is null) return BadRequest($"Studenten {model.FirstName} {model.LastName} finns inte");

        student.Age = model.Age;
        student.FirstName = model.FirstName;
        student.LastName = model.LastName;
        student.Email = model.Email;
        student.Phone = model.Phone;

        _context.Students.Update(student);
        if (await _context.SaveChangesAsync() > 0)
        {
            return NoContent();
        }

        return StatusCode(500, "Internal Server Error");
    }

    [HttpPatch("registertocourse/{courseId}/{studentId}")]
    public async Task<ActionResult> RegisterToCourse(int courseId, int studentId)
    {
        var course = await _context.Courses.FindAsync(courseId);
        if (course is null) return BadRequest("Kunde inte hitta kursen i systemet.");

        var student = await _context.Students.FindAsync(studentId);
        if (student is null) return BadRequest($"Kunde inte hitta studenten i systemet");

        student.Course = course;

        _context.Courses.Update(course);

        if (await _context.SaveChangesAsync() > 0)
        {
            return NoContent();
        }

        return StatusCode(500, "Internal Server Error");
    }

    [HttpPatch("removefromcourse/{courseId}/{studentId}")]
    public async Task<ActionResult> RemoveFromCourse(int courseId, int studentId)
    {
        var course = await _context.Courses.FindAsync(courseId);
        if (course is null) return BadRequest("Kunde inte hitta kursen i systemet.");

        var student = await _context.Students.FindAsync(studentId);
        if (student is null) return BadRequest($"Kunde inte hitta studenten i systemet");

        student.Course = null;

        _context.Courses.Update(course);

        if (await _context.SaveChangesAsync() > 0)
        {
            return NoContent();
        }

        return StatusCode(500, "Internal Server Error");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student is null) return NotFound($"Finns ingen student med id: {id}");

        _context.Students.Remove(student);
        if (await _context.SaveChangesAsync() > 0)
        {
            return NoContent();
        }

        return StatusCode(500, "Internal Server Error");
    }
}
