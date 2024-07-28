using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task5
{
    class Program
    {
        static void Main(string[] args)
        {
            // Список сотрудников
            List<string> employees = new List<string>
            {
                "Иванов Иван Иванович",
                "Петров Петр Петрович",
                "Юлина Юлия Юлиановна",
                "Сидоров Сидор Сидорович",
                "Павлов Павел Павлович",
                "Георгиев Георг Георгиевич"
            };

            // Словарь для хранения отпусков каждого сотрудника
            Dictionary<string, List<DateTime>> vacations = new Dictionary<string, List<DateTime>>();
            foreach (var employee in employees)
            {
                vacations[employee] = new List<DateTime>();
            }

            Random random = new Random();
            DateTime startDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime endDate = new DateTime(DateTime.Now.Year, 12, 31);
            int[] vacationLengths = { 7, 14 };

            foreach (var employee in employees)
            {
                int daysRemaining = 28;

                while (daysRemaining > 0)
                {
                    int vacationLength = vacationLengths[random.Next(vacationLengths.Length)];
                    if (daysRemaining < vacationLength)
                    {
                        vacationLength = daysRemaining;
                    }

                    DateTime vacationStart;
                    bool isVacationValid;
                    do
                    {
                        vacationStart = RandomDay(random, startDate, endDate);
                        isVacationValid = IsValidVacation(vacations, employee, vacationStart, vacationLength);
                    } while (!isVacationValid);

                    for (int i = 0; i < vacationLength; i++)
                    {
                        vacations[employee].Add(vacationStart.AddDays(i));
                    }
                    daysRemaining -= vacationLength;
                }
            }

            // Вывод результатов
            foreach (var employee in employees)
            {
                Console.WriteLine($"Дни отпуска {employee}:");
                foreach (var day in vacations[employee])
                {
                    Console.WriteLine(day.ToString("dd.MM.yyyy"));
                }
            }
            Console.ReadLine();
        }

        static DateTime RandomDay(Random random, DateTime start, DateTime end)
        {
            int range = (end - start).Days;
            DateTime randomDate;
            do
            {
                randomDate = start.AddDays(random.Next(range));
            } while (randomDate.DayOfWeek == DayOfWeek.Saturday || randomDate.DayOfWeek == DayOfWeek.Sunday);
            return randomDate;
        }

        static bool IsValidVacation(Dictionary<string, List<DateTime>> vacations, string employee, DateTime start, int length)
        {
            var employeeVacations = vacations[employee];
            var allVacations = vacations.Values.SelectMany(v => v).ToList();

            for (int i = 0; i < length; i++)
            {
                DateTime currentDay = start.AddDays(i);
                if (employeeVacations.Contains(currentDay) || allVacations.Contains(currentDay))
                {
                    return false;
                }
            }

            DateTime minIntervalStart = start.AddMonths(-1);
            DateTime minIntervalEnd = start.AddMonths(1).AddDays(length - 1);

            foreach (var vacationDay in employeeVacations)
            {
                if (vacationDay >= minIntervalStart && vacationDay <= minIntervalEnd)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
