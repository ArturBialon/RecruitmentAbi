namespace RecruitmentAbi.Models
{
    public class Vacation
    {
        public int Id { get; set; }
        public DateTime DateSince { get; set; }
        public DateTime DateUntil { get; set; }
        public int NumberOfHours { get; set; }
        public bool IsPartialVacation { get; set; }

        public int EmpId { get; set; }
        public Emp Emp { get; set; }
    }
}
