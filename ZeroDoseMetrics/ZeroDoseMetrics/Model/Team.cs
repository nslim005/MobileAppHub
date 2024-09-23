using System;
using SQLite;

namespace ZeroDoseMetrics.Model
{
	public class Team
	{
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int LGAId { get; set; }

        public int WardId { get; set; }

        public int CAHFId { get; set; }

        public string TeamCode { get; set; }

        public int Status { get; set; }


        public Team()
		{
		}
	}
}

