using Newtonsoft.Json;
using RecruitmentAbi.Employees;
using RecruitmentAbi.Models;

namespace RecruitmentAbi.Services
{
    public class EmployeeService
    {
        public List<EmployeeStructure> _structure = [];

        public List<Employee> LoadEmployees(string? filePath)
        {
            filePath ??= "EmployeeList.json";

            string json = File.ReadAllText(filePath);
            var employees = JsonConvert.DeserializeObject<List<Employee>>(json) ?? throw new Exception("List is empty");

            var employeeDict = new Dictionary<int, Employee>();
            foreach (var emp in employees)
                employeeDict[emp.Id] = emp;

            foreach (var emp in employees)
                if (emp.SuperiorId.HasValue)
                    emp.Superior = employeeDict[emp.SuperiorId.Value];

            return employees;
        }

        public List<EmployeeStructure> FillEmployeesStructure(List<Employee> employees)
        {
            var map = employees.ToDictionary(e => e.Id, e => e);

            foreach (var employee in employees)
            {
                var current = employee;
                int level = 1;

                while (current.SuperiorId.HasValue)
                {
                    var superiorId = current.SuperiorId.Value;

                    _structure.Add(new EmployeeStructure
                    {
                        EmployeeId = employee.Id,
                        SuperiorId = superiorId,
                        Level = level
                    });

                    current = map[superiorId];
                    level++;
                }
            }
            return _structure;
        }

        public int? GetSuperiorRowOfEmployee(int employeeId, int superiorId)
        {
            var rel = _structure.FirstOrDefault(r => r.EmployeeId == employeeId && r.SuperiorId == superiorId);
            return rel?.Level;
        }
    }
}
