using Hourglass.Pages;
using System;
using Xunit;

namespace HourglassTests
{
    public class SessionPageTests
    {
        [Fact]
        public void IsValidTimeRange_ShouldReturnTrueForValidRange()
        {
            // Arrange
            var page = new TemplateSession();
            page.StartTime = DateTime.Parse("2023-01-01 12:00"); 
            page.EndTime = DateTime.Parse("2023-01-01 14:00");   

            // Act
            var result = page.IsValidTimeRange();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidTimeRange_ShouldReturnFalseForInvalidRange()
        {
            // Arrange
            var page = new TemplateSession();
            page.StartTime = DateTime.Parse("2023-01-01 14:00"); 
            page.EndTime = DateTime.Parse("2023-01-01 12:00");   

            // Act
            var result = page.IsValidTimeRange();

            // Assert
            Assert.False(result);
        }

        
    }
}
