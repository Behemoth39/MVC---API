namespace westcoasteducation.api.Models;

public class TeacherModel : PersonModel
{
    public IList<CourseModel> Courses { get; set; }
    public IList<QualificationModel> Qualifications { get; set; }
}
