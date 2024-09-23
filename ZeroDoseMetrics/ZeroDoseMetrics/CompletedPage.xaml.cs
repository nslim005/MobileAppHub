using System;
using System.Collections.Generic;
using SQLite;
using Xamarin.Forms;
using ZeroDoseMetrics.Model;

namespace ZeroDoseMetrics
{	
	public partial class CompletedPage : ContentPage
	{	
		public CompletedPage ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<LineList>();
                var linelists = conn.Table<LineList>().Where(x=>x.Completed == 1).ToList();
                ChildrenLineList.ItemsSource = linelists;
            }

        }
    }
}

