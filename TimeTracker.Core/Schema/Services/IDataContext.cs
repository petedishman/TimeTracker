using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeTracker.Core.Schema.Entities;

namespace TimeTracker.Core.Schema.Services
{
	public interface IDataContext
	{
        IRepository<TimeSegment> TimeSegments { get; }

		void Commit();
	}
}
