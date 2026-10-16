using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Promitor.Integrations.AzureMonitor;
using Xunit;

namespace Promitor.Tests.Unit.Metrics
{
    [Category("Unit")]
    public class MetricTimeGrainSelectorTests
    {
        private static readonly TimeSpan aggregationInterval = TimeSpan.FromMinutes(5);
        private static readonly TimeSpan historyWindow = TimeSpan.FromHours(12);
        private static readonly DateTime utcNow = new(2026, 9, 28, 12, 7, 30, DateTimeKind.Utc);

        [Fact]
        public void DetermineQueryEndTime_NoOffset_ReturnsNow()
        {
            var queryEndTime = MetricTimeGrainSelector.DetermineQueryEndTime(utcNow, aggregationOffset: null, historyWindow);

            Assert.Equal(utcNow, queryEndTime);
        }

        [Fact]
        public void DetermineQueryEndTime_WithOffset_ShiftsBack()
        {
            var queryEndTime = MetricTimeGrainSelector.DetermineQueryEndTime(utcNow, TimeSpan.FromMinutes(10), historyWindow);

            Assert.Equal(utcNow.AddMinutes(-10), queryEndTime);
        }

        [Fact]
        public void DetermineQueryEndTime_OffsetEqualToHistoryWindow_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => MetricTimeGrainSelector.DetermineQueryEndTime(utcNow, historyWindow, historyWindow));
        }

        [Fact]
        public void DetermineQueryEndTime_NegativeOffset_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => MetricTimeGrainSelector.DetermineQueryEndTime(utcNow, TimeSpan.FromMinutes(-5), historyWindow));
        }

        [Fact]
        public void SelectMostRecent_NoOffset_SelectsLatestTimeGrain()
        {
            // Arrange
            var timeGrains = CreateTimeGrainsUpTo(utcNow);
            var queryEndTime = MetricTimeGrainSelector.DetermineQueryEndTime(utcNow, aggregationOffset: null, historyWindow);

            // Act
            var selected = MetricTimeGrainSelector.SelectMostRecent(timeGrains, grain => grain.TimeStamp, queryEndTime);

            // Assert
            Assert.Equal(new DateTimeOffset(2026, 9, 28, 12, 5, 0, TimeSpan.Zero), selected.TimeStamp);
        }

        [Theory]
        [InlineData(5, 12, 0)]
        [InlineData(10, 11, 55)]
        [InlineData(60, 11, 5)]
        public void SelectMostRecent_WithOffset_SkipsDelayedTimeGrains(int offsetInMinutes, int expectedHour, int expectedMinute)
        {
            // Arrange
            var timeGrains = CreateTimeGrainsUpTo(utcNow);
            var queryEndTime = MetricTimeGrainSelector.DetermineQueryEndTime(utcNow, TimeSpan.FromMinutes(offsetInMinutes), historyWindow);

            // Act
            var selected = MetricTimeGrainSelector.SelectMostRecent(timeGrains, grain => grain.TimeStamp, queryEndTime);

            // Assert
            Assert.Equal(new DateTimeOffset(2026, 9, 28, expectedHour, expectedMinute, 0, TimeSpan.Zero), selected.TimeStamp);
        }

        [Fact]
        public void SelectMostRecent_WithOffset_ReturnsValueWhereLatestTimeGrainsAreStillEmpty()
        {
            // Arrange
            var timeGrains = CreateTimeGrainsUpTo(utcNow);
            timeGrains.Where(grain => grain.TimeStamp >= new DateTimeOffset(utcNow.AddMinutes(-10))).ToList().ForEach(grain => grain.Average = null);
            var queryEndTime = MetricTimeGrainSelector.DetermineQueryEndTime(utcNow, TimeSpan.FromMinutes(15), historyWindow);

            // Act
            var selected = MetricTimeGrainSelector.SelectMostRecent(timeGrains, grain => grain.TimeStamp, queryEndTime);

            // Assert
            Assert.NotNull(selected.Average);
        }

        [Fact]
        public void SelectMostRecent_NoTimeGrainBeforeQueryEndTime_ReturnsNull()
        {
            var timeGrains = new List<TimeGrain> { new(new DateTimeOffset(utcNow.AddMinutes(10)), 1) };

            var selected = MetricTimeGrainSelector.SelectMostRecent(timeGrains, grain => grain.TimeStamp, utcNow);

            Assert.Null(selected);
        }

        private static List<TimeGrain> CreateTimeGrainsUpTo(DateTime now)
        {
            var latestTimeGrainStart = new DateTime(now.Ticks - (now.Ticks % aggregationInterval.Ticks), DateTimeKind.Utc);
            return Enumerable.Range(0, 24)
                .Select(index => new TimeGrain(new DateTimeOffset(latestTimeGrainStart.AddMinutes(-5 * index)), index))
                .ToList();
        }

        private class TimeGrain(DateTimeOffset timeStamp, double? average)
        {
            public DateTimeOffset TimeStamp { get; } = timeStamp;
            public double? Average { get; set; } = average;
        }
    }
}
