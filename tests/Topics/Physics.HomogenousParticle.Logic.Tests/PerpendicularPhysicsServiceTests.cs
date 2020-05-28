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
                PerpendicularInductionOrientation.FromPaper,
                "#000000");
            var physicsService = new PerpendicularPhysicsService(motionSetup);

            Assert.Equal(0.0011375, physicsService.ComputeRadius(), 10);
            Assert.Equal(1758241758.0, physicsService.ComputeOmega(), 0);
        }

        [Fact]
        public void ElectronValues()
        {
            var motionSetup = new PerpendicularMotionSetup(
                -1,
                0.00091f,
                2,
                0.01f,
                PerpendicularInductionOrientation.FromPaper,
                "#000000");
            var physicsService = new PerpendicularPhysicsService(motionSetup);

            Assert.Equal(0.0011375, physicsService.ComputeRadius(), 10);            
            Assert.Equal(0.0000000035, physicsService.ComputeT(), 10);
        }

        [Fact]
        public void ProtonValues()
        {
            var motionSetup = new PerpendicularMotionSetup(
                1,
                1.67f,
                1,
                0.1f,
                PerpendicularInductionOrientation.FromPaper,
                "#000000");
            var physicsService = new PerpendicularPhysicsService(motionSetup);

            Assert.Equal(0.104375, physicsService.ComputeRadius(), 6);
            
        }

        [Fact]
        public void GoldValues()
        {
            var motionSeutp = new PerpendicularMotionSetup(
                3,
                329,
                3,
                2,
                PerpendicularInductionOrientation.FromPaper,
                "#000000");
            var physicsService = new PerpendicularPhysicsService(motionSeutp);

            Assert.Equal(1.0, physicsService.ComputeRadius(), 1);
        }
    }
}
