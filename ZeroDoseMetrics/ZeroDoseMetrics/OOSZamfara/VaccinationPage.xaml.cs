using System;
using System.Collections.Generic;
using System.Globalization;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZeroDoseMetrics.Model;

namespace ZeroDoseMetrics.OOSZamfara
{	
	public partial class VaccinationPage : ContentPage
	{
        public Login helper { get; set; }



		public VaccinationPage (OOSList list, Login helper)
		{
			InitializeComponent ();
            this.helper = helper;


            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                int id = list.Id;

                conn.CreateTable<OOSList>();
                var linelist = conn.Table<OOSList>().Where(x => x.Id == id).FirstOrDefault();
                //ChildrenLineList.ItemsSource = linelists;

                string time = DateTime.Now.ToString("hh:mm tt");
                string date = DateTime.Now.ToString("dddd, dd MMMM yyyy");

                vaccinatorNameEntry.Text = helper.InterviewerName;
                phoneNoEntry.Text = helper.PhoneNo;
                dateEntry.Text = date;
                timeEntry.Text = time;
                teamCodeEntry.Text = helper.TeamCode;
                lgaEntry.Text = linelist.LGA;
                catchmentAreaHFEntry.Text = linelist.CatchmentAreaHF;
                settlementNameEnty.Text = linelist.SettlementName;
                wardEntry.Text = linelist.Ward;
                childNameEnty.Text = linelist.ChildName;
                childAgeEnty.Text = linelist.Age.ToString();
                GenderEnty.Text = linelist.Gender;
                childIDEnty.Text = linelist.ChildID.ToString();
                AntigensReceivedEnty.Text = linelist.AntigensReceived;
                if (linelist.AntigensReceived == "")
                {
                    
                    HasReceivedAntigenLbl.IsVisible = true;
                    RadioButtonGroupReceivedRIPreviously.IsVisible = true;
                   
                }
                

            }


        }

        void save_Clicked(System.Object sender, System.EventArgs e)
        {

        }

        void PreviouslyEnumerated_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {

        }

        void DayPicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            //var picker = sender as Picker;
            //string selectedValue = string.Empty;
            //foreach (var view in RadioButtonGroupAEFIType.Children)
            //{
            //    if (view is Picker picker1 && picker1.IsSet)
            //    {
            //        // Retrieve the selected value
            //        selectedValue = radioButton.Content.ToString();

            //    }

