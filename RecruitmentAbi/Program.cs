using Microsoft.EntityFrameworkCore;
using RecruitmentAbi.Context;
using RecruitmentAbi.Services;

public class Program
{
    public static void Main()
    {
        var services = new ServiceCollection();
        services.AddDbContext<EmpContext>(options => options.UseInMemoryDatabase("test.db"));
        var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<EmpContext>();

        // zadanie 1
        var employeeService = new EmployeeService();
        var structure = employeeService.FillEmployeesStructure(employeeService.LoadEmployees("EmployeeList.json"));

        var row1 = employeeService.GetSuperiorRowOfEmployee(2, 1); // row1 = 1
        var row2 = employeeService.GetSuperiorRowOfEmployee(4, 3); // row2 = null
        var row3 = employeeService.GetSuperiorRowOfEmployee(4, 1); // row3 = 2

        foreach (var employee in structure)
        {
            Console.WriteLine($"ID: {employee.EmployeeId}, Superior: {employee.SuperiorId}, Level: {employee.Level}");
        }

        // zadanie 2
        var companyService = new CompanyService(context);
        companyService.ImportSeedData(context, null);
        var first = companyService.GetEmpsFromTeamWithActiveVacationForm(".NET");
        var second = companyService.GetEmpsWithUsedVacationDaysNumber();
        var third = companyService.GetTeamsWhereNobodyHasDayOfIn2019();

        foreach (var employee in first)
        {
            Console.WriteLine($"1 Emp name: {employee.Name}, Team {employee.Team.Name}");
        }
        foreach (var employee in second)
        {
            Console.WriteLine($"2 Emp name: {employee.Emp.Name}, Used days: {employee.VacationDaysUsed}");
        }
        foreach (var team in third)
        {
            Console.WriteLine($"3 Team where noone has used vacation: {team.Name}");
        }

        //zadanie 6
        /*Zakładając, że parametry metody z zadania 3 pobieramy bezpośrednio z bazy danych.Czy
        znasz jakieś sposoby na optymalizację liczby zapytań SQL w tego typu przypadkach? Wymień
        i opisz krótko każdy z nich.*/

        // Eager loading, czyli pobieranie pełnej zawartości obiektu, odwołanie za pomocą .Include
        // ale można też skorzystać jak ja z Lazy Loading i odwołać się do pożądanego obiektu połączonego relacją z obiektem do którego się bezpośrednio odwołujemy

        //Projection (Select z anonimowym typem / DTO)
        //Zamiast ładować całe encje, wybierasz tylko te pola, które są naprawdę potrzebne

        //Zagnieżdżenie logiki po stronie SQL, czyli można przygotować jedno bardziej złożone zapytanie SQL realizowane po stronie serwera

        //Caching

        //Konsolidacja logiki w LINQ, czyli zamiast nadużywać pętli foreach możemy postarać się o jakąś sprytną lambdę/warunek logiczny w Where() i to wylistować.

    }

}