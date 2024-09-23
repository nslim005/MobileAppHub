using System;
using System.Collections.Generic;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZeroDoseMetrics.Model;

namespace ZeroDoseMetrics
{	
	public partial class AssessmentPage : ContentPage
	{
        UnderFive UnderFive = new UnderFive(); 

		public AssessmentPage (LineList linelist)
		{
			InitializeComponent();
            QuestionLabel.IsVisible = false;
            others.IsVisible = false;
            others_checkbox.IsVisible = false;
            misplacedCoupon.IsVisible = false;
            unawareOfVaccinationNeed.IsVisible = false;
            didNotKnowChildNeedVaccine.IsVisible = false;
            vaccinationNotImportant.IsVisible = false;
            doNotTrustVaccine.IsVisible = false;
            fearOfSideEffect.IsVisible = false;
            motherTooBusy.IsVisible = false;
            forgotToTakeChild.IsVisible = false;
            headOfHousWontAllow.IsVisible = false;
            unableToPayTransport.IsVisible = false;
            familiyProblemillness.IsVisible = false;
            
            //Checklist

            misplacedCouponLbl.IsVisible = false;
            unawareOfVaccinationNeedLbl.IsVisible = false;
            didNotKnowChildNeedVaccineLbl.IsVisible = false;
            vaccinationNotImportantLbl.IsVisible = false;
            doNotTrustVaccineLbl.IsVisible = false;
            fearOfSideEffectLbl.IsVisible = false;
            motherTooBusyLbl.IsVisible = false;
            forgotToTakeChildLbl.IsVisible = false;
            familiyProblemillness.IsVisible = false;
            unableToPayTransportLbl.IsVisible = false;
            familiyProblemillnessLbl.IsVisible = false;
            others_specifyChkboxEntry.IsVisible = false;
            others_checkboxLbl.IsVisible = false;
            headOfHousWontAllowLbl.IsVisible = false;


            

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                int id = linelist.Id;

                conn.CreateTable<LineList>();
                var linelists = conn.Table<LineList>().Where(x => x.Id == id).FirstOrDefault();
                //ChildrenLineList.ItemsSource = linelists;


                conn.CreateTable<Login>();
                var Login = conn.Table<Login>().Where(x => x.TeamCode == linelist.HaveVaccinationCard).FirstOrDefault();

                var user = Login.InterviewerName;
                var phoneNumber = Login.PhoneNo;

                childID.Text = linelist.Id.ToString();
                childName.Text = linelist.ChildName;
                childAgeEntry.Text = linelist.ChildAge;
                caregiverNameEntry.Text = linelist.CaregiverName;
                settlementEntry.Text = linelist.SettlementName;
                caregiverContactEntry.Text = linelist.CaregiverContact;
                teamCodeEntry.Text = linelist.TeamCode;
                addressEntry.Text = linelist.Address;
                interviewerNameEntry.Text = user;
                phoneNoEntry.Text = phoneNumber;

            }


        }

        void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            int id = Convert.ToInt32(childID.Text);

            
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<LineList>();
                LineList record = conn.Table<LineList>().Where(x => x.Id == id).FirstOrDefault();
                record.ChildName = childName.Text;
                record.ChildAge = childAgeEntry.Text;
                record.CaregiverName = caregiverNameEntry.Text;
                record.CaregiverContact = caregiverContactEntry.Text;
                record.SettlementName = settlementEntry.Text;
                //record.Latitude = Convert.ToDouble(latitudeEntry.Text);
                //record.Longitude = Convert.ToDouble(longitudeEntry.Text);
                record.TeamCode = teamCodeEntry.Text;
                record.InterviewerName = interviewerNameEntry.Text;
                record.PhoneNo = phoneNoEntry.Text;
                record.Address = addressEntry.Text;
                record.Response = RespondentValue();
                record.ReceivedAntigen = ReceivedAntigenValue();
                record.HaveVaccinationCard = HaveVaccinationCardValue();
                record.AdministeredAntigen = AllAdministeredAntigens();
                record.MotivationForVaccination = AllMotivation();
                record.ReasonForNotVaccination = ReasonsForNotVaccination();
                record.Completed = 1;

                conn.CreateTable<LineList>();
                int rows = conn.Update(record);

                if (rows > 0)
                {
                    DisplayAlert("Success", "Assessment Completed", "OK");
                    Navigation.PushAsync(new ChildrenPage(record.TeamCode));
                }
                else
                {
                    DisplayAlert("Failure", "Error Completing Assessment", "OK");
                    Navigation.PushAsync(new ChildrenPage(record.TeamCode));
                }
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


