using System.ComponentModel.DataAnnotations.Schema;

namespace westcoasteducation.api.Models;

public class StudentModel : PersonModel
{
    public int? CourseId { get; set; }

    [ForeignKey("CourseId")]
    public CourseModel Course { get; set; }
}
