namespace QuickLearnLMS.Models
{
    public class Submission
    {
        public int SubmissionID { get; set; }

        public int AssignmentID { get; set; }
        public Assignment Assignment { get; set; }

        public int StudentID { get; set; }
        public User Student { get; set; }

        public string FilePath { get; set; }
    }

}
