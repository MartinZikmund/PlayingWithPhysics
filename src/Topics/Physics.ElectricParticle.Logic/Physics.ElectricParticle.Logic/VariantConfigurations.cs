using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics.ElectricParticle.Logic
{
    public static class VariantConfigurations
    {
        public readonly static VariantConfiguration[] All { get; } = new VariantConfiguration[]
		{
			new VariantConfiguration()
			{
				InputVariant = InputVariant.EasyVertical,
				ParticleType = ParticleType.ChargedBody,
				Q = FieldConfiguration.CreateRestricted(-5, 5, 1),
				M = FieldConfiguration.CreateRestricted(1, 10000, 1, 0)
			},
			new VariantConfiguration()
			{
				InputVariant = InputVariant.EasyVertical,
				ParticleType = ParticleType.AtomNucleus,
				Q = FieldConfiguration.
			}
		}
    }
}
