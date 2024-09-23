using System;
using SQLite;

namespace ZeroDoseMetrics.Model
{
	public class UnderFive
	{

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string UnderFiveChildName { get; set; }

        public string UnderFiveChildAge { get; set; }

        public string UnderFiveChildGender { get; set; }

        public string UnderFiveCaregiverName { get; set; }

        public string UnderFiveCaregiverPhone { get; set; }

        public string PrefferedCommMethod { get; set; }

        public string PrefferedCommLanguage { get; set; }

        public string UnderFiveReceivedAntigen { get; set; }

        public string UnderFiveHaveVaccinationCard { get; set; }

        public string UnderFiveAdministeredAntigen { get; set; }

        public string UnderFiveReasonForNotVaccination { get; set; }

        public string Motivation { get; set; }

        public string InterviewerName { get; set; }

        public string PhoneNumber { get; set; }

        public string TeamCode { get; set; }

        public string ChildID { get; set; }

        public string SettlementName { get; set; }



        public UnderFive()
		{


		}
	}
}

