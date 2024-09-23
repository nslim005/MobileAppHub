using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZeroDoseMetrics.Model;

namespace ZeroDoseMetrics.OOSZamfara
{	
	public partial class ChildrenLineListPage : ContentPage
	{
        private ActivityIndicator _activityIndicator; 
        private ObservableCollection<OOSList> Item;
        public string TeamCode { get; set; }
        public string Settlement { get; set; }
        public string HealthFacility { get; set; }
        public string InterviewerName { get; set; }
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }

        public Login helper { get; set; }

        public ChildrenLineListPage (Login login)
		{
			InitializeComponent ();
            Item = new ObservableCollection<OOSList>();
            ChildrenLineList.ItemsSource = Item;
            this.TeamCode = login.TeamCode;
            this.Settlement = login.Settlement;
            this.HealthFacility = login.HealthFacility;
            this.PhoneNumber = login.PhoneNo;
            this.InterviewerName = login.InterviewerName;
            helper = new Login();                                               
            downloadLineList.Toggled += onToggleDownload; // Add an event handler for the Toggled event
            // Create the activity indicator
            _activityIndicator = new ActivityIndicator
            {
                IsRunning = true,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            OnAppearing();
        }


        async private void onToggleDownload(object sender, ToggledEventArgs e)
        {
            // Add the activity indicator to the page
            this.Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children = {
                _activityIndicator,
                new Label { Text = "Line list Download started..." }
            }
            };
            try
            {
                
                // This method will be called whenever the switch is toggled
                if (e.Value) // Switch is On
                {
                    // Begin make the call to pull linelist

                    string apiUrl = "http://cloudbits.com.ng/DEMOAPI/igetLineList.php";
                    string teamCode = TeamCode;
                    string settlement = Settlement;

                    var client = new HttpClient();
                    var content = new StringContent(JsonConvert.SerializeObject(new
                    {
                        TeamCode = teamCode,
                        SettlementName = settlement
                    }), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(apiUrl, content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    List<OOSList> OOSData = JsonConvert.DeserializeObject<List<OOSList>>(responseString);

                    using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                    {

                        //process data here
                        foreach (var record in OOSData)
                        {

                            //check if data exists, else add
                            conn.CreateTable<OOSList>();
                            var oos = conn.Table<OOSList>()
                                .Where(x => x.ChildID == record.ChildID && x.Completed == 0).OrderByDescending(x => x.Id).ToList(); // updated after stable version

                            if (oos.Count > 0)
                            {

                            }
                            else
                            {
                                //add to the database
                                OOSList ooslist = new OOSList
                                {

                                    VaccinatorName = record.VaccinatorName.Trim(),
                                    VaccinatorNumber = record.VaccinatorNumber.Trim(),
                                    TeamCode = record.TeamCode.Trim(),
                                    Respondent = record.Respondent.Trim(),
                                    HouseHoldHeadName = record.HouseHoldHeadName.Trim(),
                                    HouseHoldPhone = record.HouseHoldPhone.Trim(),
                                    CaregiverName = record.CaregiverName.Trim(),
                                    ChildID = record.ChildID.Trim(),
                                    ChildName = record.ChildName.Trim(),
                                    Gender = record.Gender.Trim(),
                                    HasReceivedAntigen = record.HasReceivedAntigen.Trim(),
                                    HasVaccinationCard = record.HasVaccinationCard.Trim(),
                                    OldAntigensReceived = record.OldAntigensReceived.Trim(),
                                    AntigensReceived = record.AntigensReceived.Trim(),
                                    ChildEnumeratedByAfenet = "",
                                    AEFI = record.AEFI.Trim(),
                                    AEFIType = record.AEFIType.Trim(),
                                    Age = record.Age.Trim(),
                                    CurrentAge = record.CurrentAge.Trim(),
                                    AgeCategory = record.AgeCategory.Trim(),
                                    CaregiverNumber = record.CaregiverNumber.Trim(),
                                    CatchmentAreaHF = record.CatchmentAreaHF.Trim(),
                                    SettlementName = record.SettlementName.Trim(),
                                    LGA = record.LGA,
                                    Ward = record.Ward,
                                    SettlementType = record.SettlementType,
                                    Latitude = record.Latitude,
                                    Longitude = record.Longitude,
                                    Completed = 0,
                                    Date = "",
                                    Time = "",
                                    Temp = "",
                                    DueForNextAntigen = record.DueForNextAntigen,
                                    isCheckedForSync = 0,
                                    uploaded = 0,
                                    VaccinationStatus = record.VaccinationStatus,
                                    TargetStatus = record.TargetStatus,

                                };
                                int rows = conn.Insert(ooslist);

                            }

                        }

                        //End make the call to pull linelist

                        conn.CreateTable<OOSList>();
                        var newoos = conn.Table<OOSList>().ToList();
                        if(newoos.Count > 0)
                        {
                            this.Content = new StackLayout
                            {
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Center,
                                Children = {
                            new Label { Text = "Line list Download Completed..."},
                        }
                            };
                        }
                        else
                        {
                            this.Content = new StackLayout
                            {
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Center,
                                Children = {
                            new Label { Text = "No Record to download"},
                        }
                            };
                        }

                    }

                    // Clear feedback after a delay
                    await Task.Delay(3000); // Optional delay before clearing message
                    this.Content = null;

                    Login model = new Login
                    {
                        InterviewerName = InterviewerName,
                        PhoneNo = PhoneNumber,
                        TeamCode = teamCode,
                        HealthFacility = HealthFacility,
                        Settlement = Settlement
                    };

                    await Navigation.PushAsync(new ChildrenLineListPage(model));
                }
                else // Switch is Off
                {
                    //do nothing. This is switching the toggle off
                }
            }
            catch (HttpRequestException ex)
            {
                // This catches the Java.Net.ConnectException and shows a friendly message
                feedbackLabel.Text = "Network error: Unable to connect to the server. Please check your internet connection.";
                await DisplayAlert("", "", "");
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                // Catch lower-level socket errors
                feedbackLabel.Text = "Network Error: Network connectivity issue. Please try again later.";
            }
            catch (Exception ex)
            {
                // Generic catch for any other types of exceptions
                //feedbackLabel.Text = $"Error: {ex.Message}";
                feedbackLabel.Text = "Network error: Unable to connect to the server. Please check your internet connection.";
            }


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<OOSList>();
                //var linelists = conn.Table<OOSList>().ToList();
                var linelists = conn.Table<OOSList>()
                    .Where(x => x.TeamCode == TeamCode && x.SettlementName == Settlement && x.Completed == 0).OrderByDescending(x => x.Id).ToList(); // updated after stable version

                string totalRecord = linelists.Count.ToString();
                ChildrenLineList.ItemsSource = linelists;
                countTotalLbl.Text = totalRecord + " U2 Children";
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
                NavigationMode = NavigationMode.Default
            }) ;

        }


        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var button = sender as Button;
            var item = button?.CommandParameter as OOSList;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<OOSList>();
                OOSList record = conn.Table<OOSList>().Where(x => x.Id == item.Id).FirstOrDefault();


