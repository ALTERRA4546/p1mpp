using Android.App;
using Android.Content;
using Newtonsoft.Json;
using RoadsOfRussiaMobile.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace RoadsOfRussiaMobile
{
    public partial class MainPage : ContentPage
    {
        private RoadsOfRussiaDLL.Mobile.MobileController mobileController;

        public MainPage()
        {
            InitializeComponent();
            mobileController = new RoadsOfRussiaDLL.Mobile.MobileController();

            UpdateNews();
            UpdateEvents();
        }

        private async void NewsButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                EventsTab.IsVisible = false;
                NewsTab.IsVisible = true;

                UpdateNews();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "Ok");
            }
        }

        private async void EventsButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                EventsTab.IsVisible = true;
                NewsTab.IsVisible = false;

                UpdateEvents();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "Ok");
            }
        }

        private async void UpdateNews()
        {
            try
            {
                var currentIndex = NewsCollectionView.SelectedItem != null ? ((List<NewsModel>)NewsCollectionView.ItemsSource).IndexOf((NewsModel)NewsCollectionView.SelectedItem) : -1;

                var newsData = await mobileController.GetNews();
                List<NewsModel> newsList = new List<NewsModel>();


                foreach (var news in newsData)
                {
                    var newsTemplate = new NewsModel();

                    newsTemplate.IDNews = news.IDNews;
                    newsTemplate.Title = news.Title;
                    newsTemplate.Description = news.Description;
                    newsTemplate.PositiveVote = news.PositiveVote;
                    newsTemplate.NegativeVote = news.NegativeVote;
                    newsTemplate.Date = news.Date.ToString("dd.MM.yyyy");
                    if (news.Image != null)
                    {
                        newsTemplate.Image = ImageSource.FromStream(() => new MemoryStream(news.Image));
                    }

                    newsList.Add(newsTemplate);
                }

                NewsCollectionView.ItemsSource = newsList;

                if (currentIndex >= 0 && currentIndex < newsList.Count)
                {
                    NewsCollectionView.ScrollTo(newsList[currentIndex], position: ScrollToPosition.Start, animate: false);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "Ok");
            }
        }

        private async void UpdateEvents()
        {
            try
            {
                var eventsData = await mobileController.GetEvents();
                List<EventsModel> eventsList = new List<EventsModel>();

                foreach (var events in eventsData)
                {
                    var eventsTemplate = new EventsModel();

                    eventsTemplate.IDEvent = events.IDEvent;
                    eventsTemplate.Title = events.Title;
                    eventsTemplate.Description = events.Description;
                    eventsTemplate.RawDate = events.Date;
                    eventsTemplate.Date = events.Date.ToString("dd.MM.yyyy");
                    eventsTemplate.Author = events.Author;

                    eventsList.Add(eventsTemplate);
                }

                EventsCollectionView.ItemsSource = eventsList;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "Ok");
            }
        }

        private async void NewsCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.CurrentSelection.Count > 0)
                {
                    var selectedNews = NewsCollectionView.SelectedItem as NewsModel;

                    if (selectedNews != null)
                    {
                        string action = await DisplayActionSheet("Выберите действие", "Отмена", null, "❤", "💢");

                        switch (action)
                        {
                            case "❤":
                                await mobileController.NewsPositiveVote(selectedNews.IDNews);
                                UpdateNews();
                                break;

                            case "💢":
                                await mobileController.NewsNegativeVote(selectedNews.IDNews);
                                UpdateNews();
                                break;
                        }

                        ((CollectionView)sender).SelectedItem = null;
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "Ok");
            }
        }

        private async void EventsCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.CurrentSelection.Count > 0)
                {
                    var selectedEvents = EventsCollectionView.SelectedItem as EventsModel;

                    if (selectedEvents != null)
                    {
                        Intent intent = new Intent(Intent.ActionInsert);
                        intent.SetData(Android.Provider.CalendarContract.Events.ContentUri);
                        intent.PutExtra(Android.Provider.CalendarContract.ExtraEventBeginTime, GetMilliseconds(selectedEvents.RawDate));
                        intent.PutExtra(Android.Provider.CalendarContract.ExtraEventEndTime, GetMilliseconds(selectedEvents.RawDate.AddHours(1)));
                        intent.PutExtra(Android.Provider.CalendarContract.EventsColumns.Title, selectedEvents.Title);
                        intent.PutExtra(Android.Provider.CalendarContract.EventsColumns.Description, selectedEvents.Description);

                        intent.AddFlags(ActivityFlags.NewTask);

                        Android.App.Application.Context.StartActivity(intent);


                        ((CollectionView)sender).SelectedItem = null;
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", ex.Message, "Ok");
            }
        }

        private static long GetMilliseconds(DateTime date)
        {
            return (long)(date.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }
    }
}
