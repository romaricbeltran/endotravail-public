using Unity.Services.Analytics;

public class FlagCompletedEvent : Event
{
	public FlagCompletedEvent() : base("flagCompleted") {}

	public string FlagName { set { SetParameter("flagName", value); } }
}
