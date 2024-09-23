using System;
using SQLite;

namespace ZeroDoseMetrics.Model
{
	public class LGA
	{
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int StateId { get; set; }

        public string LGAName { get; set; }

        public int Status { get; set; }


        public LGA()
		{
		}
	}
}

