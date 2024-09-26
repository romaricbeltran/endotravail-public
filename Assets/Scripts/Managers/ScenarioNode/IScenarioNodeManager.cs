public interface IScenarioNodeManager
{
	event System.Action OnNodeCompleted;
	void LoadData(BaseScenarioNode scenarioNode);
	void StartNode();
	void EndNode();
	BaseScenarioNode GetNextNode();
}
