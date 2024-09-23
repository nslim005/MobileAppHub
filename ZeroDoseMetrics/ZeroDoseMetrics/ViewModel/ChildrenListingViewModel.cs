using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ZeroDoseMetrics.Model;

namespace ZeroDoseMetrics.ViewModel
{
	public class ChildrenListingViewModel
	{
		public IList<LineList> ChildrenLineLists { get; set; }

		public ChildrenListingViewModel()
		{
			//try
			//{
			//	ChildrenLineLists = new ObservableCollection<LineList>();
				
   //             ChildrenLineLists.Add(new LineList
   //             {
   //                 Id = 1,
   //                 CaregiverName = "Nurudeen",
   //                 CaregiverContact = "08093946133",
   //                 ChildAge = "29",
   //                 ChildName = "Imran",
   //                 SettlementName = "Dankoli",
   //                 Latitude = 123.456,
   //                 Longitude = 12.3456
   //             });
   //             ChildrenLineLists.Add(new LineList
   //             {
   //                 Id = 2,
   //                 CaregiverName = "Aliyu",
   //                 CaregiverContact = "08093946133",
   //                 ChildAge = "29",
   //                 ChildName = "Imran",
   //                 SettlementName = "Tsafe",
   //                 Latitude = 123.456,
   //                 Longitude = 12.3456
   //             });

   //         }
			//catch (Exception ex)
			//{

			//}
		}
	}
}

