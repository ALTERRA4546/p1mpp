﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoadsOfRussiaAPI.Controllers.Model;
using RoadsOfRussiaAPI.DbModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoadsOfRussiaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Desktop : ControllerBase
    {
        private readonly ApplicationDbContext database;

        public Desktop(ApplicationDbContext database) 
        {
            this.database = database;
        }

        // Главная страница

        [HttpGet("FirstDivisions")]
        public async Task<IActionResult> GetFirstDivision()
        {
            var divisions = await database.Division
                .Include(d => d.Division1)
                .ToListAsync();

            return Ok(divisions.FirstOrDefault());
        }

        [HttpGet("GetEmployees/{divisionId}")]
        public async Task<IActionResult> GetEmployeesByDivision(int divisionId)
        {
            var divisions = await database.Division
                .Include(d => d.Division1)
                .ToListAsync();

            var selectedDivision = divisions.FirstOrDefault(d => d.IDDivision == divisionId);

            if (selectedDivision == null)
            {
                return NotFound(new { Message = "Division not found" });
            }

            var employeeSelecteds = await GetEmployees(selectedDivision);
            return Ok(employeeSelecteds);
        }

        private async Task<List<EmployeeSelectedModel>> GetEmployees(Division division)
        {
            var employeesList = new List<EmployeeSelectedModel>();

            var employeeData = await (from employee in database.Employee
                                      join post in database.Post on employee.IDPost equals post.IDPost
                                      where employee.IDDivision == division.IDDivision && (employee.DateOfFired == null || employee.DateOfFired >= DateTime.Now.AddDays(-30))
                                      select new EmployeeSelectedModel
                                      {
                                          IDEmployee = employee.IDEmployee,
                                          IDDivision = employee.IDDivision ?? 0,
                                          Division = division.Title,
                                          IDDirector = employee.IDDirector ?? 0,
                                          Director = database.Employee.Where(x=>x.IDEmployee == employee.IDDirector).Select(s => s.IDEmployee + "|" + s.Surname + " " + s.Name + " " + s.Patronimyc).FirstOrDefault(),
                                          IDAssistent = employee.IDAssistent ?? 0,
                                          Assistent = database.Employee.Where(x=>x.IDEmployee == employee.IDAssistent).Select(s => s.IDEmployee + "|" + s.Surname + " " + s.Name + " " + s.Patronimyc).FirstOrDefault(),
                                          IDPost = post.IDPost,
                                          Post = post.Title,
                                          Surname = employee.Surname,
                                          Name = employee.Name,
                                          Patronymic = employee.Patronimyc,
                                          PersonalPhone = employee.PersonalPhone,
                                          CorpPhone = employee.CorpPhone,
                                          Email = employee.Email,
                                          Cabinet = employee.Cabinet,
                                          DateOfFired = employee.DateOfFired,
                                          DateOfBirth = employee.DateOfBirth,
                                          Fired = employee.DateOfFired != null ? true : false,
                                          OtherInformation = employee.OtherInformaion
                                      }).ToListAsync();

            employeesList.AddRange(employeeData);

            if (division.Division1 != null && division.Division1.Any())
            {
                foreach (var subDivision in division.Division1)
                {
                    employeesList.AddRange(await GetEmployees(subDivision));
                }
            }

            return employeesList;
        }

        // Карточка сотрудника
        [HttpGet("DivisionEmployees/{divisionId}")]
        public async Task<IActionResult> GetDivision(int divisionId)
        {
            var employees = await (from division in database.Division
                                   join employee in database.Employee on division.IDDivision equals employee.IDDivision
                                   join post in database.Post on employee.IDPost equals post.IDPost
                                   where (division.IDDivision == divisionId)
                                   select new EmployeeSelectedModel
                                   {
                                       IDEmployee = employee.IDEmployee,
                                       IDDivision = employee.IDDivision ?? 0,
                                       Division = division.Title,
                                       IDDirector = employee.IDDirector ?? 0,
                                       Director = database.Employee.Where(x => x.IDEmployee == employee.IDDirector).Select(s => s.IDEmployee + "|" + s.Surname + " " + s.Name + " " + s.Patronimyc).FirstOrDefault(),
                                       IDAssistent = employee.IDAssistent ?? 0,
                                       Assistent = database.Employee.Where(x => x.IDEmployee == employee.IDAssistent).Select(s => s.IDEmployee + "|" + s.Surname + " " + s.Name + " " + s.Patronimyc).FirstOrDefault(),
                                       IDPost = post.IDPost,
                                       Post = post.Title,
                                       Surname = employee.Surname,
                                       Name = employee.Name,
                                       Patronymic = employee.Patronimyc,
                                       PersonalPhone = employee.PersonalPhone,
                                       CorpPhone = employee.CorpPhone,
                                       Email = employee.Email,
                                       Cabinet = employee.Cabinet,
                                       DateOfFired = employee.DateOfFired,
                                       DateOfBirth = employee.DateOfBirth,
                                       Fired = employee.DateOfFired != null ? true : false,
                                       OtherInformation = employee.OtherInformaion
                                   }).ToListAsync();

            return Ok(employees);
        }

        // Должности
        [HttpGet("Posts")]
        public async Task<IActionResult> GetPost()
        {
            var post = await database.Post.ToListAsync();

            if (post == null)
            {
                return BadRequest();
            }

            return Ok(post);
        }

        // Увольнение сотрудника
        [HttpPost("FiredEmployee/{employeeId}")]
        public async Task<IActionResult> FiredEmployee(int employeeId)
        {
            var firedEmployee = await database.Employee.FirstOrDefaultAsync(x => x.IDEmployee == employeeId);

            if (firedEmployee != null)
            {
                firedEmployee.DateOfFired = DateTime.Now;

                await database.SaveChangesAsync();

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        // Сохранение сотрудника
        [HttpPost("SaveEmployee")]
        public async Task<IActionResult> SaveEmployee([FromBody] SaveEmployeeModel requset)
        {
            if (requset.IDEmployee != null)
            {
                var employeeData = await database.Employee.FirstOrDefaultAsync(x => x.IDEmployee == requset.IDEmployee);

                if (employeeData != null)
                {
                    employeeData.Surname = requset.Surname;
                    employeeData.Name = requset.Name;
                    employeeData.Patronimyc = requset.Patronymic;
                    employeeData.PersonalPhone = requset.PersonalPhone;
                    employeeData.DateOfBirth = requset.DateOfBirth;
                    employeeData.DateOfFired = employeeData.DateOfFired;
                    employeeData.IDDivision = requset.IDDivision;
                    employeeData.IDPost = requset.IDPost;
                    if(requset.IDDirector != null)
                        employeeData.IDDirector = requset.IDDirector;
                    if(requset.IDAssistent != null)
                        employeeData.IDAssistent = requset.IDAssistent;
                    employeeData.CorpPhone = requset.CorpPhone;
                    employeeData.Email = requset.Email;
                    employeeData.Cabinet = requset.Cabinet;
                    if(requset.OtherInformation != null)
                        employeeData.OtherInformaion = requset.OtherInformation;

                    await database.SaveChangesAsync();

                    return Ok();
                }
                else
                { 
                    return BadRequest();
                }
            }
            else
            {
                var newEmployee = new Employee();

                newEmployee.Surname = requset.Surname;
                newEmployee.Name = requset.Name;
                newEmployee.Patronimyc = requset.Patronymic;
                newEmployee.PersonalPhone = requset.PersonalPhone;
                newEmployee.DateOfBirth = requset.DateOfBirth;
                newEmployee.DateOfFired = null;
                newEmployee.IDDivision = requset.IDDivision;
                newEmployee.IDPost = requset.IDPost;
                if (requset.IDDirector != null)
                    newEmployee.IDDirector = requset.IDDirector;
                if (requset.IDAssistent != null)
                    newEmployee.IDAssistent = requset.IDAssistent;
                newEmployee.CorpPhone = requset.CorpPhone;
                newEmployee.Email = requset.Email;
                newEmployee.Cabinet = requset.Cabinet;
                if(requset.OtherInformation != null)
                    newEmployee.OtherInformaion = requset.OtherInformation;

                database.Employee.Add(newEmployee);
                await database.SaveChangesAsync();

                return Ok();
            }
        }

        // Календари событий
        // Просмотр
        [HttpGet("Calendar/TraningCalendar/{employeeId}")]
        public async Task<IActionResult> GetTraningCalendar(int employeeId)
        {
            var calendar = await (from traningCalendar in database.TraningCalendar
                                  where (traningCalendar.IDEmployee == employeeId)
                                  orderby (traningCalendar.StartDate)
                                  select new CalandarModel
                                  {
                                      IDCalendar = traningCalendar.IDTraningCalendar,
                                      Title = traningCalendar.Title,
                                      Description = traningCalendar.Description,
                                      StartDate = traningCalendar.StartDate,
                                      EndDate = traningCalendar.EndDate,
                                  }).ToListAsync();

            if (calendar == null)
            {
                return BadRequest();
            }

            return Ok(calendar);
        }

        [HttpGet("Calendar/VacationCalendar/{employeeId}")]
        public async Task<IActionResult> GetVacationCalendar(int employeeId)
        {
            var calendar = await (from vacationCalendar in database.VacationCalendar
                                  where (vacationCalendar.IDEmployee == employeeId)
                                  orderby (vacationCalendar.StartDate)
                                  select new CalandarModel
                                  {
                                      IDCalendar = vacationCalendar.IDVacationCalendar,
                                      Title = vacationCalendar.Title,
                                      Description = vacationCalendar.Description,
                                      StartDate = vacationCalendar.StartDate,
                                      EndDate = vacationCalendar.EndDate,
                                  }).ToListAsync();

            if (calendar == null)
            {
                return BadRequest();
            }

            return Ok(calendar);
        }

        [HttpGet("Calendar/TemporaryAbsenceCalendar/{employeeId}")]
        public async Task<IActionResult> GetTemporaryAbsenceCalendar(int employeeId)
        {
            var calendar = await (from temporaryAbsenceCalendar in database.TemporaryAbsenceCalendar
                                  where (temporaryAbsenceCalendar.IDEmployee == employeeId)
                                  orderby(temporaryAbsenceCalendar.StartDate)
                                  select new CalandarModel
                                  {
                                      IDCalendar = temporaryAbsenceCalendar.IDTemporaryAbsenceCalendar,
                                      Title = temporaryAbsenceCalendar.Title,
                                      Description = temporaryAbsenceCalendar.Description,
                                      StartDate = temporaryAbsenceCalendar.StartDate,
                                      EndDate = temporaryAbsenceCalendar.EndDate,
                                  }).ToListAsync();

            if (calendar == null)
            {
                return BadRequest();
            }

            return Ok(calendar);
        }

        //Удаление
        [HttpPost("RemoveCalendar/TraningCalendar/{calendarId}")]
        public async Task<IActionResult> RemoveTraningCalendar(int calendarId)
        {
            var removeCalendar = await database.TraningCalendar.FirstOrDefaultAsync(x => x.IDTraningCalendar == calendarId);

            if (removeCalendar != null)
            {
                database.TraningCalendar.Remove(removeCalendar);
                
                await database.SaveChangesAsync();

                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("RemoveCalendar/VacationCalendar/{calendarId}")]
        public async Task<IActionResult> RemoveVacationCalendar(int calendarId)
        {
            var removeCalendar = await database.VacationCalendar.FirstOrDefaultAsync(x => x.IDVacationCalendar == calendarId);

            if (removeCalendar != null)
            {
                database.VacationCalendar.Remove(removeCalendar);
                
                await database.SaveChangesAsync();

                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("RemoveCalendar/TemporaryAbsenceCalendar/{calendarId}")]
        public async Task<IActionResult> RemoveTemporaryAbsenceCalendar(int calendarId)
        {
            var removeCalendar = await database.TemporaryAbsenceCalendar.FirstOrDefaultAsync(x => x.IDTemporaryAbsenceCalendar == calendarId);

            if (removeCalendar != null)
            {
                database.TemporaryAbsenceCalendar.Remove(removeCalendar);

                await database.SaveChangesAsync();

                return Ok();
            }

            return BadRequest();
        }

        // Добавление
        [HttpPost("AddCalendar/TraningCalendar")]
        public async Task<IActionResult> AddTraningCalendar([FromBody] AddCalendarModel calendar)
        {
            var newCalendar = new TraningCalendar();

            newCalendar.IDEmployee = calendar.IDEmployee;
            newCalendar.Title = calendar.Title;
            newCalendar.Description = calendar.Description;
            newCalendar.StartDate = calendar.StartDate;
            newCalendar.EndDate = calendar.EndDate;

            database.TraningCalendar.Add(newCalendar);
            await database.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("AddCalendar/VacationCalendar")]
        public async Task<IActionResult> AddVacationCalendar([FromBody] AddCalendarModel calendar)
        {
            var newCalendar = new VacationCalendar();

            newCalendar.IDEmployee = calendar.IDEmployee;
            newCalendar.Title = calendar.Title;
            newCalendar.Description = calendar.Description;
            newCalendar.StartDate = calendar.StartDate;
            newCalendar.EndDate = calendar.EndDate;

            database.VacationCalendar.Add(newCalendar);
            await database.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("AddCalendar/TemporaryAbsenceCalendar")]
        public async Task<IActionResult> AddTemporaryAbsenceCalendar([FromBody] AddCalendarModel calendar)
        {
            var newCalendar = new TemporaryAbsenceCalendar();

            newCalendar.IDEmployee = calendar.IDEmployee;
            newCalendar.Title = calendar.Title;
            newCalendar.Description = calendar.Description;
            newCalendar.StartDate = calendar.StartDate;
            newCalendar.EndDate = calendar.EndDate;

            database.TemporaryAbsenceCalendar.Add(newCalendar);
            await database.SaveChangesAsync();

            return Ok();
        }
    }
}
