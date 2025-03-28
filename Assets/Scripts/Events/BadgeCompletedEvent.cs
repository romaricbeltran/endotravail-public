using Unity.Services.Analytics;

public class BadgeCompletedEvent : Event
{
	public BadgeCompletedEvent() : base("badgeCompleted") {}

	public string BadgeName { set { SetParameter("badgeName", value); } }
}
