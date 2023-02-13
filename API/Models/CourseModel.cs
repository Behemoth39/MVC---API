using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace westcoasteducation.api.Models;

public class CourseModel
{
    [Key]
    public int Id { get; set; }
    public CourseStatusEnum Status { get; set; }
    public string CourseNumber { get; set; }
    public string CourseTitle { get; set; }
    public DateOnly CourseStartDate { get; set; }
    public DateOnly CourseEndDate { get; set; }

    public IList<StudentModel> Students { get; set; }

    public int? TeacherId { get; set; }

    [ForeignKey("TeacherId")]
    public TeacherModel Teacher { get; set; }
}
