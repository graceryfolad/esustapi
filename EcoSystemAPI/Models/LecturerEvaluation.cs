namespace ESUSTAPI.Models
{
    public class LecturerEvaluation
    {
        public string StudentFullName { get; set; }
        public string Gender { get; set; }
        public string MatricNumber { get; set; }
        public string CourseCode { get; set; }
        public string CourseTitle { get; set; }
        public string Level { get; set; }
        public string Faculty { get; set; }
        public string Department { get; set; }
        public string LecutureName { get; set; }
        public List<Titles> TitleList { get; set; }

    }

   public class Titles
    {
        public string Title { get; set; }
        public int Score { get; set; }
    }
}
