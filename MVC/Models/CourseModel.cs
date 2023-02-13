using System.ComponentModel.DataAnnotations;

namespace WestCoastEducation.web.Models;

public class CourseModel
{
    [Key]
    public int CourseId { get; set; }
    public string CourseName { get; set; } = "";
    public string CourseNumber { get; set; } = "";
    public string CourseTitle { get; set; } = "";
    public string CourseStart { get; set; } = "";
    public int CourseLenght { get; set; }
}
