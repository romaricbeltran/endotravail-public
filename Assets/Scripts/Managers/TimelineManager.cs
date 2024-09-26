using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
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
    public LevelLoader levelLoader;
    public GameManager gameManager;
	public BadgeManager badgeManager;
	public DialogueScenarioNodeManager dialogueScenarioNodeManager;
	public PopupScenarioNodeManager popupScenarioNodeManager;
	public EndGameScenarioNodeManager endGameScenarioNodeManager;
	public int chapter;
    public BaseScenarioNode[] scenario;

	private int currentNodeIndex = 0;
	private IScenarioNodeManager previousManager;
	private bool endGame = false;

	private PlayableDirector director;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }

	public void Play()
	{
		if ( scenario != null && scenario.Length > 0 )
		{
			PlayScenarioNode( scenario[currentNodeIndex] );
		}
		else
		{
			Debug.LogWarning( "Scenario array is empty!" );
		}
	}

	public void PlayScenarioNode(BaseScenarioNode scenarioNode)
    {
		//if (MissionScenarioNode.ON_MISSION_END)
		//{
		//	if (scenarioNode is ActionScenarioNode actionScenarioNode)
		//	{
		//		MissionScenarioNode.WAS_ACTION_MISSION_COMPONENT = true;
		//	}

		//	MissionScenarioNode.EndMission();
		//}

		if ( scenarioNode.TimelineClip )
		{
			director.Play( scenarioNode.TimelineClip, scenarioNode.DirectorWrapMode );
		}

		if ( scenarioNode is DialogueScenarioNode dialogueScenarioNode )
		{
			gameManager.SwitchPlayerInput( false );
			HandleScenario( dialogueScenarioNodeManager, dialogueScenarioNode );
		}
		else if ( scenarioNode is PopupScenarioNode popupScenarioNode )
		{
			gameManager.SwitchPlayerInput( false );
			HandleScenario( popupScenarioNodeManager, popupScenarioNode );
		}
		else if ( scenarioNode is EndGameScenarioNode endGameScenarioNode )
		{
			endGame = true;
			gameManager.SwitchPlayerInput( false );
			HandleScenario( endGameScenarioNodeManager, endGameScenarioNode );
		}
		else if ( scenarioNode is MenuScenarioNode menuScenarioNode )
		{
			HandleEndChapter();
		}
		else
		{
			HandleEndChapter();
			Debug.LogWarning( "Unsupported scenario node type." );
		}

		HandleBadge( scenarioNode );
		HandleAnalytics( scenarioNode );
	}

	private void HandleScenario(IScenarioNodeManager manager, BaseScenarioNode scenarioNode)
	{
		if ( previousManager != null )
		{
			previousManager.OnNodeCompleted -= OnNodeCompletedHandler;
		}

		manager.OnNodeCompleted -= OnNodeCompletedHandler;
		manager.OnNodeCompleted += OnNodeCompletedHandler;

		previousManager = manager;

		manager.LoadData( scenarioNode );
	}

	private void HandleBadge(BaseScenarioNode scenarioNode)
	{
		if ( scenarioNode.Badge )
		{
			badgeManager.LoadData( scenarioNode.Badge );
			badgeManager.StartNode();
		}
	}

	private void OnNodeCompletedHandler()
	{
		badgeManager.EndNode();
		PlayNextNode();
	}

	private void PlayNextNode()
	{
		currentNodeIndex++;

		BaseScenarioNode returnedNode = previousManager?.GetNextNode();

		if ( endGame )
		{
			HandleEndGame();
		}
		else if( returnedNode != null )
		{
			//@Todo assigner à currentNodeIndex l'index du nouveau noeud
			PlayScenarioNode( returnedNode );
		}
		else if ( currentNodeIndex + 1 < scenario.Length )
		{
			PlayScenarioNode( scenario[currentNodeIndex++] );
		}
		else
		{
			HandleEndChapter();
			Debug.Log( "End of chapter scenario reached." );
		}
	}

	private void HandleEndChapter()
	{
		if ( chapter > LevelLoader.LoadProgress() )
		{
			LevelLoader.SaveProgress( chapter );
		}

		levelLoader.LoadLevel( 1 ); // Charge le menu
	}

	private void HandleEndGame()
	{
		if ( chapter > LevelLoader.LoadProgress() )
		{
			LevelLoader.SaveProgress( chapter );
		}

		levelLoader.LoadLevel( 6 ); // Charge le formulaire
	}

	private void HandleAnalytics(BaseScenarioNode scenarioNode)
	{
		Debug.Log( "Playing Node :" + scenarioNode.ScenarioNodeName );

		//AnalyticsService.Instance.CustomData("gameProgress",
		//	new Dictionary<string, object>{{ "gameProgressStepName", scenarioNode.ScenarioNodeName } });
	}
}
