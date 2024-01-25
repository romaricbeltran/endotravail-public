using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class TextInputHelper : MonoBehaviour
{
    private Scrollbar scrollbar;
    private CanvasGroup scrollbarCanvasGroup;

    [DllImport("__Internal")]
    private static extern void MoveInputMobilePreviewToTop();
    
    private void Start()
    {
        scrollbar = GetComponentInChildren<Scrollbar>();

        if (scrollbar != null)
        {
            scrollbarCanvasGroup = scrollbar.GetComponent<CanvasGroup>();
        }
        else
        {
            Debug.LogError("Le composant Scrollbar n'a pas été trouvé sur le GameObject ou ses enfants.");
        }
    }

    public void HandleSelect()
    {
        MoveInputMobilePreviewToTop();
    }

    public void HandleValueChanged()
    {
        if (scrollbar != null && scrollbarCanvasGroup != null)
        {
            scrollbarCanvasGroup.alpha = scrollbar.size < 1f ? 1f : 0f;
        }
    }
}
