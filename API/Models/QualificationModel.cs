using System.ComponentModel.DataAnnotations;

namespace westcoasteducation.api.Models;

public class QualificationModel
{
    [Key]
    public int Id { get; set; }
    public string Qualification { get; set; }

    public IList<TeacherModel> Teachers { get; set; }
}
