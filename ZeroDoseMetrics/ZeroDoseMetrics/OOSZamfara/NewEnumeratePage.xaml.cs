using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZeroDoseMetrics.Model;

namespace ZeroDoseMetrics.OOSZamfara
{	
	public partial class NewEnumeratePage : ContentPage
	{
        public Login helper { get; set; }

        public OOSList newChild { get; set; }

        public string TeamCode { get; set; }
        public string Settlement { get; set; }
        public string HealthFacility { get; set; }
        public string InterviewerName { get; set; }
        public string PhoneNumber { get; set; }

        public NewEnumeratePage (Login helper)
		{
			InitializeComponent ();
            this.helper = helper;
            newChild = new OOSList();
            this.TeamCode = helper.TeamCode;
            this.Settlement = helper.Settlement;
            this.HealthFacility = helper.HealthFacility;
            this.PhoneNumber = helper.PhoneNo;
            this.InterviewerName = helper.InterviewerName;
            CheckBoxGroup.IsVisible = false;

            string time = DateTime.Now.ToString("hh:mm tt");
            string date = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            string unique = DateTime.Now.ToString("yyyyMMddHHmmss");

            EnumeratorNameEntry.Text = InterviewerName;
            phoneNoEntry.Text = PhoneNumber;
            dateEntry.Text = date;
            timeEntry.Text = time;
            teamCodeEntry.Text = TeamCode;
            lgaEntry.Text = "TSAFE";
            wardEntry.Text = "TSAFE CENTRAL";
            LocationLabel.Text = LocationLabel.Text;
            childIDEnty.Text = "IEV/TSF/TSC/" + unique;
            settlementEntry.Text = Settlement;
            catchmentAreaHFEntry.Text = HealthFacility;
            RadioButtonGroupAEFI.IsVisible = false;
            AEFIAfterLbl.IsVisible = false;
            AEFIAfterLblComp.IsVisible = false;
        }

        void SettlementPicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {

        }

        void CatchmentAreaHF_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {

        }

        void Gender_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            
        }

