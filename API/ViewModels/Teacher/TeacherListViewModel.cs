namespace westcoasteducation.api.ViewModels;

public class TeacherListViewModel : PersonViewModel
{
    public IList<CourseListViewModel> Courses { get; set; }
    public IList<QualificationListVIewModel> Qualifications { get; set; }
}
