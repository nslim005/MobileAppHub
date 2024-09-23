using System;
using SQLite;

namespace ZeroDoseMetrics.Model
{
	public class Ward
	{
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int LGAId { get; set; }

        public string WardName { get; set; }

        public int Status { get; set; }

        public Ward()
		{

		}
	}
}

