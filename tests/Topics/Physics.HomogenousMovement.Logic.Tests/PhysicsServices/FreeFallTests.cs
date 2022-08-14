using System.Numerics;
using Physics.HomogenousMovement.PhysicsServices;
using Xunit;

namespace Physics.HomogenousMovement.Logic.Tests.PhysicsServices
{
    public class FreeFallTests
    {
        [Fact]
        public void PropertiesAreCorrectlySet()
        {
            var physicsService = MotionFactory.CreateFreeFall(new System.Numerics.Vector2(0, 100), 3, 0.1f, "#000000", 10f);
            Assert.Equal(100, physicsService.Origin.Y);
            Assert.Equal(0, physicsService.Origin.X);
            Assert.Equal(3, physicsService.Mass);
            Assert.Equal(0, physicsService.V0);
        }

        [Fact]
        public void FreeFallFromOneMeter()
        {
            var physicsService = new PhysicsService(MotionFactory.CreateFreeFall(new System.Numerics.Vector2(0, 1), 3, 0.1f, "#000000", 10f));
            var startY = physicsService.ComputeY(0);
            Assert.Equal(1, startY);
            var endY = physicsService.ComputeY(0.4472f);
            Assert.Equal(0f, endY, 2f);
            var beforeEndY = physicsService.ComputeY(0.3f);
            Assert.NotEqual(0f, beforeEndY, 2);
        }

        [Fact]
        public void FreeFallFromGround()
        {
            var physicsService = new PhysicsService(MotionFactory.CreateFreeFall(new Vector2(0, 0), 1, 0.1f, "#000000", 10));
            var fallTime = physicsService.MaxT;
            Assert.Equal(0, fallTime);
            var maxY = physicsService.MaxY;
            Assert.Equal(0, maxY);
            var maxX = physicsService.MaxX;
            Assert.Equal(0, maxX);
        }

        [Fact]
        public void FreeFallOnMoon()
        {
            var physicsService = new PhysicsService(MotionFactory.CreateFreeFall(new Vector2(0, 4.5f), 1, 0.1f, "#000000", 1.62f));
            var fallTime = physicsService.MaxT;
            Assert.Equal(2.357f, fallTime, 3f);
            var maxY = physicsService.MaxY;
            Assert.Equal(4.5f, maxY, 1f);
            var maxX = physicsService.MaxX;
            Assert.Equal(0, maxX);
            var groundSpeed = physicsService.ComputeV(2.357f);
            Assert.Equal(3.818f, groundSpeed, 3f);
        }
    }
}
