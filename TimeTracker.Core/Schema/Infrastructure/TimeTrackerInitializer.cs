using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Core.Schema.Infrastructure
{
    class TimeTrackerInitializer : DropCreateDatabaseIfModelChanges<TimeTrackerDataContext>
    {
        protected override void Seed(TimeTrackerDataContext context)
        {
            // TODO:
            // Generate some test data for the last couple of days
            base.Seed(context);
        }
    }
}
