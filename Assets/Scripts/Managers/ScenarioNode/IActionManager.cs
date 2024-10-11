public interface IActionManager
{
	event System.Action OnNodeCompleted;
	string nextScenarioNodeName { get; set; }
	void LoadData(BaseAction action);
	void StartAction();
	void EndAction();
}
