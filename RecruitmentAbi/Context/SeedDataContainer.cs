using RecruitmentAbi.Models;
using System.Text.Json;

namespace RecruitmentAbi.Context
{
    public class SeedDataContainer
    {
        public List<Team> Teams { get; set; }
        public List<VacationPackage> VacationPackages { get; set; }
        public List<Emp> Employees { get; set; }
        public List<Vacation> Vacations { get; set; }

        public static Task ImportSeedData(EmpContext context, string? jsonPath)
        {
            jsonPath ??= "DatabaseRecords.json";
            var json = File.ReadAllText(jsonPath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var seedData = JsonSerializer.Deserialize<SeedDataContainer>(json, options) ?? throw new Exception("Could not parse seed JSON.");

            context.Teams.AddRange(seedData.Teams);
            context.VacationPackages.AddRange(seedData.VacationPackages);
            context.Employees.AddRange(seedData.Employees);
            context.Vacations.AddRange(seedData.Vacations);

            context.SaveChangesAsync();
            return Task.CompletedTask;
        }
    }
}