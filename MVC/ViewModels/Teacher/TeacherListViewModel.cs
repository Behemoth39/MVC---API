using WestCoastEducation.web.ViewModels;

namespace WestCoastEducation.web.ViewModels;

public class TeacherListViewModel : PersonViewModel
{
    public IList<CourseListViewModel> Courses { get; set; }
    //public IList<QualificationVIewModel> Qualifications { get; set; } // finns ej än
}
