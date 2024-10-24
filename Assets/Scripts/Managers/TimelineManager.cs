using StarterAssets;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class NodeLocation
{
	public ScenarioNode ParentNode;
	public int BranchIndex;
	public int NodeIndexInBranch;

	public NodeLocation(ScenarioNode parentNode, int branchIndex, int nodeIndexInBranch)
	{
		ParentNode = parentNode;
		BranchIndex = branchIndex;
		NodeIndexInBranch = nodeIndexInBranch;
	}
}

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

	private Dictionary<string, (ScenarioNode node, NodeLocation location)> nodeDictionary;

	private void Awake()
	{
		nodeDictionary = new Dictionary<string, (ScenarioNode, NodeLocation)>();

		foreach ( ScenarioNode node in chapter.Scenario )
		{
			AddNodeWithLocationToDictionary( node, null, -1, -1 ); // Depth 0
		}

		Debug.Log( $"Node dictionary initialized with {nodeDictionary.Count} nodes." );
	}

	private void AddNodeWithLocationToDictionary(ScenarioNode node, ScenarioNode parentNode, int branchIndex, int nodeIndexInBranch)
	{
		if ( !nodeDictionary.ContainsKey( node.Action.name ) )
		{
			var location = new NodeLocation( parentNode, branchIndex, nodeIndexInBranch );
			nodeDictionary.Add( node.Action.name, (node, location) );
		}
		else
		{
			Debug.LogWarning( $"Duplicate node name found: {node.Action.name}. Skipping..." );
		}

		for ( int i = 0; i < node.Branches.Count; i++ )
		{
			for ( int j = 0; j < node.Branches[i].Nodes.Count; j++ )
			{
				AddNodeWithLocationToDictionary( node.Branches[i].Nodes[j], node, i, j );
			}
		}
	}

	void Start()
	{
		PlayScenarioNode( chapter.Scenario[0] ); // Start Progression From Beginning
	}

	public void PlayScenarioNode(ScenarioNode node)
    {
		currentScenarioNode = node;

		HandleAnalytics( currentScenarioNode.Action.name );
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
		ScenarioNode nextNode = HandleFlaggedNodes( currentScenarioNode.FlaggedNodes )
			?? FindScenarioNodeByName( currentActionManager.nextScenarioNodeName )
			?? GetNextNodeInBranchOrScenario( GetLocationOfNode( currentScenarioNode ) );

		if ( nextNode != null )
		{
			PlayScenarioNode( nextNode );
		}
		else
		{
			HandleEndChapter();
		}
	}

	public ScenarioNode FindScenarioNodeByName(string nodeName)
	{
		if ( string.IsNullOrEmpty( nodeName ) || nodeDictionary == null )
		{
			return null;
		}

		if ( nodeDictionary.TryGetValue( nodeName, out var nodeData ) )
		{
			return nodeData.node;
		}

		Debug.LogWarning( $"Node with name {nodeName} not found." );

		return null;
	}

	private NodeLocation GetLocationOfNode(ScenarioNode node)
	{
		foreach ( var kvp in nodeDictionary )
		{
			if ( kvp.Value.node == node )
			{
				return kvp.Value.location;
			}
		}
		return null;
	}

	private ScenarioNode GetNextNodeInBranchOrScenario(NodeLocation nodeLocation)
	{
		// Check if we are in a valid branch
		if ( nodeLocation.ParentNode != null && nodeLocation.BranchIndex >= 0 && nodeLocation.NodeIndexInBranch >= 0 )
		{
			Branch currentBranch = nodeLocation.ParentNode.Branches[nodeLocation.BranchIndex];

			// If there's a next node in the current branch, return it
			if ( nodeLocation.NodeIndexInBranch + 1 < currentBranch.Nodes.Count )
			{
				return currentBranch.Nodes[nodeLocation.NodeIndexInBranch + 1];
			}

			// If we're at the end of the branch, recursively move up to the parent
			// This handles nested branches
			return GetNextNodeInBranchOrScenario( GetLocationOfNode( nodeLocation.ParentNode ) );
		}

		// If we're not in a branch or we've reached the root,
		// look for the next node in the main scenario
		return GetNextNodeInScenario( nodeLocation.ParentNode ?? currentScenarioNode );
	}

	private ScenarioNode GetNextNodeInScenario(ScenarioNode node)
	{
		int nodeIndex = chapter.Scenario.IndexOf( node );

		return (nodeIndex >= 0 && nodeIndex + 1 < chapter.Scenario.Count)
			? chapter.Scenario[nodeIndex + 1]
			: null;
	}

	public ScenarioNode HandleFlaggedNodes(List<FlaggedScenarioNode> flaggedNodes)
	{
		if ( flaggedNodes != null )
		{	
			foreach ( FlaggedScenarioNode flaggedNode in flaggedNodes )
			{
				if ( flaggedNode.Flag == null || flagManager.IsFlagActive( flaggedNode.Flag ) )
				{
					return FindScenarioNodeByName( flaggedNode.NodeName );
				}
			}
		}

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
