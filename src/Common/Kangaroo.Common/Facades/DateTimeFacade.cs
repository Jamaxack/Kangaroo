using System;

namespace Kangaroo.Common.Facades
{
    public class DateTimeFacade : IDateTimeFacade
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
