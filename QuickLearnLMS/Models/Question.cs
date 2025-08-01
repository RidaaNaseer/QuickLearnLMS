namespace QuickLearnLMS.Models
{
    public class Question
    {
        public int QuestionID { get; set; }
        public string Text { get; set; }

        public int QuizID { get; set; }
        public Quiz Quiz { get; set; }

        public ICollection<Option> Options { get; set; }

    }
}
