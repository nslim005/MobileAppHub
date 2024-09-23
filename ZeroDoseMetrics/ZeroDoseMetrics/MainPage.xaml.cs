using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using ZeroDoseMetrics.Model;
using ZeroDoseMetrics.OOSZamfara;

namespace ZeroDoseMetrics
{
    public partial class MainPage : ContentPage
    {
        Login isExist;
        OOSList localDBRecord;
        public MainPage()
        {
            InitializeComponent();
            isExist = new Login();
            localDBRecord = new OOSList();
        }


        protected override void OnAppearing()
        {   
            base.OnAppearing();
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<LGA>();
                var LGARecords = GetLGA();
                foreach (var item in LGARecords)
                {
                    HFPickerLGA.Items.Add(item.LGAName);
                }

            }
            
        }


        public List<CAHF> GetCAHF()
        {
            List<CAHF> list;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                list = conn.Table<CAHF>().ToList();
            }

            return list;
        }

        public List<LGA> GetLGA()
        {
            List<LGA> list;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                list = conn.Table<LGA>().ToList();
            }

            return list;
        }

        public List<Ward> GetWard()
        {
            List<Ward> list;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                list = conn.Table<Ward>().ToList();
            }

            return list;
        }

        public List<Team> GetTeam()
        {
            List<Team> list;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                list = conn.Table<Team>().ToList();
            }

            return list;
        }

        public List<Settlement> GetSettlement()
        {
            List<Settlement> list;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                list = conn.Table<Settlement>().ToList();
            }

            return list;
        }

        async void ImageButton_Clicked(System.Object sender, System.EventArgs e)
        {

            //begin mirrow all the tables online
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {

                using (var client = new HttpClient())
                {
                    try
                    {
                        //check if the table exists already else create
                        conn.CreateTable<CAHF>();
                        conn.CreateTable<LGA>();
                        conn.CreateTable<Settlement>();
                        conn.CreateTable<State>();
                        conn.CreateTable<Team>();
                        conn.CreateTable<Ward>();

                        string CAHFurl = "http://cloudbits.com.ng/DEMOAPI/getCAHF.php";
                        string LGAurl = "http://cloudbits.com.ng/DEMOAPI/getLGA.php";
                        string Settlementurl = "http://cloudbits.com.ng/DEMOAPI/getSettlement.php";
                        string Stateurl = "http://cloudbits.com.ng/DEMOAPI/getState.php";
                        string Teamurl = "http://cloudbits.com.ng/DEMOAPI/getTeam.php";
                        string Wardurl = "http://cloudbits.com.ng/DEMOAPI/getWard.php";


                        //Delet and recreate all the tables
                        // Drop the table
                        conn.Execute("DROP TABLE IF EXISTS CAHF");
                        conn.Execute("DROP TABLE IF EXISTS LGA");
                        conn.Execute("DROP TABLE IF EXISTS Settlement");
                        conn.Execute("DROP TABLE IF EXISTS State");
                        conn.Execute("DROP TABLE IF EXISTS Team");
                        conn.Execute("DROP TABLE IF EXISTS Ward");

                        //Recreate table
                        conn.CreateTable<CAHF>();
                        conn.CreateTable<LGA>();
                        conn.CreateTable<Settlement>();
                        conn.CreateTable<State>();
                        conn.CreateTable<Team>();
                        conn.CreateTable<Ward>();


                        // Show loading spinner and message
                        activityIndicator.IsRunning = true;
                        activityIndicator.IsVisible = true;
                        feedbackLabel.Text = "Setting up, please wait...";
                        //CAHF DATA//
                        HttpResponseMessage CAHFresponse = await client.GetAsync(CAHFurl);
                        CAHFresponse.EnsureSuccessStatusCode();
                        string CAHFresponseBody = await CAHFresponse.Content.ReadAsStringAsync();

                        // Deserialize the JSON response
                        List<CAHF> CAHFdata = JsonConvert.DeserializeObject<List<CAHF>>(CAHFresponseBody);


                        // Process the data

                        foreach (var record in CAHFdata)
                        {
                            CAHF cahf = new CAHF
                            {
                                LGAId = record.LGAId,
                                WardId = record.WardId,
                                Status = record.Status,
                                CAHFName = record.CAHFName

                            };
                            int rows = conn.Insert(cahf);
                        }

                        //LGA DATA//
                        HttpResponseMessage LGAresponse = await client.GetAsync(LGAurl);
                        LGAresponse.EnsureSuccessStatusCode();
                        string LGAresponseBody = await LGAresponse.Content.ReadAsStringAsync();

                        // Deserialize the JSON response
                        List<LGA> LGAdata = JsonConvert.DeserializeObject<List<LGA>>(LGAresponseBody);

                        // Process the data
                        
                        foreach (var record in LGAdata)
                        {
                            LGA lga = new LGA
                            {
                                StateId = record.StateId,
                                LGAName = record.LGAName,
                                Status = record.Status
                            };
                            int rows = conn.Insert(lga);

                            
                        }
                        var ret = conn.Table<LGA>().ToList();
                        foreach (var item in ret)
                        {
                            HFPickerLGA.Items.Add(item.LGAName);
                        }


                        //Settlement DATA//
                        HttpResponseMessage Settlementresponse = await client.GetAsync(Settlementurl);
                        Settlementresponse.EnsureSuccessStatusCode();
                        string SettlementresponseBody = await Settlementresponse.Content.ReadAsStringAsync();

                        // Deserialize the JSON response
                        List<Settlement> Settlementdata = JsonConvert.DeserializeObject<List<Settlement>>(SettlementresponseBody);

                        // Process the data
                        
                        foreach (var record in Settlementdata)
                        {
                            Settlement settlement = new Settlement
                            {
                                LGAId = record.LGAId,
                                SettlementName = record.SettlementName,
                                TeamId = record.TeamId,
                                WardId = record.WardId,
                                CAHFId = record.CAHFId,
                                Status = record.Status
                            };
                            int rows = conn.Insert(settlement);
                        }

                        //State DATA//
                        HttpResponseMessage Stateresponse = await client.GetAsync(Stateurl);
                        Stateresponse.EnsureSuccessStatusCode();
                        string SateresponseBody = await Stateresponse.Content.ReadAsStringAsync();

                        // Deserialize the JSON response
                        List<State> Statedata = JsonConvert.DeserializeObject<List<State>>(SateresponseBody);

                        // Process the data
                        
                        foreach (var record in Statedata)
                        {
                            State state = new State
                            {
                                StateName = record.StateName,
                                Status = record.Status
                            };
                            int rows = conn.Insert(state);
                        }

                        //Team DATA//
                        HttpResponseMessage Teamresponse = await client.GetAsync(Teamurl);
                        Teamresponse.EnsureSuccessStatusCode();
                        string TeamresponseBody = await Teamresponse.Content.ReadAsStringAsync();

                        // Deserialize the JSON response
                        List<Team> Teamdata = JsonConvert.DeserializeObject<List<Team>>(TeamresponseBody);

                        // Process the data
                        
                        foreach (var record in Teamdata)
                        {
                            Team team = new Team
                            {
                                CAHFId = record.CAHFId,
                                LGAId = record.LGAId,
                                TeamCode = record.TeamCode,
                                WardId = record.WardId,
                                Status = record.Status
                            };
                            int rows = conn.Insert(team);
                        }

                        //Ward DATA//
                        HttpResponseMessage Wardresponse = await client.GetAsync(Wardurl);
                        Wardresponse.EnsureSuccessStatusCode();
                        string WardresponseBody = await Wardresponse.Content.ReadAsStringAsync();

                        // Deserialize the JSON response
                        List<Ward> Warddata = JsonConvert.DeserializeObject<List<Ward>>(WardresponseBody);

                        // Process the data
                        
                        foreach (var record in Warddata)
                        {
                            Ward ward = new Ward
                            {
                                LGAId = record.LGAId,
                                WardName = record.WardName,
                                Status = record.Status
                            };
                            int rows = conn.Insert(ward);
                        }

                        feedbackLabel.Text = "Setup completed.";
                    }
                    catch (HttpRequestException ex)
                    {
                        // This catches the Java.Net.ConnectException and shows a friendly message
                        feedbackLabel.Text = "Network error: Unable to connect to the server. Please check your internet connection.";
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
                    finally
                    {
                        // Hide loading spinner after the API call
                        activityIndicator.IsRunning = false;
                        activityIndicator.IsVisible = false;

                        // Clear feedback after a delay
                        await Task.Delay(3000); // Optional delay before clearing message
                        feedbackLabel.Text = "";
                    }

                }

            }

        }

        void loginButton_Clicked(System.Object sender, System.EventArgs e)
        {
           
            bool isInterviewerEmpty = string.IsNullOrEmpty(interviewerNameEntry.Text);
            bool isPhoneNumberEmpty = string.IsNullOrEmpty(interviewerPhoneNoEntry.Text);
            bool isTeamCodeEmpty = string.IsNullOrEmpty(TeamCodePicker.ToString());
            bool isHFEmpty = string.IsNullOrEmpty(HFPicker.ToString());
            bool isSettlementEmpty = string.IsNullOrEmpty(SettlementPicker.ToString());

            //validation

            if (isInterviewerEmpty || isPhoneNumberEmpty || isTeamCodeEmpty || isHFEmpty || isSettlementEmpty)
            {
                DisplayAlert("ERROR LOGIN", "ALL FIELDS ARE REQUIRED FOR LOGIN", "OK");
            }
            else if (!(interviewerPhoneNoEntry.Text.Length == 11))
            {
                DisplayAlert("ERROR PHONE NO", "PHONE NUMBER MUST BE 11 DIGITS", "OK");
            }
            else
            {
                string InterviewerName = interviewerNameEntry.Text.Trim().ToUpper();
                string interviewerPhoneNumber = interviewerPhoneNoEntry.Text;
                string teamCode = TeamCodePicker.SelectedItem.ToString();
                string catchmentAreaHF = HFPicker.SelectedItem.ToString();
                string settlementName = SettlementPicker.SelectedItem.ToString();

                Login model = new Login
                {
                    InterviewerName = InterviewerName,
                    PhoneNo = interviewerPhoneNumber,
                    TeamCode = teamCode,
                    HealthFacility = catchmentAreaHF,
                    Settlement = settlementName
                };

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    //check if the record exists already 
                    conn.CreateTable<Login>();
                    isExist = conn.Table<Login>()
                        .Where(x => x.PhoneNo.Equals(interviewerPhoneNumber) && x.InterviewerName.Equals(InterviewerName)).FirstOrDefault();

                        if(isExist != null)
                        {
                            Navigation.PushAsync(new ChildrenLineListPage(model));
                        }
                        else
                        {
                            //else create user
                            conn.CreateTable<Login>();
                            int rows = conn.Insert(model);
                            Navigation.PushAsync(new ChildrenLineListPage(model));
                        }   
                }
               
            }
        }


       

        //LGA Operation
        void HFPickerLGA_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var picker = sender as Picker;

            string sel = picker.SelectedItem.ToString();
            HFPickerWard.Items.Clear();
            HFPicker.Items.Clear();
            TeamCodePicker.Items.Clear();
            SettlementPicker.Items.Clear();
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                var selected = conn.Table<LGA>().Where(x => x.LGAName == sel).FirstOrDefault();

                List<Ward> lga = GetWard();
                foreach (var item in lga.Where(x=>x.LGAId == selected.Id))
                {
                    HFPickerWard.Items.Add(item.WardName);
                }
            }

        }

        // Ward Operation
        void HFPickerWard_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var picker = sender as Picker;

            if(picker.SelectedIndex == -1)
            {

            }
            else
            {
                string sel = picker.SelectedItem.ToString();
                HFPicker.Items.Clear();
                TeamCodePicker.Items.Clear();
                SettlementPicker.Items.Clear();
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    var selected = conn.Table<Ward>().Where(x => x.WardName == sel).FirstOrDefault();

                    List<CAHF> ward = GetCAHF().Where(x => x.WardId == selected.Id).ToList();

                    foreach (var item in ward) { 
                    
                        HFPicker.Items.Add(item.CAHFName);
                    }
                }
            }    
        }

        //CAHF Operation
        void HFPicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {

            var picker = sender as Picker;

            if (picker.SelectedIndex == -1)
            {

            }
            else
            {
                string sel = picker.SelectedItem.ToString();
                TeamCodePicker.Items.Clear();
                SettlementPicker.Items.Clear();
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    var selected = conn.Table<CAHF>().Where(x => x.CAHFName == sel).FirstOrDefault();

                    List<Team> cahf = GetTeam().Where(x => x.CAHFId == selected.Id).ToList();

                    foreach (var item in cahf)
                    {

                        TeamCodePicker.Items.Add(item.TeamCode);
                    }
                }
            }

        }

        //Team Operation
        void TeamCodePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {

            var picker = sender as Picker;

            if (picker.SelectedIndex == -1)
            {

            }
            else
            {
                string sel = picker.SelectedItem.ToString();
                SettlementPicker.Items.Clear();
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    var selected = conn.Table<Team>().Where(x => x.TeamCode == sel).FirstOrDefault();

                    List<Settlement> cahf = GetSettlement().Where(x => x.TeamId == selected.Id).ToList();

                    foreach (var item in cahf)
                    {

                        SettlementPicker.Items.Add(item.SettlementName);
                    }
                }
            }

        }

        void SettlementPicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {

        }       

    }
}

