namespace Physics.Shared.UI.Views.Interactions
{
	public interface ISimulationViewInteraction<TController>
    {
		TController PrepareController();        
    }
}
