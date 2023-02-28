using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mission
{
    public int nodeCode;
    public int nextNodeCode;
    [TextArea] [SerializeField] private string mainText;
    [SerializeField] private bool isAccomplished;

    public string GetMainText()
    {
        return mainText;
    }

    public bool IsAccomplished()
    {
        return isAccomplished;
    }
}
