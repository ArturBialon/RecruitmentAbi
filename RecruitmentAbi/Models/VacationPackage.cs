using RecruitmentAbi.Context;

namespace RecruitmentAbi.Models
{
    public class VacationPackage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GrantedDays { get; set; }
        public int Year { get; set; }

        public List<Emp> Emps { get; set; }
    }
}
