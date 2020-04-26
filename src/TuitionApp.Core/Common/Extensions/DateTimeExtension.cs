using System;
using System.Collections.Generic;
using System.Text;

namespace TuitionApp.Core.Common.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime DateTimeWithoutMilisecond(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Kind);
        }
    }
}
