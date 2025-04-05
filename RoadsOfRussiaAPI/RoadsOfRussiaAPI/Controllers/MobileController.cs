using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoadsOfRussiaAPI.Controllers.Model;
using RoadsOfRussiaAPI.DbModel;

namespace RoadsOfRussiaAPI.Controllers
{
    [Route("api/Mobile")]
    [ApiController]
    public class MobileController : ControllerBase
    {
        private readonly ApplicationDbContext database;

        public MobileController(ApplicationDbContext database)
        {
            this.database = database;
        }

        // Новости
        // Получение новости
        [HttpGet("News")]
        public async Task<IActionResult> GetNews()
        {
            try
            {
                var newsData = await (from news in database.News
                                      select new NewsModel
                                      {
                                          IDNews = news.IDNews,
                                          Title = news.Title,
                                          Description = news.Description,
                                          Image = news.Image,
                                          PositiveVote = news.PositiveVote,
                                          NegativeVote = news.NegativeVote,
                                          Date = news.Date,
                                      }).ToListAsync();

                if (newsData.Count == 0)
                {
                    return NotFound(new { Message = "News not found" });
                }

                return Ok(newsData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Положительный голос
        [HttpPost("News/PositiveVote/{newsId}")]
        public async Task<IActionResult> NewsPositiveVote(int newsId)
        {
            try
            {
                var newsData = await database.News.FirstOrDefaultAsync(x => x.IDNews == newsId);

                if (newsData != null)
                {
                    newsData.PositiveVote++;

                    database.SaveChanges();

                    return Ok();
                }
                else
                {
                    return NotFound(new { Message = "News not found" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Негативный голос
        [HttpPost("News/NegativeVote/{newsId}")]
        public async Task<IActionResult> NewsNegativeVote(int newsId)
        {
            try
            {
                var newsData = await database.News.FirstOrDefaultAsync(x => x.IDNews == newsId);

                if (newsData != null)
                {
                    newsData.NegativeVote++;

                    database.SaveChanges();

                    return Ok();
                }
                else
                {
                    return NotFound(new { Message = "News not found" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // События
        [HttpGet("Events")]
        public async Task<IActionResult> GetEvents()
        {
            try
            {
                var eventsData = await (from events in database.Event
                                        join employee in database.Employee on events.IDResponsible equals employee.IDEmployee
                                        orderby events.StartDate
                                        select new EventsModel
                                        {
                                            IDEvent = events.IDEvent,
                                            Title = events.Title,
                                            Description = events.Description,
                                            Date = events.StartDate,
                                            Author = employee.Surname + " " + employee.Name + " " + employee.Patronimyc
                                        }).ToListAsync();

                if (eventsData.Count == 0)
                {
                    return NotFound(new { Message = "Events not found" });
                }

                return Ok(eventsData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