        void PreviouslyReceivedRI_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            
        }

        void AEFI_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {

            var radioButton = sender as RadioButton;


            if (radioButton == AEFIYes)
            {
                RadioButtonGroupAEFI.BindingContext = "Yes";
                RadioButtonGroupAEFIType.IsVisible = true;
                SeriousAEFI.IsVisible = true;
                NonSeriousAEFI.IsVisible = true;
                AEFITypeLabel.IsVisible = true;
            }
            if (radioButton == AEFINo)
            {
                RadioButtonGroupAEFI.BindingContext = "No";
                SeriousAEFI.IsVisible = false;
                NonSeriousAEFI.IsVisible = false;
                AEFITypeLabel.IsVisible = false;
                RadioButtonGroupAEFIType.IsVisible = false;

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

        void AgePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
        }

        void Submit_Clicked(System.Object sender, System.EventArgs e)
        {

            //validation
            string coordi = LocationLabel.Text;
            string settlementTypePic = SettlementTypePicker.SelectedIndex.ToString();
            string respond = RespondentEnty.Text;
            string household = HouseholdNameEnty.Text;
            string caregiver = caregiverNameEnty.Text;
            string caregiverNumber = caregiverNumberEnty.Text;
            string childID = childIDEnty.Text;
            string childName = childNameEnty.Text;
            string age = AgePicker.SelectedIndex.ToString();
            string gender = GenderPicker.SelectedIndex.ToString();
            string antigenPicker = ChildreceivedAntigenPicker.SelectedIndex.ToString();
            string administeredAntigens = AllAdministeredAntigens();
            string aefi = GetAEFI();
            string aefiType = GetAEFIType();

            if (CheckBoxGroup.IsVisible && (administeredAntigens == "" || aefi == ""))
            {
                DisplayAlert("ERROR", "SELECT ANTIGEN AND AEFI BEFORE YOU PROCEED", "OK");
            }
            else if (!(caregiverNumberEnty.Text.Length == 11))
            {
                DisplayAlert("ERROR PHONE NO", "PHONE NUMBER MUST BE 11 DIGITS", "OK");
            }
            else if(aefi == "Yes" && aefiType == "")
            {
                DisplayAlert("ERROR", "SELECT AEFI TYPE BEFORE YOU PROCEED", "OK");
            }       
            else if (string.IsNullOrEmpty(LocationLabel.Text) || settlementTypePic == "-1" || coordi == "Fetch location..."
                || respond == null || settlementTypePic == "-1" || household == null || caregiver == null
                || childID == null || childName == null || age == "-1" || gender == "-1" || antigenPicker == "-1"
                )
            {
                DisplayAlert("ERROR", "FILL ALL FIELDS IN THE FORM BEFORE SUBMITTING", "OK");
            }
            else
            {
                string geocord = LocationLabel.Text;
                string lat = geocord.Split(',')[0];
                string lat_ = lat.Split(':')[1];
                double x = Double.Parse(lat_.Trim(), CultureInfo.InvariantCulture);

                string longi = geocord.Split(',')[1];
                string longi_ = lat.Split(':')[1];
                double y = Double.Parse(longi_.Trim(), CultureInfo.InvariantCulture);

                newChild.VaccinatorName = EnumeratorNameEntry.Text;
                newChild.VaccinatorNumber = phoneNoEntry.Text;
                newChild.Date = dateEntry.Text;
                newChild.Time = timeEntry.Text;
                newChild.LGA = lgaEntry.Text;
                newChild.Ward = wardEntry.Text;
                newChild.SettlementType = SettlementTypePicker.SelectedItem.ToString();
                newChild.Latitude = x;
                newChild.Longitude = y;
                newChild.Respondent = RespondentEnty.Text;
                newChild.HouseHoldHeadName = HouseholdNameEnty.Text;
                newChild.CaregiverName = caregiverNameEnty.Text;
                newChild.CaregiverNumber = caregiverNumberEnty.Text;
                newChild.ChildID = childIDEnty.Text;
                newChild.ChildName = childNameEnty.Text;
                newChild.CurrentAge = AgePicker.SelectedItem.ToString();
                newChild.Gender = GenderPicker.SelectedItem.ToString();
                newChild.HasReceivedAntigen = ChildreceivedAntigenPicker.SelectedItem.ToString();
                newChild.AntigensReceived = AllAdministeredAntigens();
                newChild.AEFI = GetAEFI();
                newChild.AEFIType = GetAEFIType();
                newChild.Completed = 0;
                newChild.SettlementName = settlementEntry.Text;
                newChild.CatchmentAreaHF = catchmentAreaHFEntry.Text;
                newChild.TeamCode = teamCodeEntry.Text;
                newChild.Age = age;

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {

                    conn.CreateTable<OOSList>();
                    int rows = conn.Insert(newChild);

                    if (rows > 0)
                    {
                        DisplayAlert("Success", "Child Enumeration record successfully saved", "OK");
                        Navigation.PushAsync(new ChildrenLineListPage(helper));
                    }
                    else
                    {
                        DisplayAlert("Failure", "Error saving Chilf Enumeration record", "OK");
                    }
                }
            }

            //vcalidation


            

        }

        void SettlementTypePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {

        }

        void GenderPicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
        }

        void ChildreceivedAntigenPicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var item = sender as Picker;

            if(item.SelectedItem.ToString() == "Yes")
            {
                CheckBoxGroup.IsVisible = true;
                administeredAntigens.IsVisible = true;
                RadioButtonGroupAEFI.IsVisible = true;
                AEFIAfterLbl.IsVisible = true;
                AEFIAfterLblComp.IsVisible = true;
            }
            if(item.SelectedItem.ToString() == "No")
            {
                CheckBoxGroup.IsVisible = false;
                administeredAntigens.IsVisible = false;
                RadioButtonGroupAEFI.IsVisible = false;
                AEFIAfterLbl.IsVisible = false;
                AEFIAfterLblComp.IsVisible = false;

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

        private string AllAdministeredAntigens()
        {
            bool isOPVChecked = OPV0.IsChecked;
            bool isHepBChecked = HepB0.IsChecked;
            bool isBCGChecked = BCG.IsChecked;
            bool isOPV1Checked = OPV1.IsChecked;
            bool isPENTA1Checked = PENTA1.IsChecked;
            bool isPCV1Checked = PCV1.IsChecked;
            bool isRota1Checked = ROTA1.IsChecked;
            bool isIPV1Checked = IPV1.IsChecked;
            bool isOPV2Checked = OPV2.IsChecked;
            bool isPENTA2Checked = PENTA2.IsChecked;
            bool isPCV2Checked = PCV2.IsChecked;
            bool isRota2Checked = ROTA2.IsChecked;
            bool isOPV3Checked = OPV3.IsChecked;
            bool isPENTA3Checked = PENTA3.IsChecked;
            bool isPCV3Checked = PCV3.IsChecked;
            bool isIPV2Checked = IPV2.IsChecked;
            bool isMEASLES1Checked = MEASLES1.IsChecked;
            bool isYFChecked = YF.IsChecked;
            bool isMENAChecked = MENA.IsChecked;
            bool isMEASLES2Checked = MEASLES2.IsChecked;


            string selectedOptions = string.Empty;

            if (isOPVChecked) selectedOptions += "OPV | ";
            if (isHepBChecked) selectedOptions += "HepB | ";
            if (isBCGChecked) selectedOptions += "BCG | ";
            if (isOPV1Checked) selectedOptions += "OPV1 | ";
            if (isPENTA1Checked) selectedOptions += "PENTA1 | ";
            if (isPCV1Checked) selectedOptions += "PCV1 | ";
            if (isRota1Checked) selectedOptions += "ROTA1 | ";
            if (isIPV1Checked) selectedOptions += "IPV1 | ";
            if (isOPV2Checked) selectedOptions += "OPV2 | ";
            if (isPENTA2Checked) selectedOptions += "PENTA2 | ";
            if (isPCV2Checked) selectedOptions += "PCV2 | ";
            if (isRota2Checked) selectedOptions += "ROTA2 | ";
            if (isOPV3Checked) selectedOptions += "OPV3 | ";
            if (isPENTA3Checked) selectedOptions += "PENTA3 | ";
            if (isPCV3Checked) selectedOptions += "PCV3 | ";
            if (isIPV2Checked) selectedOptions += "IPV2 | ";
            if (isMEASLES1Checked) selectedOptions += "MEASLES1 | ";
            if (isYFChecked) selectedOptions += "YF | ";
            if (isMENAChecked) selectedOptions += "MENA | ";
            if (isMEASLES2Checked) selectedOptions += "MEASLES2 | ";

            return selectedOptions;
        }
    }
}

