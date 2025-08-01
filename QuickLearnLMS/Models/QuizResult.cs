using System.ComponentModel.DataAnnotations;

namespace QuickLearnLMS.Models
{
    public class QuizResult
    {
        [Key]
        public int ResultID { get; set; }

        public int QuizID { get; set; }
        public Quiz Quiz { get; set; }

        public int StudentID { get; set; }
        public User Student { get; set; }

        public int Score { get; set; }

    }

}
