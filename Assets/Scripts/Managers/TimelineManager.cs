using StarterAssets;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

/*
 * Les Dialogue, Popup, Action, Menu, EndGame sont des nœuds.
 * Les Badge ne sont pas des nœuds mais se comportent de manière similaire (ils ne renvoient pas de nœuds eux-mêmes et ne sont pas indépendants).
 * TimelineManager est responsable de lire les clips des différents nœuds, de les charger et, si un nœud contient un Badge, il charge aussi le Badge.
 * Les Dialogue, Popup, Action et Badge, une fois chargés, s'affichent depuis l'éditeur quand on le souhaite en activant startNode avec un signal depuis la timeline, donc pas besoin de s'occuper de les afficher.
 * Les Mission s'affichent dès le début du nœud, donc on les affiche directement à la fin du chargement.
 * La timeline attend que le nœud soit fini (observer) pour lancer le prochain nœud.
 * Si le nœud renvoie un autre nœud en référence, il le joue. Sinon, s'il ne renvoie rien, il joue le nœud suivant dans son scénario.
 * Si le nœud est une fin de jeu ou un nœud de menu, on met fin à la lecture du scénario.
 * Les flags permettent de stocker en mémoire des variables, que les nœuds peuvent utiliser pour aller vers un nœud ou un autre (branches)  
 *
*/
public class TimelineManager : MonoBehaviour
{
    public Chapter chapter;
    public LevelLoader levelLoader;
    public GameManager gameManager;
	public BadgeManager badgeManager;
	public FlagManager flagManager;
	public ChoiceActionManager choiceActionManager;
	public DialogueActionManager dialogueActionManager;
	public PopupActionManager popupActionManager;
	public MissionActionManager missionActionManager;
	public EndGameActionManager endGameActionManager;

	private ScenarioNode currentScenarioNode;
	private IActionManager currentActionManager;
	private bool endGame = false;

	private Dictionary<string, ScenarioNode> nodeDictionary;

	private void Awake()
	{
		nodeDictionary = new Dictionary<string, ScenarioNode>();

		foreach ( ScenarioNode node in chapter.Scenario )
		{
			AddNodeAndChildrenToDictionary( node );
		}

		Debug.Log( $"Node dictionary initialized with {nodeDictionary.Count} nodes." );
	}

	private void AddNodeAndChildrenToDictionary(ScenarioNode node)
	{
		if ( !nodeDictionary.ContainsKey( node.NodeName ) )
		{
			nodeDictionary.Add( node.NodeName, node );
			//Debug.Log( $"Node added: {node.NodeName}" );
		}
		else
		{
			Debug.LogWarning( $"Duplicate node name found: {node.NodeName}. Skipping..." );
		}

		if ( node.Children != null && node.Children.Count > 0 )
		{
			foreach ( ScenarioNode childNode in node.Children )
			{
				AddNodeAndChildrenToDictionary( childNode );
			}
		}
	}

	void Start()
	{
		PlayScenarioNode( chapter.Scenario[0] ); // Start Progression From Beginning
	}

	public void PlayScenarioNode(ScenarioNode node)
    {
		//if (MissionNode.ON_MISSION_END)
		//{
		//	if (node is ActionNode actionNode)
		//	{
		//		MissionNode.WAS_ACTION_MISSION_COMPONENT = true;
		//	}

		//	MissionNode.EndMission();
		//}
		currentScenarioNode = node;

		HandleAnalytics( currentScenarioNode.NodeName );
		HandleFlags( currentScenarioNode.Flags );
		HandleBadge( currentScenarioNode.Badge );
		HandleAction( currentScenarioNode.Action );
	}

	private void HandleAnalytics(string nodeName)
	{
		Debug.Log( "Playing Node :" + nodeName );

		//AnalyticsService.Instance.CustomData("gameProgress",
		//	new Dictionary<string, object>{{ "gameProgressStepName", nodeName } });
	}

	private void HandleFlags(List<Flag> flags)
	{
		if ( flags != null )
		{
			flagManager.SaveFlags( flags );
		}
	}

	private void HandleBadge(Badge badge)
	{
		if ( badge != null )
		{
			badgeManager.LoadBadge( badge );
		}
	}

	private void HandleAction(BaseAction action)
	{
		gameManager.SwitchPlayerInput( false );

		if ( action.TimelineClip )
		{
			GetComponent<PlayableDirector>().Play( action.TimelineClip, action.DirectorWrapMode );
		}

		switch ( action )
		{
			case ChoiceAction choiceAction:
				LoadActionManager( choiceActionManager, choiceAction );
				break;
			case DialogueAction dialogueAction:
				LoadActionManager( dialogueActionManager, dialogueAction );
				break;
			case PopupAction popupAction:
				LoadActionManager( popupActionManager, popupAction );
				break;
			case MissionAction missionAction:
				LoadActionManager( missionActionManager, missionAction );
				break;
			case EndGameAction endGameAction:
				endGame = true;
				LoadActionManager( endGameActionManager, endGameAction );
				break;
			default:
				Debug.LogWarning( "Unsupported node type." );
				break;
		}
	}

	private void LoadActionManager(IActionManager manager, BaseAction action)
	{
		currentActionManager = manager;

		if (!endGame)
		{
			currentActionManager.OnNodeCompleted += OnNodeCompletedHandler;
		}

		manager.LoadData( action );
	}

	private void OnNodeCompletedHandler()
	{
		currentActionManager.OnNodeCompleted -= OnNodeCompletedHandler;
		PlayNextScenarioNode();
	}

	private void PlayNextScenarioNode()
	{
		ScenarioNode activeFlaggedNode = HandleFlaggedNodes( currentScenarioNode.FlaggedNodes );
		ScenarioNode returnedNextNode = !string.IsNullOrEmpty( currentActionManager.nextScenarioNodeName )
			? FindScenarioNodeByName( currentActionManager.nextScenarioNodeName )
			: null;

		if ( activeFlaggedNode != null )
		{
			PlayScenarioNode( activeFlaggedNode );
		}
		else if ( returnedNextNode != null )
		{
			PlayScenarioNode( returnedNextNode );
		}
		else if ( currentScenarioNode.Children.Count > 0 )
		{
			PlayScenarioNode( currentScenarioNode.Children[0] );
		}
		else
		{
			HandleEndChapter();
			Debug.Log( "End of chapter scenario reached." );
		}
	}

	public ScenarioNode HandleFlaggedNodes(List<FlaggedScenarioNode> flaggedNodes)
	{
		if ( flaggedNodes != null )
		{
			foreach ( FlaggedScenarioNode flaggedNode in flaggedNodes )
			{
				if ( flagManager.IsFlagActive( flaggedNode.Flag ) )
				{
					return FindScenarioNodeByName( flaggedNode.NodeName );
				}
			}
		}

		return null;
	}

	public ScenarioNode FindScenarioNodeByName(string nodeName)
	{
		if ( nodeDictionary != null && nodeDictionary.TryGetValue( nodeName, out ScenarioNode node ) )
		{
			return node;
		}

		Debug.LogWarning( $"Node with name {nodeName} not found." );
		return null;
	}

	public void HandleEndChapter()
	{
		if ( chapter.Id > GameManager.LoadProgress() )
		{
			GameManager.SaveProgress( chapter.Id );
		}

		LevelLoader.LoadMenu();
	}
}
