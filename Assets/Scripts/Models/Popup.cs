using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Popup
{
    public int nodeCode;
    public int nextNodeCode;
    [TextArea] [SerializeField] private string mainText;
    [TextArea] [SerializeField] private string sourceText;

    public string GetMainText()
    {
        return mainText;
    }

    public string GetSourceText()
    {
        return sourceText;
    }
}
