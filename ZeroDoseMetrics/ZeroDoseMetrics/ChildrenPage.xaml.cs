using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Odbc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MySqlConnector;
using Newtonsoft.Json;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZeroDoseMetrics.Model;
using System.Text.Json;

namespace ZeroDoseMetrics
{	
	public partial class ChildrenPage : ContentPage
	{
        private ObservableCollection<LineList> Item;

        public string TeamCode { get; set; }

        public ChildrenPage (string teamCode)
		{
			InitializeComponent ();

            Item = new ObservableCollection<LineList>();
            ChildrenLineList.ItemsSource = Item;
            this.TeamCode = teamCode;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //BEGIN GET From API

            //string baseAddress = "http://cloudbits.com.ng";

            //var baseAddr = new Uri(baseAddress);

            //var client = new HttpClient { BaseAddress = baseAddr };

            //var returnedData = await client.GetStringAsync("API/api.php");

            //var allReviews = JsonConvert.DeserializeObject<List<Login>>(returnedData);

            //END GET From API



            //BEGIN POST TO API

            //List<Login> login = new List<Login>
            //{
            //    new Login
            //    {
            //        InterviewerName = "Nurudeen S Quadri",
            //        PhoneNo = "08093946133",
            //        TeamCode = "01"
            //    },
            //    new Login
            //    {
            //        InterviewerName = "Neemah N Suleiman",
            //        PhoneNo = "08067345903",
            //        TeamCode = "03"
            //    },
            //    new Login
            //    {
            //        InterviewerName = "AbduLLAHI Suleiman",
            //        PhoneNo = "0812345456",
            //        TeamCode = "02"
            //    }
            //};

            

            //string jsonString = System.Text.Json.JsonSerializer.Serialize(login);

            //var client = new HttpClient();
            //var request = new HttpRequestMessage(HttpMethod.Post, "http://cloudbits.com.ng/API/create.php");
            //var content = new StringContent(jsonString, null, "application/json");
            //request.Content = content;
            //var response = await client.SendAsync(request);
            //response.EnsureSuccessStatusCode();
            //Console.WriteLine(await response.Content.ReadAsStringAsync());

            //END POST TO API

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<LineList>();
                var linelists = conn.Table<LineList>()
                    .Where(x => x.TeamCode.Equals(TeamCode)).ToList();

                ChildrenLineList.ItemsSource = linelists;
            }

        }

        void ChildrenLineList_SelectionChanged(System.Object sender, Xamarin.Forms.SelectionChangedEventArgs e)
        {

            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                var selectedModel = e.CurrentSelection[0] as LineList;

                if (selectedModel != null)
                {
                    //DisplayAlert("Item Selected", $"Name: {selectedModel.CaregiverName}\nDescription: {selectedModel.ChildName}", "OK");

                    Navigation.PushAsync(new MapPage(selectedModel));
                }
            }
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var button = sender as Button;
            var item = button?.CommandParameter as LineList;

            //DisplayAlert("Output", item.CaregiverName, "OK");

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<LineList>();
                LineList record = conn.Table<LineList>().Where(x=>x.Id == item.Id).FirstOrDefault();


                decimal lat = Convert.ToDecimal(record.Latitude);
                decimal longi = Convert.ToDecimal(record.Longitude);

                GotoDirection(lat,longi);
            }

        }

        void Button_Assess(System.Object sender, System.EventArgs e)
        {
            var button = sender as Button;
            var item = button?.CommandParameter as LineList;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<LineList>();
                LineList record = conn.Table<LineList>().Where(x => x.Id == item.Id).FirstOrDefault();
                record.HaveVaccinationCard = TeamCode; //used to temporarily hold the teamcode of a logged in user

                Navigation.PushAsync(new AssessmentPage(record));

            }
        }


        async public void GotoDirection(decimal lat, decimal longi)
        {
            double latitude = Convert.ToDouble(lat);
            double longitude = Convert.ToDouble(longi);
            //await Map.OpenAsync(12.97360882, 7.570604637);

            await Map.OpenAsync(latitude, longitude, new MapLaunchOptions
            {
                Name = "Name here",
                NavigationMode = NavigationMode.Driving
            });

        }
    }
}

