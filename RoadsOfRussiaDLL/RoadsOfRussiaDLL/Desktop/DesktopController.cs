using Newtonsoft.Json;
using RoadsOfRussiaDLL.Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RoadsOfRussiaDLL.Desktop
{
    public class DesktopController
    {
        // Сотрудники подразделения
        public async Task<List<EmployeeSelectedModel>> GetEmployeesAsync(int divisionId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string apiUrl = $"http://localhost:5246/api/Desktop/GetEmployees/{divisionId}";
                var response = await httpClient.GetStringAsync(apiUrl);

                var employees = JsonConvert.DeserializeObject<List<EmployeeSelectedModel>>(response);
                return employees;
            }
        }

        // Структура подразделений
        public async Task<DivisionModel> GetDivisionsAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("http://localhost:5246/api/Desktop/FirstDivisions");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<DivisionModel>(json);
                }
                return null;
            }
        }

        // Должности
        public async Task<List<PostModel>> GetPostAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string apiUrl = $"http://localhost:5246/api/Desktop/Posts";
                var response = await httpClient.GetStringAsync(apiUrl);

                var post = JsonConvert.DeserializeObject<List<PostModel>>(response);
                return post;
            }
        }

        // Директора и помощники по подразделению
        public async Task<List<DivisionEmployeeModel>> GetDirecorAndAssistent(int divisionId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string apiUrl = $"http://localhost:5246/api/Desktop/GetEmployees/{divisionId}";
                var response = await httpClient.GetStringAsync(apiUrl);

                var post = JsonConvert.DeserializeObject<List<DivisionEmployeeModel>>(response);
                return post;
            }
        }

        // Календарь обучения
        public async Task<List<CalandarModel>> GetTraningCalendar(int employeeId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string apiUrl = $"http://localhost:5246/api/Desktop/Calendar/TraningCalendar/{employeeId}";
                var response = await httpClient.GetStringAsync(apiUrl);

                var calendar = JsonConvert.DeserializeObject<List<CalandarModel>>(response);
                return calendar;
            }
        }

        // Календарь отпуска
        public async Task<List<CalandarModel>> GetVacationCalendar(int employeeId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string apiUrl = $"http://localhost:5246/api/Desktop/Calendar/VacationCalendar/{employeeId}";
                var response = await httpClient.GetStringAsync(apiUrl);

                var calendar = JsonConvert.DeserializeObject<List<CalandarModel>>(response);
                return calendar;
            }
        }

        // Календарь временного отсутствия
        public async Task<List<CalandarModel>> GetTemporaryAbsenceCalendar(int employeeId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string apiUrl = $"http://localhost:5246/api/Desktop/Calendar/TemporaryAbsenceCalendar/{employeeId}";
                var response = await httpClient.GetStringAsync(apiUrl);

                var calendar = JsonConvert.DeserializeObject<List<CalandarModel>>(response);
                return calendar;
            }
        }

        // Удаление календаря обучения
        public async Task<bool> RemoveTraningCalendar(int calendarId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string apiUrl = $"http://localhost:5246/api/Desktop/RemoveCalendar/TraningCalendar/{calendarId}";
                var response = await httpClient.PostAsync(apiUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Удаления календаря временного отсутсвия
        public async Task<bool> RemoveTemporaryAbsenceCalendar(int calendarId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string apiUrl = $"http://localhost:5246/api/Desktop/RemoveCalendar/TemporaryAbsenceCalendar/{calendarId}";
                var response = await httpClient.PostAsync(apiUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Удаление календаря отпуска
        public async Task<bool> RemoveVacationCalendar(int calendarId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string apiUrl = $"http://localhost:5246/api/Desktop/RemoveCalendar/VacationCalendar/{calendarId}";
                var response = await httpClient.PostAsync(apiUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Увольнение сотрудника
        public async Task<bool> FiredEmployee(int employeeId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var apiUrl = $"http://localhost:5246/api/Desktop/FiredEmployee/{employeeId}";
                var response = await httpClient.PostAsync(apiUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Сохранение изменений сотрудника
        public async Task<bool> SaveEmployee(int? employeeId, string surname, string name, string patronymic, string personalPhone, DateTime dateOfBirth, int divisionId, int postId, int? directorId, int? assistentId, string corpPhone, string email, string cabinet, string otherInformation)
        {
            var newEmployee = new SaveEmployeeModel() { IDEmployee = employeeId, Surname = surname, Name = name, Patronymic = patronymic, PersonalPhone = personalPhone, DateOfBirth = dateOfBirth, IDDivision = divisionId, IDPost = postId, IDDirector = directorId, IDAssistent = assistentId, CorpPhone = corpPhone, Email = email, Cabinet = cabinet, OtherInformation = otherInformation };

            string jsonData = JsonConvert.SerializeObject(newEmployee);

            using (var client = new HttpClient())
            {
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:5246/api/Desktop/SaveEmployee", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Создание календаря обучения
        public async Task<bool> PostTraningCalendar(int employeeId, string title, string description, DateTime startDate, DateTime endDate)
        {
            var newCalendar = new AddCalendarModel() { IDEmployee = employeeId, Title = title, Description = description, StartDate = startDate, EndDate = endDate };

            string jsonData = JsonConvert.SerializeObject(newCalendar);

            using (var client = new HttpClient())
            {
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:5246/api/Desktop/AddCalendar/TraningCalendar", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Создание календаря отпуска
        public async Task<bool> PostVacationCalendar(int employeeId, string title, string description, DateTime startDate, DateTime endDate)
        {
            var newCalendar = new AddCalendarModel() { IDEmployee = employeeId, Title = title, Description = description, StartDate = startDate, EndDate = endDate };

            string jsonData = JsonConvert.SerializeObject(newCalendar);

            using (var client = new HttpClient())
            {
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:5246/api/Desktop/AddCalendar/VacationCalendar", content);


                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // Создание календаря временного отсутствия
        public async Task<bool> PostTemporaryAbsenceCalendar(int employeeId, string title, string description, DateTime startDate, DateTime endDate)
        {
            var newCalendar = new AddCalendarModel() { IDEmployee = employeeId, Title = title, Description = description, StartDate = startDate, EndDate = endDate };

            string jsonData = JsonConvert.SerializeObject(newCalendar);

            using (var client = new HttpClient())
            {
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:5246/api/Desktop/AddCalendar/TemporaryAbsenceCalendar", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
