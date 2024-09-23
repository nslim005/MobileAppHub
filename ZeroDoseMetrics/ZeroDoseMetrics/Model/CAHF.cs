using System;
using SQLite;

namespace ZeroDoseMetrics.Model
{
	public class CAHF
	{

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int LGAId { get; set; }

        public int WardId { get; set; }

        public string CAHFName { get; set; }

        public int Status { get; set; }

        public CAHF()
		{
		}
	}
}



