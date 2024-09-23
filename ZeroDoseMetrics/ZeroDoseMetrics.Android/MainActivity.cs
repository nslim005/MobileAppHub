using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using System.IO;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Essentials;

namespace ZeroDoseMetrics.Droid
{
    [Activity(Label = "ZeroDoseMetrics", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);

            string dbName = "OOSDB.sqlite";
            var documentPath = FileSystem.AppDataDirectory;

            var folderPath = Path.Combine(documentPath, "OOSDATA");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fullPath = Path.Combine(folderPath, dbName);

            //string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            //string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            //string fullPath = Path.Combine(folderPath, dbName);
            //string fullPath = Path.Combine("/storage/emulated/Download", dbName);

            //var fullPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDocuments, "IEVDATA");
            // Ensure the directory exists
            //if (!Directory.Exists(fullPath))
            //{
            //    Directory.CreateDirectory(fullPath);
            //}

            // Specify the database file path
            //var dbPath = Path.Combine(fullPath, dbName);

            LoadApplication(new App(fullPath));

            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize); // added for screen size scrolling, etc
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
