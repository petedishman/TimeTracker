using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using TimeTracker.Core.Schema.Entities;

namespace TimeTracker.Core.Schema.Infrastructure
{
	public class TimeTrackerDataContext : DbContext
	{
		public TimeTrackerDataContext()
			: base("TimeTracker")
		{
	
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		public DbSet<TimeSegment> TimeSegments { get; set; }
	}
}
