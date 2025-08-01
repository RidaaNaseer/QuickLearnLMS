namespace QuickLearnLMS.Models
{
    public class Option
    {
        public int OptionID { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }

        public int QuestionID { get; set; }
        public Question Question { get; set; }
    }
}