            //}
            //return selectedValue;
        }

        void PreviouslyReceivedRI_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            //var picker = sender as Picker;
            //string selectedValue = string.Empty;
            //foreach (var view in RadioButtonGroupAEFIType.Children)
            //{
            //    if (view is Picker picker1 && picker1.IsSet)
            //    {
            //        // Retrieve the selected value
            //        selectedValue = radioButton.Content.ToString();

            //    }

            //}
            //return selectedValue;
        }

        void AgePickerCurrent_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {

        }



        void Button_Submit(System.Object sender, System.EventArgs e)
        {
            //Begin validation

            //string previously = GetPreviouslyEnumerated();
            string selectedAge = AgePickerCurrent.SelectedIndex.ToString();
            string receivedRI = GetPreviouslyReceivedRI();
            string administeredA = AllAdministeredAntigens();
            string aefi = GetAEFI();
            string aefiType = GetAEFIType();
            string dueForNextAntigens = DueForNextAntigen.SelectedIndex.ToString();

            if (followupQ.IsVisible && (administeredA == "" || aefi == ""))
            {
    
             DisplayAlert("ERROR", "ANSWER FOLLOW-UP QUESTIONS.", "OK");
               
            }
            else if (aefi == "Yes" && aefiType == "")
            {
                DisplayAlert("ERROR", "SELECT AEFI TYPE", "OK");
            }
            else if (selectedAge == "-1")
            {
                DisplayAlert("ERROR", "SELECT CHILD CURRENT AGE", "OK");
            }
            else if (dueForNextAntigens == "-1")
            {
                DisplayAlert("ERROR", "FILL IS CHILD DUE FOR NEXT ANTIGEN?", "OK");
            }
            else if (string.IsNullOrEmpty(childNameEnty.Text) || LocationLabel.Text == "Fetch location...")
            {
                DisplayAlert("ERROR", "ENSURE GEO-CORDINATE AND CHILD NAME ARE FILLED", "OK");
            }
            
            else
            {
                string childId = childIDEnty.Text;

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<OOSList>();
                    OOSList record = conn.Table<OOSList>().Where(x => x.ChildID.Equals(childId)).FirstOrDefault();

                    string geocord = LocationLabel.Text;
                    string lat = geocord.Split(',')[0];
                    string lat_ = lat.Split(':')[1];
                    double xx = Double.Parse(lat_.Trim(), CultureInfo.InvariantCulture);

                    string longi = geocord.Split(',')[1];
                    string longi_ = longi.Split(':')[1];
                    double yy = Double.Parse(longi_.Trim(), CultureInfo.InvariantCulture);

                    record.VaccinatorName = vaccinatorNameEntry.Text;
                    record.VaccinatorNumber = phoneNoEntry.Text;
                    record.Date = dateEntry.Text;
                    record.Time = timeEntry.Text;
                    record.TeamCode = teamCodeEntry.Text;
                    record.LGA = lgaEntry.Text;
                    record.CatchmentAreaHF = catchmentAreaHFEntry.Text;
                    record.SettlementName = settlementNameEnty.Text;
                    record.Ward = wardEntry.Text;
                    record.ChildName = childNameEnty.Text;
                    record.Age = childAgeEnty.Text;
                    record.Gender = GenderEnty.Text;
                    record.ChildID = childIDEnty.Text;
                    record.ChildEnumeratedByAfenet = "";
                    record.Latitude = xx; //latitude
                    record.Longitude = yy;//longitude
                    record.CurrentAge = AgePickerCurrent.SelectedItem.ToString();
                    record.HasReceivedAntigen = GetPreviouslyReceivedRI();
                    record.AntigensReceived = AllAdministeredAntigens();
                    record.AEFI = GetAEFI();
                    record.AEFIType = GetAEFIType();
                    record.Completed = 1;
                    helper.Settlement = settlementNameEnty.Text;
                    helper.HealthFacility = catchmentAreaHFEntry.Text;
                    record.DueForNextAntigen = DueForNextAntigen.SelectedItem.ToString(); ; // used to hold due for next antigen


                    int rows = conn.Update(record);

                    if (rows > 0)
                    {
                        DisplayAlert("Success", "Vaccination Record Saved", "OK");
                        Navigation.PushAsync(new ChildrenLineListPage(helper));
                    }
                    else
                    {
                        DisplayAlert("Failure", "Error Saving Vaccination Record", "OK");
                        Navigation.PushAsync(new ChildrenLineListPage(helper));
                    }

                }

                //End Validation


            }
        }


        private string GetAEFI()
        {
            // Loop through each RadioButton in the RadioButtonGroup
            string selectedValue = string.Empty;

            foreach (var view in RadioButtonGroupAEFI.Children)
            {
                if (view is RadioButton radioButton && radioButton.IsChecked)
                {
                    // Retrieve the selected value
                    selectedValue = radioButton.Content.ToString();

                }

            }
            return selectedValue;
        }

        private string GetAEFIType()
        {
            // Loop through each RadioButton in the RadioButtonGroup
            string selectedValue = string.Empty;

            foreach (var view in RadioButtonGroupAEFIType.Children)
            {
                if (view is RadioButton radioButton && radioButton.IsChecked)
                {
                    // Retrieve the selected value
                    selectedValue = radioButton.Content.ToString();

                }

            }
            return selectedValue;
        }

        private string GetPreviouslyReceivedRI()
        {
            // Loop through each RadioButton in the RadioButtonGroup
            string selectedValue = string.Empty;

            foreach (var view in RadioButtonGroupReceivedRIPreviously.Children)
            {
                if (view is RadioButton radioButton && radioButton.IsChecked)
                {
                    // Retrieve the selected value
                    selectedValue = radioButton.Content.ToString();

                }

            }
            return selectedValue;
        }

        private string AllAdministeredAntigens()
        {
            bool isHepBChecked = HepB0.IsChecked;
            bool isBCGChecked = BCG.IsChecked;
            bool isYFChecked = YF.IsChecked;
            bool isMENAChecked = MENA.IsChecked;

            string selectedOptions = string.Empty;


            if (isHepBChecked) selectedOptions += "HepB | ";
            if (isBCGChecked) selectedOptions += "BCG | ";
            if (isYFChecked) selectedOptions += "YF | ";
            if (isMENAChecked) selectedOptions += "MENA | ";
            if (PENTATypes.SelectedIndex != -1) { selectedOptions += PENTATypes.SelectedItem.ToString()+ " |"; };
            if (MeaslesTypes.SelectedIndex != -1) { selectedOptions += MeaslesTypes.SelectedItem.ToString() + " |"; };
            if (PCVTypes.SelectedIndex != -1) { selectedOptions += PCVTypes.SelectedItem.ToString() + " |"; };
            if (ROTATypes.SelectedIndex != -1) { selectedOptions += ROTATypes.SelectedItem.ToString() + " |"; };
            if (IPVTypes.SelectedIndex != -1) { selectedOptions += IPVTypes.SelectedItem.ToString() + " |"; };
            if (OPVTypes.SelectedIndex != -1) { selectedOptions += OPVTypes.SelectedItem.ToString() + " |"; };


            return selectedOptions;
        }


        private async void OnGetLocationClicked(object sender, EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    // No cached location, get the real-time location
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }

                if (location != null)
                {
                    LocationLabel.Text = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}";
                }
                else
                {
                    LocationLabel.Text = "Unable to retrieve location.";
                }
            }
            catch (FeatureNotSupportedException Ex)
            {
                // Handle not supported on device exception
                LocationLabel.Text = "Location not supported on this device.";
            }
            catch (PermissionException Ex)
            {
                // Handle permission exception
                LocationLabel.Text = "Location permission denied.";
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                LocationLabel.Text = "An error occurred: " + ex.Message;
            }
        }

       

        void AEFI_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {

            var radioButton = sender as RadioButton;


            if (radioButton == AEFIYes)
            {
                RadioButtonGroupAEFI.BindingContext = "Yes";
                SeriousAEFI.IsEnabled = true;
                NonSeriousAEFI.IsEnabled = true;
                AEFITypeLabel.IsEnabled = true;
                compulsory.IsEnabled = true;
            }
            if (radioButton == AEFINo)
            {
                RadioButtonGroupAEFI.BindingContext = "No";
                SeriousAEFI.IsEnabled = false;
                NonSeriousAEFI.IsEnabled = false;
                AEFITypeLabel.IsEnabled = false;
                compulsory.IsEnabled = false;

            }
        }


        void AEFIType_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {

            var radioButton = sender as RadioButton;


            if (radioButton == SeriousAEFI)
            {
                // For Yes Option
                RadioButtonGroupAEFIType.BindingContext = "Serious";
            }
            if (radioButton == NonSeriousAEFI)
            {
                // For Yes Option
                RadioButtonGroupAEFIType.BindingContext = "Non-Serious";
            }

        }

        void PENTA_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked){
                PENTATypes.SelectedIndex = -1;
                PENTATypes.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                PENTATypes.SelectedIndex = -1;
                PENTATypes.IsVisible = false;
            }
           
        }

        void Measles_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                MeaslesTypes.SelectedIndex = -1;
                MeaslesTypes.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                MeaslesTypes.SelectedIndex = -1;
                MeaslesTypes.IsVisible = false;
            }
        }

        void PCV_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                PCVTypes.SelectedIndex = -1;
                PCVTypes.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                PCVTypes.SelectedIndex = -1;
                PCVTypes.IsVisible = false;
            }
        }

        void IPV_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                IPVTypes.SelectedIndex = -1;
                IPVTypes.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                IPVTypes.SelectedIndex = -1;
                IPVTypes.IsVisible = false;
            }
        }

        void OPV_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                OPVTypes.SelectedIndex = -1;
                OPVTypes.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                OPVTypes.SelectedIndex = -1;
                OPVTypes.IsVisible = false;
            }
        }

        void ROTA_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                ROTATypes.SelectedIndex = -1;
                ROTATypes.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                ROTATypes.SelectedIndex = -1;
                ROTATypes.IsVisible = false;
            }
        }

        void IPV_CheckedChanged_1(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                IPVTypes.SelectedIndex = -1;
                IPVTypes.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                IPVTypes.SelectedIndex = -1;
                IPVTypes.IsVisible = false;
            }
        }

        void Measles_CheckedChanged_1(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                MeaslesTypes.SelectedIndex = -1;
                MeaslesTypes.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                MeaslesTypes.SelectedIndex = -1;
                MeaslesTypes.IsVisible = false;
            }
        }

        void DueForNextAntigen_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var item = sender as Picker;

            if(item.SelectedItem.ToString() == "Yes")
            {
                followupQ.IsVisible = true;
            }
            if (item.SelectedItem.ToString() == "No")
            {
                followupQ.IsVisible = false;
            }
        }
    }
}

