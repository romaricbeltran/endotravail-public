using TMPro;
using Unity.VisualScripting;
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

	public Animator popupBoxAnimator;

	private int currentPageIndex = 0;

	public override void LoadData(PopupAction currentAction)
	{
		currentPageIndex = 0;
		UpdatePageDisplay();
		nextButton.onClick.RemoveAllListeners();
		nextButton.onClick.AddListener( () => OnNextPage() );

		popupBoxAnimator.SetBool( "IsIntroOrOutro", currentAction.IsIntroOrOutro );
		nextButton.GetComponent<Animator>().SetBool("IsIntroOrOutro", currentAction.IsIntroOrOutro);
		nextButton.GetComponent<Animator>().SetTrigger( "Normal" );
	}

	public override void StartAction()
	{
		popupCanvas.GetComponent<CanvasGroup>().alpha = 1f;
		popupCanvas.GetComponent<CanvasGroup>().interactable = true;
		popupCanvas.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	public override void EndAction()
	{
		popupCanvas.GetComponent<CanvasGroup>().alpha = 0f;
		popupCanvas.GetComponent<CanvasGroup>().interactable = false;
		popupCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
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

		EventSystem.current.SetSelectedGameObject( null );
	}
}
