using GitcSimulator.Core.Elements;

namespace GitcSimulator.Core.Attacks
{
	public class ElementalInstance
	{
		public ElementalInstance(ElementType elementType, double units)
		{
			ElementType = elementType;
			Units = units;
		}

		public ElementType ElementType { get; }

		public double Units { get; }
	}
}