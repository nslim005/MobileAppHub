using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using ZeroDoseMetrics.Model;
using static Xamarin.Forms.Internals.GIFBitmap;

namespace ZeroDoseMetrics.OOSZamfara
{	
	public partial class VaccinLogPage : ContentPage
	{
        public string InterviewerNo { get; set; }

        public string Code { get; set; }

		public VaccinLogPage (string interviewerNo)
		{
			InitializeComponent ();
            InterviewerNo = interviewerNo;
            OnAppearing();
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<OOSList>();
                var linelists = conn.Table<OOSList>().Where(x => x.Completed == 1 && x.VaccinatorNumber == InterviewerNo && x.uploaded ==0).ToList();
                ChildrenLineList.ItemsSource = linelists;

            }

        }


        void Review_Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var button = sender as Button;
            var item = button?.CommandParameter as OOSList;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {

                //Navigation.PushAsync(new VaccinationPage(record, helper));

            }
        }

        void Synchronize_Clicked(System.Object sender, System.EventArgs e)
        {
            string PhoneNo = InterviewerNo;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<OOSList>();
                var ret = conn.Table<OOSList>().Where(x => x.VaccinatorNumber == PhoneNo && x.Completed == 1 && x.isCheckedForSync == 1 && x.uploaded == 0).ToList();
                //int rows = conn.Update(ret);
                if(ret.Count > 0)
                {
                    Synchronize(ret);
                    if(Code == "200")
                    {
                        foreach (var row in ret)
                        {
                            row.uploaded = 1;
                            conn.Update(row);
                        }

                        DisplayAlert("Success", "Vaccination Record Synchronized successfully", "OK");
                        OnAppearing();
                    }
                    else
                    {
                        DisplayAlert("Success", "Ensure your network is Good and try again.", "OK");
                    }

                }
                else
                {
                    DisplayAlert("Error", "Select atleast one record to synchronize", "OK");
                }

            }

        }


        private async void Synchronize(List<OOSList> list)
        {
            //BEGIN API CALL
            try
            {
                string jsonString = System.Text.Json.JsonSerializer.Serialize(list);

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "http://cloudbits.com.ng/DEMOAPI/create.php");
                var content = new StringContent(jsonString, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                //var statCode = response.StatusCode;
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                Code = await response.Content.ReadAsStringAsync();
                //End API CALL
            }
            catch (Exception ex)
            {
                var message = ex.Message.ToUpper();
            }
            
        }



        void checkedForSynchronization_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var box = sender as CheckBox;
            var item = box?.BindingContext as OOSList;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {

                if(box.IsChecked == false)
                {
                    conn.CreateTable<OOSList>();
                    var ret = conn.Table<OOSList>().Where(x => x.Id == item.Id).First();
                    ret.isCheckedForSync = 0;
                    int rows = conn.Update(ret);
                }
                if(box.IsChecked == true)
                {
                    
                    conn.CreateTable<OOSList>();
                    var ret = conn.Table<OOSList>().Where(x => x.Id == item.Id).First();
                    ret.isCheckedForSync = 1;
                    int rows = conn.Update(ret);
                }
                
                
            }

            //OnAppearing();

        }

       

    }
}

