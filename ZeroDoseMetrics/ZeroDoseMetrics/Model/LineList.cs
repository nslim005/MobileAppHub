using System;
using SQLite;

namespace ZeroDoseMetrics.Model
{
	public class LineList
	{
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

		public string TeamCode { get; set; }

		public string ChildName { get; set; }

		public string CaregiverName { get; set; }

		public string CaregiverContact { get; set; }

		public string SettlementName { get; set; }

		public string ChildAge { get; set; }

		public double Latitude { get; set; }

		public double Longitude { get; set; }

		public string StructureID { get; set; }

        public int Completed { get; set; }

        public string InterviewerName { get; set; }

        public string PhoneNo { get; set; }

		public string Address { get; set; }

		public string Response { get; set; }

		public string ReceivedAntigen { get; set; }

		public string HaveVaccinationCard { get; set; }

		public string AdministeredAntigen { get; set; }

		public string MotivationForVaccination { get; set; }

		public string ReasonForNotVaccination { get; set; }

		public string ChildLessThanFiveNotEnumerated { get; set; }

        public LineList()
		{
			
		}
	}
}