                decimal lat = Convert.ToDecimal(record.Latitude);
                decimal longi = Convert.ToDecimal(record.Longitude);

                GotoDirection(lat, longi);
            }

        }

        void Button_Vaccinate(System.Object sender, System.EventArgs e)
        {
            var button = sender as Button;
            var item = button?.CommandParameter as OOSList;

            helper.InterviewerName = InterviewerName;
            helper.PhoneNo = PhoneNumber;
            helper.TeamCode = TeamCode;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<OOSList>();
                OOSList record = conn.Table<OOSList>().Where(x => x.Id == item.Id).FirstOrDefault();
                record.Temp = TeamCode; //used to temporarily hold the teamcode of a logged in user

                Navigation.PushAsync(new VaccinationPage(record,helper));

            }
        }


        void vaccination_log(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new VaccinLogPage(PhoneNumber));
        }

        void EnumerateChild_Clicked(System.Object sender, System.EventArgs e)
        {
            Login newChildEnumeratorDetails = new Login();

            newChildEnumeratorDetails.InterviewerName = this.InterviewerName;
            newChildEnumeratorDetails.PhoneNo = this.PhoneNumber;
            newChildEnumeratorDetails.TeamCode = this.TeamCode;
            newChildEnumeratorDetails.HealthFacility = this.HealthFacility;
            newChildEnumeratorDetails.Settlement = this.Settlement;

            Navigation.PushAsync(new NewEnumeratePage(newChildEnumeratorDetails));
        }

    }


    public class LoginHelper
    {
        public string InterviewerName { get; set; }

        public string PhoneNo { get; set; }

        public string TeamCode { get; set; }

        public string HealthFacility { get; set; }

        public string Settlement { get; set; }
    }
}

