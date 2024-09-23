using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZeroDoseMetrics.Model;

namespace ZeroDoseMetrics
{	
	public partial class MapPage : ContentPage
	{
		LineList linelist;

		public MapPage (LineList linelist)
		{
			InitializeComponent ();
            this.linelist = linelist;
			//DisplayAlert("Item Selected", $"Name: {linelist.CaregiverName}\nDescription: {linelist.ChildName}", "OK");
			double lat = Convert.ToDouble(linelist.Latitude);
			double longi = Convert.ToDouble(linelist.Longitude);
			GotoDirection(lat,longi);
			
        }

		async public void GotoDirection(double lat, double longi)
		{
			//double latitude = Convert.ToDouble(lat);
			//double longitude = Convert.ToDouble(longi);
			await Map.OpenAsync(lat, longi);

            //await Map.OpenAsync(lat, longi, new MapLaunchOptions
            //{
            //    Name = "Name here",
            //    NavigationMode = NavigationMode.Driving
            //});

        }
    }
}

