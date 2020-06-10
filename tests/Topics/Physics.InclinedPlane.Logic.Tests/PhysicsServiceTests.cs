using Physics.InclinedPlane.Logic.PhysicsServices;
using Physics.InclinedPlane.Services;
using System;
using Xunit;

namespace Physics.InclinedPlane.Logic.Tests
{
    public class PhysicsServiceTests
    {
        [InlineData(0.0, 0.0, 0.0, 7.1, 5.0)]
        [InlineData(0.1, 0.5, 0.4, 6.7, 5.1)]
        [InlineData(0.2, 1.0, 0.7, 6.3, 5.3)]
        [InlineData(0.3, 1.6, 1.1, 6.0, 5.4)]
        [InlineData(0.4, 2.1, 1.5, 5.6, 5.6)]
        [InlineData(0.5, 2.7, 1.9, 5.2, 5.7)]
        [InlineData(0.6, 3.2, 2.3, 4.8, 5.8)]
        [InlineData(0.7, 3.8, 2.7, 4.4, 6.0)]
        [InlineData(0.8, 4.4, 3.1, 3.9, 6.1)]
        [InlineData(0.9, 5.1, 3.6, 3.5, 6.2)]
        [InlineData(1.0, 5.7, 4.0, 3.0, 6.4)]
        [InlineData(1.1, 6.3, 4.5, 2.6, 6.5)]
        [InlineData(1.2, 7.0, 4.9, 2.1, 6.7)]
        [InlineData(1.3, 7.7, 5.4, 1.6, 6.8)]
        [InlineData(1.4, 8.4, 5.9, 1.2, 6.9)]
        [InlineData(1.5, 9.1, 6.4, 0.7, 7.1)]
        [InlineData(1.6, 9.8, 6.9, 0.2, 7.2)]
        [InlineData(1.8, 10, 7.1, 0, 7.3)]
        [InlineData(30, 10, 7.1, 0, 7.3)]
        [Theory]
        public void AcceleratingMotionWithoutFinish(float t, double expectedS, double expectedX, double expectedY, double expectedV)
        {
            var physicsService = new PhysicsService(new InclinedPlaneMotionSetup(1, 5, 9.81f, 10, 0.8f, 45, 0, 0, "#000000"));
            Assert.Equal(expectedS, physicsService.CalculateS(t), 1);
            Assert.Equal(expectedX, physicsService.CalculateX(t), 1);
            Assert.Equal(expectedY, physicsService.CalculateY(t), 1);
            Assert.Equal(expectedV, physicsService.CalculateV(t), 1);
        }

        [InlineData(0.0, 0.0, 0.0, 7.1, 5.0)]
        [InlineData(0.1, 0.5, 0.4, 6.7, 5.0)]
        [InlineData(0.2, 1.0, 0.7, 6.4, 5.0)]
        [InlineData(0.3, 1.5, 1.1, 6.0, 5.0)]
        [InlineData(0.4, 2.0, 1.4, 5.7, 5.0)]
        [InlineData(0.5, 2.5, 1.8, 5.3, 5.0)]
        [InlineData(0.6, 3.0, 2.1, 4.9, 5.0)]
        [InlineData(0.7, 3.5, 2.5, 4.6, 5.0)]
        [InlineData(0.8, 4.0, 2.8, 4.2, 5.0)]
        [InlineData(0.9, 4.5, 3.2, 3.9, 5.0)]
        [InlineData(1.0, 5.0, 3.5, 3.5, 5.0)]
        [InlineData(1.1, 5.5, 3.9, 3.2, 5.0)]
        [InlineData(1.2, 6.0, 4.2, 2.8, 5.0)]
        [InlineData(1.3, 6.5, 4.6, 2.5, 5.0)]
        [InlineData(1.4, 7.0, 4.9, 2.1, 5.0)]
        [InlineData(1.5, 7.5, 5.3, 1.8, 5.0)]
        [InlineData(1.6, 8.0, 5.7, 1.4, 5.0)]
        [InlineData(1.7, 8.5, 6.0, 1.1, 5.0)]
        [InlineData(1.8, 9.0, 6.4, 0.7, 5.0)]
        [InlineData(1.9, 9.5, 6.7, 0.4, 5.0)]
        [InlineData(2.0, 10.0, 7.1, 0.0, 5.0)]
        [InlineData(30, 10.0, 7.1, 0.0, 5.0)]
        [Theory]
        public void StableMotionWithoutFinish(float t, double expectedS, double expectedX, double expectedY, double expectedV)
        {
            var physicsService = new PhysicsService(new InclinedPlaneMotionSetup(1, 5, 9.81f, 10, 1f, 45, 0, 0, "#000000"));
            Assert.Equal(expectedS, physicsService.CalculateS(t), 1);
            Assert.Equal(expectedX, physicsService.CalculateX(t), 1);
            Assert.Equal(expectedY, physicsService.CalculateY(t), 1);
            Assert.Equal(expectedV, physicsService.CalculateV(t), 1);
        }

        [InlineData(0.0, 0.0, 0.0, 7.1, 5.0)]
        [InlineData(0.1, 0.5, 0.3, 6.7, 4.7)]
        [InlineData(0.2, 0.9, 0.7, 6.4, 4.4)]
        [InlineData(0.3, 1.4, 1.0, 6.1, 4.2)]
        [InlineData(0.4, 1.8, 1.3, 5.8, 3.9)]
        [InlineData(0.5, 2.2, 1.5, 5.5, 3.6)]
        [InlineData(0.6, 2.5, 1.8, 5.3, 3.3)]
        [InlineData(0.7, 2.8, 2.0, 5.1, 3.1)]
        [InlineData(0.8, 3.1, 2.2, 4.9, 2.8)]
        [InlineData(0.9, 3.4, 2.4, 4.7, 2.5)]
        [InlineData(1.0, 3.6, 2.6, 4.5, 2.2)]
        [InlineData(1.1, 3.8, 2.7, 4.4, 1.9)]
        [InlineData(1.2, 4.0, 2.8, 4.2, 1.7)]
        [InlineData(1.3, 4.2, 2.9, 4.1, 1.4)]
        [InlineData(1.4, 4.3, 3.0, 4.0, 1.1)]
        [InlineData(1.5, 4.4, 3.1, 4.0, 0.8)]
        [InlineData(1.6, 4.4, 3.1, 3.9, 0.6)]
        [InlineData(1.7, 4.5, 3.2, 3.9, 0.3)]
        [InlineData(1.8, 4.5, 3.2, 3.9, 0.0)]
        [InlineData(30, 4.5, 3.2, 3.9, 0.0)]
        [Theory]
        public void DecceleratingMotionWithoutFinish1(float t, double expectedS, double expectedX, double expectedY, double expectedV)
        {
            var physicsService = new PhysicsService(new InclinedPlaneMotionSetup(1, 5, 9.81f, 10, 1.4f, 45, 0, 0, "#000000"));
            Assert.Equal(expectedS, physicsService.CalculateS(t), 1);
            Assert.Equal(expectedX, physicsService.CalculateX(t), 1);
            Assert.Equal(expectedY, physicsService.CalculateY(t), 1);
            Assert.Equal(expectedV, physicsService.CalculateV(t), 1);
        }
    }
}
