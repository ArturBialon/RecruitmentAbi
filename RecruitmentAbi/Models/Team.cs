using RecruitmentAbi.Context;

namespace RecruitmentAbi.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Emp> Emp { get; set; }
    }
}
