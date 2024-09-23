using System;
using SQLite;

namespace ZeroDoseMetrics.Model
{
	public class Settlement
	{
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int LGAId { get; set; }

        public int WardId { get; set; }

        public int CAHFId { get; set; }

        public int TeamId { get; set; }

        public string SettlementName { get; set; }

        public int Status { get; set; }


        public Settlement()
		{
		}
	}
}

