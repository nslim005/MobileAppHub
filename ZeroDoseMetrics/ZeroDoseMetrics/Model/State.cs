using System;
using SQLite;

namespace ZeroDoseMetrics.Model
{
	public class State
	{
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string StateName { get; set; }

        public int Status { get; set; }


        public State()
		{
		}
	}
}

