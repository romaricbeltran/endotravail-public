using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopupActionManager : BaseActionManager<PopupAction>
{
	// UI
	public GameObject popupCanvas;
	public TextMeshProUGUI mainText;
	public TextMeshProUGUI sourceText;
	public Button nextButton;

	private int currentPageIndex = 0;

	public override void LoadData(PopupAction currentAction)
	{
		currentPageIndex = 0;
		UpdatePageDisplay();
		nextButton.onClick.RemoveAllListeners();
		nextButton.onClick.AddListener( () => OnNextPage() );
	}

	public override void StartAction()
	{
		popupCanvas.SetActive( true );
	}

	public override void EndAction()
	{
		popupCanvas.SetActive( false );
		EventSystem.current.SetSelectedGameObject( null ); // Resets focus on the button
		base.EndAction();
	}

	private void UpdatePageDisplay()
	{
		if ( currentPageIndex < base.currentAction.Pages.Count )
		{
			Page currentPage = base.currentAction.Pages[currentPageIndex];

			mainText.text = currentPage.MainText;
			sourceText.text = currentPage.SourceText;
		}
	}

	private void OnNextPage()
	{
		currentPageIndex++;

		if ( currentPageIndex >= base.currentAction.Pages.Count )
		{
			EndAction();
		}
		else
		{
			UpdatePageDisplay();
		}
	}
}
