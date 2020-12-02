namespace Physics.Shared.UI.Views.Interactions
{
	public interface ISetSimulationViewInteraction<TController>
    {
		void SetViewInteraction(ISimulationViewInteraction<TController> controller);
    }
}
