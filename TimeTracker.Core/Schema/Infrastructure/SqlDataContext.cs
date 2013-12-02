using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeTracker.Core.Schema.Entities;
using TimeTracker.Core.Schema.Services;

namespace TimeTracker.Core.Schema.Infrastructure
{
	public class SqlDataContext : IDataContext,  IDisposable
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

        #region IDisposable
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                    _context.Dispose();
                }

                // release any unmanaged resources here
                disposed = true;
            }
        }
        ~SqlDataContext()
        {
            Dispose(false);
        }
        #endregion
	}
}
