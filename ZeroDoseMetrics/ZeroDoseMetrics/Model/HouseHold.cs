using System;
using SQLite;

namespace ZeroDoseMetrics.Model
{
	public class HouseHold
	{
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string HouseHoldID { get; set; }

        public string Respondent { get; set; }

        public int RespondentContact { get; set; }

        public HouseHold()
		{

		}
	}
}

