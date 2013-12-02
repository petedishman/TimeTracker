using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeTracker.Core.Schema.Entities;
using TimeTracker.Core.Schema.Services;

namespace TimeTracker.Core.Schema.Infrastructure
{
	public class SqlDataContext : IDataContext
	{
		public SqlDataContext()
		{
			_context = new TimeTrackerDataContext();
		}
        
		public IRepository<TimeSegment> TimeSegments
		{
			get
			{
                if (_timeSegments == null)
				{
                    _timeSegments = new SqlRepository<TimeSegment>(_context.TimeSegments, _context);
				}
                return _timeSegments;
			}
		}

		public void Commit()
		{
			_context.SaveChanges();
		}

		SqlRepository<TimeSegment> _timeSegments = null;

		readonly TimeTrackerDataContext _context;

	}
}
