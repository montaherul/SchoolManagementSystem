namespace school_management_system.Models
{
    public class TeacherSubject
    {
        public int TeacherSubjectID { get; set; }

        public int TeacherID { get; set; }

        public int SubjectID { get; set; }

        public int ClassID { get; set; }

        public int SectionID { get; set; }

        public Teacher? Teacher { get; set; }

        public Subject? Subject { get; set; }

        public Class? Class { get; set; }

        public Section? Section { get; set; }
    }
}