        private string ReasonsForNotVaccination()
        {
            bool ismisplacedCouponChecked = misplacedCoupon.IsChecked;
            bool isunawareOfVaccinationNeedChecked = unawareOfVaccinationNeed.IsChecked;
            bool isdidNotKnowChildNeedVaccineChecked = didNotKnowChildNeedVaccine.IsChecked;
            bool isvaccinationNotImportantChecked = vaccinationNotImportant.IsChecked;
            bool isdoNotTrustVaccineChecked = doNotTrustVaccine.IsChecked;
            bool isfearOfSideEffectChecked = fearOfSideEffect.IsChecked;
            bool ismotherTooBusyChecked = motherTooBusy.IsChecked;
            bool isforgotToTakeChildChecked = forgotToTakeChild.IsChecked;
            bool isheadOfHousWontAllowChecked = headOfHousWontAllow.IsChecked;
            bool isunableToPayTransportChecked = unableToPayTransport.IsChecked;
            bool isfamiliyProblemillnessChecked = misplacedCoupon.IsChecked;
            bool isothers_checkboxChecked = others_checkbox.IsChecked;


            string selectedOptions = string.Empty;

            if (ismisplacedCouponChecked) selectedOptions += "Misplaced coupon | ";
            if (isunawareOfVaccinationNeedChecked) selectedOptions += "Unaware of the need for vaccination | ";
            if (isdidNotKnowChildNeedVaccineChecked) selectedOptions += "Did not know the child needed other vaccines | ";
            if (isvaccinationNotImportantChecked) selectedOptions += "Feel vaccination is not important | ";
            if (isdoNotTrustVaccineChecked) selectedOptions += "Do not trust vaccines | ";
            if (isfearOfSideEffectChecked) selectedOptions += "Fear of side effects/adverse events | ";
            if (ismotherTooBusyChecked) selectedOptions += "Mother too busy | ";
            if (isforgotToTakeChildChecked) selectedOptions += "Forgot to take child for vaccination | ";
            if (isheadOfHousWontAllowChecked) selectedOptions += "Husband/head of household won't allow | ";
            if (isunableToPayTransportChecked) selectedOptions += "Unable to pay for transport | ";
            if (isfamiliyProblemillnessChecked) selectedOptions += "Family problems, like the illness of the mother | ";
            if (isothers_checkboxChecked) selectedOptions += "Others | ";

            return selectedOptions;
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

        private string AllMotivation()
        {
            bool isavailabilityOfReferralCOuponChecked = availabilityOfReferralCOupon.IsChecked;
            bool ispreventChildFromDiseaseChecked = preventChildFromDisease.IsChecked;
            bool isadviceChecked = advice.IsChecked;
            bool isdiseaseOutbreakChecked = diseaseOutbreak.IsChecked;
            bool isrecommendationChecked = recommendation.IsChecked;
            bool isensurewellbeingChecked = ensurewellbeing.IsChecked;
            bool ispreventdiseaseSpreadChecked = preventdiseaseSpread.IsChecked;
            bool isothers_checkboxmotivatedChecked = others_checkboxmotivated.IsChecked;

            string selectedOptions = string.Empty;

            if (isavailabilityOfReferralCOuponChecked) selectedOptions += "Availability of referral coupon | ";
            if (ispreventChildFromDiseaseChecked) selectedOptions += "To protect my child from preventable diseases | ";
            if (isadviceChecked) selectedOptions += "Advice from a healthcare worker/community mobilizer | ";
            if (isdiseaseOutbreakChecked) selectedOptions += "Due to a recent outbreak of a disease in the area | ";
            if (isrecommendationChecked) selectedOptions += "Recommendations from family/friends/religious leaders e.t.c | ";
            if (isensurewellbeingChecked) selectedOptions += "To ensure the overall health and well-being of my child | ";
            if (ispreventdiseaseSpreadChecked) selectedOptions += "To prevent the spread of diseases in the community | ";
            if (isothers_checkboxmotivatedChecked) selectedOptions += "Others | ";
            return selectedOptions;
        }


        private string RespondentValue()
        {
            // Loop through each RadioButton in the RadioButtonGroup
            string selectedValue = string.Empty;
            foreach (var view in RadioButtonGroupRespondent.Children)
            {
                if (view is RadioButton radioButton && radioButton.IsChecked)
                {
                    // Retrieve the selected value
                    selectedValue  = radioButton.Value.ToString();
                    
                }
                
            }
            return selectedValue;
        }

        private string ReceivedAntigenValue()
        {
            // Loop through each RadioButton in the RadioButtonGroup
            string selectedValue = string.Empty;
            foreach (var view in RadioButtonGroupReceivedAntigen.Children)
            {
                if (view is RadioButton radioButton && radioButton.IsChecked)
                {
                    // Retrieve the selected value
                    selectedValue = radioButton.Value.ToString();

                }

            }
            return selectedValue;
        }

        private string HaveVaccinationCardValue()
        {
            // Loop through each RadioButton in the RadioButtonGroup
            string selectedValue = string.Empty;
            foreach (var view in RadioButtonGroupHaveVaccinationCard.Children)
            {
                if (view is RadioButton radioButton && radioButton.IsChecked)
                {
                    // Retrieve the selected value
                    selectedValue = radioButton.Value.ToString();

                }

            }
            return selectedValue;
        }


        private void OnRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            //if (e.Value == true) // Check if the RadioButton is checked
            //{
                var radioButton = sender as RadioButton;

                
                if (radioButton == Option2RadioButton)
                {
                // For Yes Option
                    ToggleTrueForOptionTwo();
                    ToggleFalseForOptioOne();
                }

                else //(radioButton == Option1RadioButton)
                {
                // For Yes Option
                    ToggleTrueForOptioOne();
                    ToggleFalseForOptionTwo();
                }
        }




