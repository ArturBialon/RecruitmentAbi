using RecruitmentAbi.Context;
using RecruitmentAbi.Models;

namespace RecruitmentAbi.Services
{
    public class CompanyService
    {
        private readonly EmpContext _context;
        private readonly int currentYear = 2019;

        public CompanyService(EmpContext context)
        {
            _context = context;
        }

        //zadanie 2
        public List<Emp> GetEmpsFromTeamWithActiveVacationForm(string teamName)
        {
            var empList = _context.Employees
                .Where(emp => emp.Team.Name == teamName &&
                              emp.Vacations.Any(v =>
                                  (v.DateSince.Year == currentYear && v.DateUntil.Year == currentYear) ||
                                  (v.DateSince.Year < currentYear && v.DateUntil.Year == currentYear) ||
                                  (v.DateSince.Year == currentYear && v.DateUntil.Year <= currentYear)
                              )
                       )
                .ToList();

            return empList;
        }

        public List<EmpDTO> GetEmpsWithUsedVacationDaysNumber()
        {
            var today = DateTime.Today.AddYears(-6);
            var yearStart = new DateTime(currentYear, 1, 1);

            var result = _context.Employees
            .Select(emp => new EmpDTO
            {
                Emp = emp,
                VacationDaysUsed = emp.Vacations
                    .Where(v => v.DateSince.Year <= currentYear && v.DateUntil.Year == currentYear)
                    .Select(v => new
                    {
                        Start = v.DateSince < yearStart ? yearStart : v.DateSince > today ? today.AddDays(1) : v.DateSince,
                        End = v.DateUntil > today ? today : v.DateUntil
                    })
                    .Sum(x => (int)x.End.Subtract(x.Start).TotalDays + 1)
            })
            .ToList();

            return result;
        }

        public List<Team> GetTeamsWhereNobodyHasDayOfIn2019()
        {
            var result = _context.Teams
            .Where(t => !t.Emp.Any(e =>
                e.Vacations.Any(v =>
                    v.DateSince.Year == currentYear ||
                    v.DateUntil.Year == currentYear ||
                    (v.DateSince.Year < currentYear && v.DateUntil.Year > currentYear))))
            .ToList();

            return result;
        }

        //zadanie 3
        public int CountFreeDaysForEmployee(Emp employee)
        {
            ArgumentNullException.ThrowIfNull(employee);

            var vacationPackageDays = _context.Employees.Select(e => e.VacationPackage.GrantedDays).SingleOrDefault();
            var vacations = employee.Vacations;

            var yearStart = new DateTime(currentYear, 1, 1);
            var yearEnd = new DateTime(currentYear, 12, 31);

            int usedDays = GetEmpsWithUsedVacationDaysNumber().Where(e => e.Emp.Id == employee.Id).Select(e => e.VacationDaysUsed).SingleOrDefault();

            int remaining = Math.Max(vacationPackageDays - usedDays, 0);

            return remaining;
        }

        //zadanie 4 + 5 Unit test
        public bool IfEmployeeCanRequestVacation(Emp employee)
        {
            ArgumentNullException.ThrowIfNull(employee);

            var vacationPackageDays = employee.VacationPackage.GrantedDays;

            var yearStart = new DateTime(currentYear, 1, 1);
            var yearEnd = new DateTime(currentYear, 12, 31);

            int usedDays = GetEmpsWithUsedVacationDaysNumber().Where(e => e.Emp.Id == employee.Id).Select(e => e.VacationDaysUsed).SingleOrDefault();

            int remaining = Math.Max(vacationPackageDays - usedDays, 0);

            return remaining > 0;
        }


        public void ImportSeedData(EmpContext context, string? pathToFile)
        {
            SeedDataContainer.ImportSeedData(context, pathToFile);
        }
    }
}
