using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeTracker.Core.Schema.Entities;

namespace TimeTracker.Core.Schema.Services
{
	public interface IDataContext
	{
        IRepository<TimeSlice> TimeSlices { get; }

		void Commit();
	}
}
