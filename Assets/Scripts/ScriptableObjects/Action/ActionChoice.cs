using UnityEngine;

[System.Serializable]
public class ActionChoice
{
	[SerializeField] [TextArea( 3, 10 )] private string choiceText;
	[SerializeField] private BaseScenarioNode scenarioNode;

    public global::System.String ChoiceText { get => choiceText; set => choiceText = value; }
    public BaseScenarioNode ScenarioNode { get => scenarioNode; set => scenarioNode = value; }
}
