namespace QuickLearnLMS.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        public string Title { get; set; }
        public int TeacherID { get; set; }
        public User Teacher { get; set; }
    }
}
