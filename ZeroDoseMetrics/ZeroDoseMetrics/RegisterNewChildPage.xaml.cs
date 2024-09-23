using System;
using System.Collections.Generic;
using System.IO;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using ZeroDoseMetrics.Model;

namespace ZeroDoseMetrics
{
    public partial class RegisterNewChildPage : ContentPage
    {
        Login login;
        public RegisterNewChildPage(Login user)
        {
            InitializeComponent();
            this.login = user;
        }

        private void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            LineList model = new LineList()
            {
                CaregiverName = caregiverNameEntry.Text,
                CaregiverContact = caregiverContactEntry.Text,
                ChildName = childNameEntry.Text,
                ChildAge = childAgeEntry.Text,
                SettlementName = settlementEntry.Text,
                StructureID = structureEntry.Text,
                //Latitude = Convert.ToDouble(latitudeEntry.Text),
                //Longitude = Convert.ToDouble(longitudeEntry.Text),
                PhoneNo = login.PhoneNo,
                InterviewerName = login.InterviewerName,
                TeamCode = login.TeamCode
            };


            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<LineList>();
                int rows = conn.Insert(model);

                if (rows > 0)
                {
                    DisplayAlert("Success", "Child Record successfully saved", "OK");
                }
                else
                {
                    DisplayAlert("Failure", "Child Record successfully saved", "OK");
                }

            }
        }

        void HandleDelete_Clicked(System.Object sender, System.EventArgs e)
        {
          

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<LineList>();
                var rows = conn.Table<LineList>().ToList() ;
                    
                for(int i = 0; i <= rows.Count; i++)
                {
                    conn.Delete(rows[i]);
                }
                DisplayAlert("Success", "Record Cleared Successfully", "ok");

                //if (rows.Count > 0)
                //{
                //    conn.DeleteAll(rows);

                    
                //}



            }

        }

        void Export_Clicked(System.Object sender, System.EventArgs e)
        {
            ExportDatabase();

        }

       

        //public void ExportDatabase()
        //{
        //    string dbName = "ZeroDoseMetrics.sqlite";
        //    string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbName);
        //    string backupFolderPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "ZeroDoseMetricsFolder");
        //    //System.IO.Path.Combine((string)Android.OS.Environment.ExternalStorageDirectory, "FolderPath");

        //    if (!Directory.Exists(backupFolderPath))
        //    {
        //        Directory.CreateDirectory(backupFolderPath);
        //    }

        //    string backupFilePath = Path.Combine(backupFolderPath, dbName);

        //    try
        //    {
        //        File.Copy(dbPath, backupFilePath, true);
        //        Console.WriteLine("Database exported successfully to: " + backupFilePath);
        //    }
        //    catch (IOException e)
        //    {
        //        Console.WriteLine("Error exporting database: " + e.Message);
        //    }
        //}

        private async void ExportDatabase()
        {
            //var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ZeroDoseMetrics.sqlite");
            string dbName = "ZeroDoseMetrics.sqlite";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string fullPath = Path.Combine(folderPath, dbName);
            var destinationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ZeroDoseMetrics_export.sqlite");


           


            File.Copy(fullPath, destinationPath, true);
            await DisplayAlert("Export Successful", $"Database exported to {destinationPath}", "OK");
        }
    }
}

