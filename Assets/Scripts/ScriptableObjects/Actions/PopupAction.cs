using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PopupAction", menuName = "ScriptableObjects/Actions/PopupAction" )]
public class PopupAction : BaseAction
{
	[SerializeField] private bool isIntroOrOutro;
	[SerializeField] private List<Page> pages;

	public bool IsIntroOrOutro { get => isIntroOrOutro; set => isIntroOrOutro = value; }
    public List<Page> Pages { get => pages; set => pages = value; }
}

[System.Serializable]
public class Page
{
	[SerializeField][TextArea] private string mainText;
	[SerializeField][TextArea] private string sourceText;

    public global::System.String MainText { get => mainText; set => mainText = value; }
    public global::System.String SourceText { get => sourceText; set => sourceText = value; }
}
