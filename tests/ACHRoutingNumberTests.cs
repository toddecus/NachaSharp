using Xunit;

namespace NachaSharp.Tests
{
    public class ACHRoutingNumberTests
    {
        [Fact]
        public void IsValidRoutingNumber_ValidNumber_ReturnsTrue()
        {
            // Arrange
            var routingNumber = "071000505"; // Replace with a valid routing number
            var achRoutingNumber = new ACHRoutingNumber(routingNumber);

            // Assert
            Assert.True(achRoutingNumber.ToRoutingString() == "071000505");
        }

        [Fact]
        public void IsValidRoutingNumber_InvalidNumber_ReturnsFalse()
        {
            // Arrange
            var routingNumber = "123456782"; // Replace with an invalid routing number
            // Assert
            Assert.Throws<ArgumentException>(() => new ACHRoutingNumber(routingNumber));

        }

        [Fact]
        public void IsValidRoutingNumber_EmptyNumber_ReturnsFalse()
        {
            // Arrange
            var routingNumber = "";
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new ACHRoutingNumber(routingNumber));
        }

    }
}