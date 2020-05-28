using Physics.HomogenousParticle.Services;
using Physics.HomongenousParticle.Logic.PhysicsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Physics.HomogenousParticle.Logic.Tests
{
    public class PerpendicularPhysicsServiceTests
    {
        [Fact]
        public void PozitronValues()
        {
            var motionSetup = new PerpendicularMotionSetup(
                1,
                0.00091f,
                2,
                0.01f,
                PerpendicularInductionOrientation.IntoPaper,
                "#000000");
            var physicsService = new PerpendicularPhysicsService(motionSetup);

            Assert.Equal(0.0011375m, physicsService.ComputeRadius(), 10);
            Assert.True(Math.Abs(1758241758.0m - physicsService.ComputeOmega()) < 100);
            Assert.Equal(0.0000000036m, physicsService.ComputeT(), 10);

            var t1 = 0.0000000004;
            Assert.Equal(0.000735662, physicsService.ComputeX(t1), 9);
            Assert.Equal(0.000269913, physicsService.ComputeY(t1), 9);
        }

        [Fact]
        public void ElectronValues()
        {
            var motionSetup = new PerpendicularMotionSetup(
                -1,
                0.00091f,
                2,
                0.01f,
                PerpendicularInductionOrientation.IntoPaper,
                "#000000");
            var physicsService = new PerpendicularPhysicsService(motionSetup);

            Assert.Equal(0.0011375m, physicsService.ComputeRadius(), 10);
            Assert.True(Math.Abs(1758241758.0m - physicsService.ComputeOmega()) < 100);
            Assert.Equal(0.0000000036m, physicsService.ComputeT(), 10);

            var t1 = 0.0000000004;
            Assert.Equal(0.000735662, physicsService.ComputeX(t1), 9);
            Assert.Equal(-0.000269913, physicsService.ComputeY(t1), 9);
        }

        [Fact]
        public void ProtonValues()
        {
            var motionSetup = new PerpendicularMotionSetup(
                1,
                1.67f,
                1,
                0.1f,
                PerpendicularInductionOrientation.IntoPaper,
                "#000000");
            var physicsService = new PerpendicularPhysicsService(motionSetup);
            
            Assert.Equal(0.104375m, physicsService.ComputeRadius(), 6);
            Assert.True(Math.Abs(9580838.323m- physicsService.ComputeOmega()) < 10);
            Assert.Equal(0.00000066m, physicsService.ComputeT(), 8);

            var t1 = 0.00000048;
            Assert.Equal(-0.103702405, physicsService.ComputeX(t1), 9);
            Assert.Equal(0.116205123, physicsService.ComputeY(t1), 9);
        }

        [Fact]
        public void GoldValues()
        {
            var motionSetup = new PerpendicularMotionSetup(
                3,
                329,
                3,
                2,
                PerpendicularInductionOrientation.IntoPaper,
                "#000000");
            var physicsService = new PerpendicularPhysicsService(motionSetup);

            Assert.Equal(1m, physicsService.ComputeRadius(), 1);
            //Assert.True(Math.Abs(2912621.359m - physicsService.ComputeOmega()) < 100);
            //Assert.Equal(0.0000022m, physicsService.ComputeT(), 7);

            //var t1 = 0.0000012;
            //Assert.Equal(-0.442520443, physicsService.ComputeX(t1), 9);
            //Assert.Equal(1.896758416, physicsService.ComputeY(t1), 9);
        }
    }
}
