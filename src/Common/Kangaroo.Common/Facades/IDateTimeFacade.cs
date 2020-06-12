using System;

namespace Kangaroo.Common.Facades
{
    public interface IDateTimeFacade
    {
        DateTime UtcNow { get; }
    }
}
