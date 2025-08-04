﻿namespace QuickLearnLMS.Models
{
    public class Quiz
    {
        public int QuizID { get; set; }

        public int CourseID { get; set; }
        public Course Course { get; set; }

        public string Title { get; set; }
        public ICollection<Question> Questions { get; set; }

    }

}
