namespace QuickLearnLMS.Models
{
    public class Material
    {
        public int MaterialID { get; set; }

        public int CourseID { get; set; }
        public Course Course { get; set; }

        public string FilePath { get; set; }
    }

}
