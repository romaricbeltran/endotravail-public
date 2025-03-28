using Unity.Services.Analytics;

public class GameProgressEvent : Event
{
	public GameProgressEvent() : base("gameProgress") {}

	public string GameProgressStepName { set { SetParameter("gameProgressStepName", value); } }
}
