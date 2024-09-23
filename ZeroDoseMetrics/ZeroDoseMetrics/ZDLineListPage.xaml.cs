using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ZeroDoseMetrics.Model;

namespace ZeroDoseMetrics
{	
	public partial class ZDLineListPage : TabbedPage
	{
		Login user;

		public ZDLineListPage (Login login)
		{
			InitializeComponent ();
			this.user = login;
		}

        void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
			Navigation.PushAsync(new RegisterNewChildPage(user));
        }
    }
}