        private void OnRadioButtonCheckedChangedRespondent(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value == true) // Check if the RadioButton is checked
            {
                var radioButton = sender as RadioButton;

                    if (radioButton == Option4Others)
                    {
                        others.IsVisible = true;
                    }
                    else
                    {
                        others.IsVisible = false;
                    }
            }
        }

        void others_checkbox_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            //if(e.Value == true)
            //{
                var checkedItem = sender as CheckBox;

                if(checkedItem.IsChecked == true)
                {
                    others_specifyChkboxEntry.IsVisible = true;
                }
                else
                {
                    others_specifyChkboxEntry.IsVisible = false;
                }
            //}
           
        }

        void others_checkboxmotivated_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var checkedItem = sender as CheckBox;

            if (checkedItem.IsChecked == true)
            {
                others_checkboxmotivatedEntry.IsVisible = true;
            }
            else
            {
                others_checkboxmotivatedEntry.IsVisible = false;
            }
        }

        public void ToggleTrueForOptioOne()
        {
            childHaveVaccinationCard.IsVisible = true;
            childHaveVaccinationCardHint.IsVisible = true;
            YesCardSeen.IsVisible = true;
            YesCardVerbal.IsVisible = true;
            CaregiverCanRecall.IsVisible = true;
            CaregiverCannotRecall.IsVisible = true;
            No.IsVisible = true;

            OPV0.IsVisible = true;
            OPVLbl.IsVisible = true;
            HepB0.IsVisible = true;
            hepB0Lbl.IsVisible = true;
            BCG.IsVisible = true;
            BCGLbl.IsVisible = true;
            OPV1Lbl.IsVisible = true;
            OPV1.IsVisible = true;
            PENTA1Lbl.IsVisible = true;
            PENTA1.IsVisible = true;
            PCV1Lbl.IsVisible = true;
            PCV1.IsVisible = true;
            ROTA1Lbl.IsVisible = true;
            ROTA1.IsVisible = true;
            IPV1Lbl.IsVisible = true;
            IPV1.IsVisible = true;
            OPV2Lbl.IsVisible = true;
            OPV2.IsVisible = true;
            PENTA2Lbl.IsVisible = true;
            PENTA2.IsVisible = true;
            PCV2Lbl.IsVisible = true;
            PCV2.IsVisible = true;
            ROTA2Lbl.IsVisible = true;
            ROTA2.IsVisible = true;
            OPV3Lbl.IsVisible = true;
            OPV3.IsVisible = true;
            PENTA3Lbl.IsVisible = true;
            PENTA3.IsVisible = true;
            PCV3Lbl.IsVisible = true;
            PCV3.IsVisible = true;
            IPV2Lbl.IsVisible = true;
            IPV2.IsVisible = true;
            MEASLES1Lbl.IsVisible = true;
            MEASLES1.IsVisible = true;
            YFLbl.IsVisible = true;
            YF.IsVisible = true;
            MENALbl.IsVisible = true;
            MENA.IsVisible = true;
            MEASLES2Lbl.IsVisible = true;
            MEASLES2.IsVisible = true;

            administeredAntigens.IsVisible = true;
            motivation.IsVisible = true;

            availabilityOfReferralCOuponLbl.IsVisible = true;
            availabilityOfReferralCOupon.IsVisible = true;

            preventChildFromDiseaseLbl.IsVisible = true;
            preventChildFromDisease.IsVisible = true;

            adviceLbl.IsVisible = true;
            advice.IsVisible = true;

            diseaseOutbreakLbl.IsVisible = true;
            diseaseOutbreak.IsVisible = true;

            recommendationLbl.IsVisible = true;
            recommendation.IsVisible = true;

            ensurewellbeingLbl.IsVisible = true;
            ensurewellbeing.IsVisible = true;

            preventdiseaseSpreadLbl.IsVisible = true;
            preventdiseaseSpread.IsVisible = true;

            others_checkboxmotivatedLbl.IsVisible = true;
            others_checkboxmotivated.IsVisible = true;

            others_checkboxmotivatedLbl.IsVisible = true;
        }

        public void ToggleFalseForOptioOne()
        {
            childHaveVaccinationCard.IsVisible = false;
            childHaveVaccinationCardHint.IsVisible = false;
            YesCardSeen.IsVisible = false;
            YesCardVerbal.IsVisible = false;
            CaregiverCanRecall.IsVisible = false;
            CaregiverCannotRecall.IsVisible = false;
            No.IsVisible = false;

            OPV0.IsVisible = false;
            OPVLbl.IsVisible = false;
            HepB0.IsVisible = false;
            hepB0Lbl.IsVisible = false;
            BCGLbl.IsVisible = false;
            BCGLbl.IsVisible = false;
            OPV1Lbl.IsVisible = false;
            OPV1.IsVisible = false;
            PENTA1Lbl.IsVisible = false;
            PENTA1.IsVisible = false;
            PCV1Lbl.IsVisible = false;
            PCV1.IsVisible = false;
            ROTA1Lbl.IsVisible = false;
            ROTA1.IsVisible = false;
            IPV1Lbl.IsVisible = false;
            IPV1.IsVisible = false;
            OPV2Lbl.IsVisible = false;
            OPV2.IsVisible = false;
            PENTA2Lbl.IsVisible = false;
            PENTA2.IsVisible = false;
            PCV2Lbl.IsVisible = false;
            PCV2.IsVisible = false;
            ROTA2Lbl.IsVisible = false;
            ROTA2.IsVisible = false;
            OPV3Lbl.IsVisible = false;
            OPV3.IsVisible = false;
            PENTA3Lbl.IsVisible = false;
            PENTA3.IsVisible = false;
            PCV3Lbl.IsVisible = false;
            PCV3.IsVisible = false;
            IPV2Lbl.IsVisible = false;
            IPV2.IsVisible = false;
            MEASLES1Lbl.IsVisible = false;
            MEASLES1.IsVisible = false;
            YFLbl.IsVisible = false;
            YF.IsVisible = false;
            MENALbl.IsVisible = false;
            MENA.IsVisible = false;
            MEASLES2Lbl.IsVisible = false;
            MEASLES2.IsVisible = false;

            administeredAntigens.IsVisible = false;
            motivation.IsVisible = false;

            availabilityOfReferralCOuponLbl.IsVisible = false;
            availabilityOfReferralCOupon.IsVisible = false;

            preventChildFromDiseaseLbl.IsVisible = false;
            preventChildFromDisease.IsVisible = false;

            adviceLbl.IsVisible = false;
            advice.IsVisible = false;

            diseaseOutbreakLbl.IsVisible = false;
            diseaseOutbreak.IsVisible = false;

            recommendationLbl.IsVisible = false;
            recommendation.IsVisible = false;

            ensurewellbeingLbl.IsVisible = false;
            ensurewellbeing.IsVisible = false;

            preventdiseaseSpreadLbl.IsVisible = false;
            preventdiseaseSpread.IsVisible = false;

            others_checkboxmotivatedLbl.IsVisible = false;
            others_checkboxmotivated.IsVisible = false;

            others_checkboxmotivatedLbl.IsVisible = false;
        }


        public void ToggleTrueForOptionTwo()
        {
            QuestionLabel.IsVisible = true;
            misplacedCoupon.IsVisible = true;
            unawareOfVaccinationNeed.IsVisible = true;
            didNotKnowChildNeedVaccine.IsVisible = true;
            vaccinationNotImportant.IsVisible = true;
            doNotTrustVaccine.IsVisible = true;
            fearOfSideEffect.IsVisible = true;
            motherTooBusy.IsVisible = true;
            forgotToTakeChild.IsVisible = true;
            headOfHousWontAllow.IsVisible = true;
            unableToPayTransport.IsVisible = true;
            familiyProblemillness.IsVisible = true;


            // checkbox
            others_checkbox.IsVisible = true;
            misplacedCouponLbl.IsVisible = true;
            unawareOfVaccinationNeedLbl.IsVisible = true;
            didNotKnowChildNeedVaccineLbl.IsVisible = true;
            vaccinationNotImportantLbl.IsVisible = true;
            doNotTrustVaccineLbl.IsVisible = true;
            fearOfSideEffectLbl.IsVisible = true;
            motherTooBusyLbl.IsVisible = true;
            forgotToTakeChildLbl.IsVisible = true;
            familiyProblemillness.IsVisible = true;
            unableToPayTransportLbl.IsVisible = true;
            familiyProblemillnessLbl.IsVisible = true;
            others_checkboxLbl.IsVisible = true;
            headOfHousWontAllowLbl.IsVisible = true;
        }


        public void ToggleFalseForOptionTwo()
        {
            QuestionLabel.IsVisible = false;
            misplacedCoupon.IsVisible = false;
            unawareOfVaccinationNeed.IsVisible = false;
            didNotKnowChildNeedVaccine.IsVisible = false;
            vaccinationNotImportant.IsVisible = false;
            doNotTrustVaccine.IsVisible = false;
            fearOfSideEffect.IsVisible = false;
            motherTooBusy.IsVisible = false;
            forgotToTakeChild.IsVisible = false;
            headOfHousWontAllow.IsVisible = false;
            unableToPayTransport.IsVisible = false;
            familiyProblemillness.IsVisible = false;


            // checkbox
            others_checkbox.IsVisible = false;
            misplacedCouponLbl.IsVisible = false;
            unawareOfVaccinationNeedLbl.IsVisible = false;
            didNotKnowChildNeedVaccineLbl.IsVisible = false;
            vaccinationNotImportantLbl.IsVisible = false;
            doNotTrustVaccineLbl.IsVisible = false;
            fearOfSideEffectLbl.IsVisible = false;
            motherTooBusyLbl.IsVisible = false;
            forgotToTakeChildLbl.IsVisible = false;
            familiyProblemillness.IsVisible = false;
            unableToPayTransportLbl.IsVisible = false;
            familiyProblemillnessLbl.IsVisible = false;
            others_checkboxLbl.IsVisible = false;
            headOfHousWontAllowLbl.IsVisible = false;
        }

     

        void UnderFive_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var radioButton = sender as RadioButton;


            if (radioButton == NoUnderFive)
            {
                summary.IsVisible = true;
                ToggleHideFollowupForUnderFive();
                ToggleFalseForOptionOneU5();
                submitZDBtn.IsVisible = false;

            }
            else 
            {
                summary.IsVisible = false;
                ToggleShowFollowupForUnderFive();
                submitZDBtn.IsVisible = true;


            }
        }

        public void ToggleShowFollowupForUnderFive()
        {
            instruction.IsVisible = true;
            childNameLbl.IsVisible = true;
            childNameU5.IsVisible = true;
            AgePickerUnderFive.IsVisible = true;
            genderPickerUnderFive.IsVisible = true;
            caregiverNameEntryU5.IsVisible = true;
            caregiverContactEntryU5.IsVisible = true;
            CommMethodPicker.IsVisible = true;
            CommLangPicker.IsVisible = true;
            ChildReceivedAntigenU5.IsVisible = true;
            Option1RadioButtonU5.IsVisible = true;
            caregiverNameLblU5.IsVisible = true;
            caregiverNameEntryU5.IsVisible = true;
            caregiverNumberLblU5.IsVisible = true;
            Option2RadioButtonU5.IsVisible = true;

        }

        public void ToggleHideFollowupForUnderFive()
        {
            instruction.IsVisible = false;
            childNameLbl.IsVisible = false;
            childNameU5.IsVisible = false;
            AgePickerUnderFive.IsVisible = false;
            genderPickerUnderFive.IsVisible = false;
            caregiverNameEntryU5.IsVisible = false;
            caregiverContactEntryU5.IsVisible = false;
            CommMethodPicker.IsVisible = false;
            CommLangPicker.IsVisible = false;
            ChildReceivedAntigenU5.IsVisible = false;
            Option1RadioButtonU5.IsVisible = false;
            caregiverNameLblU5.IsVisible = false;
            caregiverNameEntryU5.IsVisible = false;
            caregiverNumberLblU5.IsVisible = false;
            Option2RadioButtonU5.IsVisible = false;



        }

        void AgePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
        }

        void genderPicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
        }

        void CommLangPicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
        }

        void CommMethodPicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
        }

        void Option1RadioButtonU5_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var radioButton = sender as RadioButton;


            if (radioButton == Option1RadioButtonU5)
            {
                // For Yes Option
                ToggleTrueForOptionOneU5();
                ToggleFalseForOptionTwoU5();
            }
            else 
            {
                // For Yes Option
                ToggleTrueForOptionTwoU5();
                ToggleFalseForOptionOneU5();
            }
        }

        public void ToggleTrueForOptionTwoU5()
        {
            unawareOfVaccinationNeedU5.IsVisible = true;
            unawareOfVaccinationNeedLblU5.IsVisible = true;
            didNotKnowChildNeedVaccineU5.IsVisible = true;
            didNotKnowChildNeedVaccineLblU5.IsVisible = true;
            vaccinationNotImportantLblU5.IsVisible = true;
            vaccinationNotImportantU5.IsVisible = true;
            doNotTrustVaccineLblU5.IsVisible = true;
            doNotTrustVaccineU5.IsVisible = true;
            fearOfSideEffectLblU5.IsVisible = true;
            fearOfSideEffectU5.IsVisible = true;
            motherTooBusyLblU5.IsVisible = true;
            motherTooBusyU5.IsVisible = true;
            forgotToTakeChildLblU5.IsVisible = true;
            forgotToTakeChildU5.IsVisible = true;
            headOfHousWontAllowLblU5.IsVisible = true;
            headOfHousWontAllowU5.IsVisible = true;
            unableToPayTransportLblU5.IsVisible = true;
            unableToPayTransportU5.IsVisible = true;
            familiyProblemillnessLblU5.IsVisible = true;
            familiyProblemillnessU5.IsVisible = true;
            others_checkboxU5.IsVisible = true;
            others_checkboxLblU5.IsVisible = true;
            QuestionLabelU5.IsVisible = true;

        }

        public void ToggleFalseForOptionTwoU5()
        {
            unawareOfVaccinationNeedU5.IsVisible = false;
            unawareOfVaccinationNeedLblU5.IsVisible = false;
            didNotKnowChildNeedVaccineU5.IsVisible = false;
            didNotKnowChildNeedVaccineLblU5.IsVisible = false;
            vaccinationNotImportantLblU5.IsVisible = false;
            vaccinationNotImportantU5.IsVisible = false;
            doNotTrustVaccineLblU5.IsVisible = false;
            doNotTrustVaccineU5.IsVisible = false;
            fearOfSideEffectLblU5.IsVisible = false;
            fearOfSideEffectU5.IsVisible = false;
            motherTooBusyLblU5.IsVisible = false;
            motherTooBusyU5.IsVisible = false;
            forgotToTakeChildLblU5.IsVisible = false;
            forgotToTakeChildU5.IsVisible = false;
            headOfHousWontAllowLblU5.IsVisible = false;
            headOfHousWontAllowU5.IsVisible = false;
            unableToPayTransportLblU5.IsVisible = false;
            unableToPayTransportU5.IsVisible = false;
            familiyProblemillnessLblU5.IsVisible = false;
            familiyProblemillnessU5.IsVisible = false;
            others_checkboxU5.IsVisible = false;
            others_checkboxLblU5.IsVisible = false;
            QuestionLabelU5.IsVisible = false;

        }

        public void ToggleTrueForOptionOneU5()
        {
            childHaveVaccinationCardU5.IsVisible = true;
            childHaveVaccinationCardHintU5.IsVisible = true;
            YesCardSeenU5.IsVisible = true;
            YesCardVerbalU5.IsVisible = true;
            CaregiverCanRecallU5.IsVisible = true;
            CaregiverCannotRecallU5.IsVisible = true;
            NoU5.IsVisible = true;
            administeredAntigensU5.IsVisible = true;

            OPV0U5.IsVisible = true;
            OPVLblU5.IsVisible = true;
            HepB0U5.IsVisible = true;
            hepB0LblU5.IsVisible = true;
            BCGU5.IsVisible = true;
            BCGLblU5.IsVisible = true;
            OPV1LblU5.IsVisible = true;
            OPV1U5.IsVisible = true;
            PENTA1LblU5.IsVisible = true;
            PENTA1U5.IsVisible = true;
            PCV1LblU5.IsVisible = true;
            PCV1U5.IsVisible = true;
            ROTA1LblU5.IsVisible = true;
            ROTA1U5.IsVisible = true;
            IPV1LblU5.IsVisible = true;
            IPV1U5.IsVisible = true;
            OPV2LblU5.IsVisible = true;
            OPV2U5.IsVisible = true;
            PENTA2LblU5.IsVisible = true;
            PENTA2U5.IsVisible = true;
            PCV2LblU5.IsVisible = true;
            PCV2U5.IsVisible = true;
            ROTA2LblU5.IsVisible = true;
            ROTA2U5.IsVisible = true;
            OPV3LblU5.IsVisible = true;
            OPV3U5.IsVisible = true;
            PENTA3LblU5.IsVisible = true;
            PENTA3U5.IsVisible = true;
            PCV3LblU5.IsVisible = true;
            PCV3U5.IsVisible = true;
            IPV2LblU5.IsVisible = true;
            IPV2U5.IsVisible = true;
            MEASLES1LblU5.IsVisible = true;
            MEASLES1U5.IsVisible = true;
            YFLblU5.IsVisible = true;
            YFU5.IsVisible = true;
            MENALblU5.IsVisible = true;
            MENAU5.IsVisible = true;
            MEASLES2LblU5.IsVisible = true;
            MEASLES2U5.IsVisible = true;
            //summary.IsVisible = true;

        }

        public void ToggleFalseForOptionOneU5()
        {
            childHaveVaccinationCardU5.IsVisible = false;
            childHaveVaccinationCardHintU5.IsVisible = false;
            YesCardSeenU5.IsVisible = false;
            YesCardVerbalU5.IsVisible = false;
            CaregiverCanRecallU5.IsVisible = false;
            CaregiverCannotRecallU5.IsVisible = false;
            NoU5.IsVisible = false;
            administeredAntigensU5.IsVisible = false;

            OPV0U5.IsVisible = true;
            OPVLblU5.IsVisible = false;
            HepB0U5.IsVisible = false;
            hepB0LblU5.IsVisible = false;
            BCGU5.IsVisible = false;
            BCGLblU5.IsVisible = false;
            OPV1LblU5.IsVisible = false;
            OPV1U5.IsVisible = false;
            PENTA1LblU5.IsVisible = false;
            PENTA1U5.IsVisible = false;
            PCV1LblU5.IsVisible = false;
            PCV1U5.IsVisible = false;
            ROTA1LblU5.IsVisible = false;
            ROTA1U5.IsVisible = false;
            IPV1LblU5.IsVisible = false;
            IPV1U5.IsVisible = false;
            OPV2LblU5.IsVisible = false;
            OPV2U5.IsVisible = false;
            PENTA2LblU5.IsVisible = false;
            PENTA2U5.IsVisible = false;
            PCV2LblU5.IsVisible = false;
            PCV2U5.IsVisible = false;
            ROTA2LblU5.IsVisible = false;
            ROTA2U5.IsVisible = false;
            OPV3LblU5.IsVisible = false;
            OPV3U5.IsVisible = false;
            PENTA3LblU5.IsVisible = false;
            PENTA3U5.IsVisible = false;
            PCV3LblU5.IsVisible = false;
            PCV3U5.IsVisible = false;
            IPV2LblU5.IsVisible = false;
            IPV2U5.IsVisible = false;
            MEASLES1LblU5.IsVisible = false;
            MEASLES1U5.IsVisible = false;
            YFLblU5.IsVisible = false;
            YFU5.IsVisible = false;
            MENALblU5.IsVisible = false;
            MENAU5.IsVisible = false;
            MEASLES2LblU5.IsVisible = false;
            MEASLES2U5.IsVisible = false;
        }

        void others_checkboxU5_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var checkedItem = sender as CheckBox;

            if (checkedItem.IsChecked == true)
            {
                others_specifyChkboxEntryU5.IsVisible = true;
            }
            else
            {
                others_specifyChkboxEntryU5.IsVisible = false;
            }
        }

        void submitZDBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            UnderFive.ChildID = childID.Text;
            UnderFive.UnderFiveChildName = childNameU5.Text;

            if (AgePickerUnderFive.SelectedIndex != -1) {

                UnderFive.UnderFiveChildAge = AgePickerUnderFive.SelectedItem.ToString();
            }

            if(genderPickerUnderFive.SelectedIndex != -1)
            {
                UnderFive.UnderFiveChildGender = genderPickerUnderFive.SelectedItem.ToString();
            }
            
            UnderFive.UnderFiveCaregiverName = caregiverNameEntryU5.Text;
            UnderFive.UnderFiveCaregiverPhone = caregiverContactEntryU5.Text;


            if (CommLangPicker.SelectedIndex != -1)
            {

                UnderFive.PrefferedCommLanguage = CommLangPicker.SelectedItem.ToString();
            }

            if (CommMethodPicker.SelectedIndex != -1)
            {

                UnderFive.PrefferedCommMethod = CommMethodPicker.SelectedItem.ToString();
            }

            UnderFive.UnderFiveReceivedAntigen = ChildReceivedAntigenU5.Text;
            UnderFive.UnderFiveHaveVaccinationCard = childHaveVaccinationCardU5.Text;
            UnderFive.UnderFiveAdministeredAntigen = administeredAntigensU5.Text;
            UnderFive.UnderFiveReasonForNotVaccination = ReasonsForNotVaccination();
            UnderFive.InterviewerName = interviewerNameEntry.Text;
            UnderFive.TeamCode = teamCodeEntry.Text;
            UnderFive.PhoneNumber = phoneNoEntry.Text;
            UnderFive.SettlementName = settlementEntry.Text;

            UnderFive.UnderFiveReceivedAntigen = ReceivedAntigenValue();
            UnderFive.UnderFiveHaveVaccinationCard = HaveVaccinationCardValue();
            UnderFive.UnderFiveAdministeredAntigen = AllAdministeredAntigens();
            UnderFive.Motivation = AllMotivation();
            UnderFive.UnderFiveReasonForNotVaccination = ReasonsForNotVaccination();



            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<UnderFive>();
                int rows = conn.Insert(UnderFive);

                if (rows > 0)
                {
                    DisplayAlert("Success", "New Under Five Record successfully saved", "OK");
                    childNameU5.Text = string.Empty;
                    AgePickerUnderFive.SelectedIndex = -1;
                    genderPickerUnderFive.SelectedIndex = -1;
                    caregiverNameEntryU5.Text = string.Empty;
                    caregiverContactEntryU5.Text = string.Empty;
                    CommLangPicker.SelectedIndex = -1;  
                    CommMethodPicker.SelectedIndex = -1;  
                    ChildReceivedAntigenU5.Text = string.Empty;
                    childHaveVaccinationCardU5.Text = string.Empty;
                    administeredAntigensU5.Text = string.Empty;
                    interviewerNameEntry.Text = string.Empty;
                    teamCodeEntry.Text = string.Empty;
                    phoneNoEntry.Text = string.Empty;
                    settlementEntry.Text = string.Empty;
                    ResetAntigents();
                    ResetReasonsForNotVaccination();

                }
                else
                {
                    DisplayAlert("Failure", "Error Saving New Under Five Record", "OK");
                }

            }
        }


        public void ResetAntigents()
        {
            OPV0U5.IsChecked = false;
            HepB0U5.IsChecked = false;
            BCGU5.IsChecked = false;
            OPV1U5.IsChecked = false;
            PENTA1U5.IsChecked = false;
            PCV1U5.IsChecked = false;
            ROTA1U5.IsChecked = false;
            IPV1U5.IsChecked = false;
            OPV2U5.IsChecked = false;
            PENTA2U5.IsChecked = false;
            PCV2U5.IsChecked = false;
            ROTA2U5.IsChecked = false;
            OPV3U5.IsChecked = false;
            PENTA3U5.IsChecked = false;
            PCV3U5.IsChecked = false;
            IPV2U5.IsChecked = false;
            MEASLES1U5.IsChecked = false;
            YFU5.IsChecked = false;
            MENAU5.IsChecked = false;
            MEASLES2U5.IsChecked = false;
        }


        public void ResetReasonsForNotVaccination()
        {
            unawareOfVaccinationNeedU5.IsChecked = false;
            didNotKnowChildNeedVaccineU5.IsChecked = false;
            vaccinationNotImportantU5.IsChecked = false;
            doNotTrustVaccineU5.IsChecked = false;
            fearOfSideEffectU5.IsChecked = false;
            motherTooBusyU5.IsChecked = false;
            forgotToTakeChildU5.IsChecked = false;
            headOfHousWontAllowU5.IsChecked = false;
            unableToPayTransportU5.IsChecked = false;
            familiyProblemillnessU5.IsChecked = false;
            others_checkboxU5.IsChecked = false;
            QuestionLabelU5.IsEnabled = false;
        }

        void ViewAssement_ToolBar(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new CompletedPage());
        }
    }
}

