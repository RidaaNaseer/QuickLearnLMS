namespace QuickLearnLMS.Models
{
    public class StudentDashboardViewModel
    {
        public Course Course { get; set; }
        public Assignment? Assignment { get; set; }
        public Quiz? Quiz { get; set; }
    }
}
