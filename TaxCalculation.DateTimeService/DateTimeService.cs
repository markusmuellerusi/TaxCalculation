using System;
using TaxCalculation.Common;

namespace TaxCalculation.DateTimeService
{
    public class DateTimeService: IDateTimeService
    {
        private readonly DateTime? _now;

        public DateTimeService()
        {
        }

        public DateTimeService(DateTime now)
        {
            _now = now;
        }

        public DateTime Now => _now ?? DateTime.Now;
    }
}
