using Newtonsoft.Json;
using RoadsOfRussiaDLL.Mobile.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RoadsOfRussiaDLL.Mobile
{
    public class MobileController
    {
        // Новости
        public async Task<List<NewsModel>> GetNews()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string apiUrl = "http://10.0.2.2:5246/api/Mobile/News";
                var response = await httpClient.GetStringAsync(apiUrl);

                var newsData = JsonConvert.DeserializeObject<List<NewsModel>>(response);
                return newsData ?? new List<NewsModel>();
            }
        }

        // События
        public async Task<List<EventsModel>> GetEvents()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string apiUrl = $"http://10.0.2.2:5246/api/Mobile/Events";
                var response = await httpClient.GetStringAsync(apiUrl);

                var eventsData = JsonConvert.DeserializeObject<List<EventsModel>>(response);
                return eventsData ?? new List<EventsModel>();
            }
        }

        // Положительный отзыв на новость
        public async Task<bool> NewsPositiveVote(int newsId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string apiUrl = $"http://10.0.2.2:5246/api/Mobile/News/PositiveVote/{newsId}";
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

        // Отрицательный отзыв на новость
        public async Task<bool> NewsNegativeVote(int newsId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string apiUrl = $"http://10.0.2.2:5246/api/Mobile/News/NegativeVote/{newsId}";
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
    }
}
