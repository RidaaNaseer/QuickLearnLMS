namespace QuickLearnLMS.Models
{
    public class Assignment
    {
        public int AssignmentID { get; set; }

        public int CourseID { get; set; }
        public Course Course { get; set; }

        public string Title { get; set; }

        public DateTime Deadline { get; set; }
        public string FilePath { get; set; } // <== new property for uploaded file

    }

}
