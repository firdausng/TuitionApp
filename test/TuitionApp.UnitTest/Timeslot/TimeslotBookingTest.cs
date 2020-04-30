using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TuitionApp.Core.Domain.Entities;
using Xunit;

namespace TuitionApp.UnitTest.Timeslot
{
    public class TimeslotBookingTest
    {
        Core.Domain.Entities.Timeslot timeslot1 = new Core.Domain.Entities.Timeslot
        {
            Id = Guid.NewGuid(),
            StartTime = new TimeSpan(0, 20, 0),
            Duration = new TimeSpan(0, 1, 0),
        };

        Core.Domain.Entities.Timeslot timeslot2 = new Core.Domain.Entities.Timeslot
        {
            Id = Guid.NewGuid(),
            StartTime = new TimeSpan(0, 22, 0),
            Duration = new TimeSpan(0, 1, 0),
        };

        List<Core.Domain.Entities.Timeslot> list = new List<Core.Domain.Entities.Timeslot>();

        public TimeslotBookingTest()
        {
            list = new List<Core.Domain.Entities.Timeslot>
            {
                timeslot1, timeslot2
            };
        }


        [Fact]
        public void ShouldGetOverlapTimeslot()
        {
            var overlapList = list.GetOverlapTimeslot(new TimeSpan(0, 20, 30), new TimeSpan(0, 1, 0));

            overlapList.ShouldContain(timeslot1);
        }

        [Fact]
        public void ShouldNotGetOverlapTimeslot()
        {
            var overlapList = list.GetOverlapTimeslot(new TimeSpan(0, 15, 30), new TimeSpan(0, 1, 0));

            overlapList.ShouldBeEmpty();
        }

        [Fact]
        public void ShouldGetOverlapTimeslotWhenExactTimeAndDurationAdded()
        {
            var overlapList = list.GetOverlapTimeslot(new TimeSpan(0, 20, 00), new TimeSpan(0, 1, 0));

            overlapList.ShouldContain(timeslot1);
        }

    }
}
