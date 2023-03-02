using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mission : EventNode
{
    [TextArea] [SerializeField] private string mainText;
    [SerializeField] private GameObject triggerObject;
    [SerializeField] private bool isAccomplished;

    public string GetMainText()
    {
        return mainText;
    }

    public GameObject GetTriggerObject()
    {
        return triggerObject;
    }

    public bool IsAccomplished()
    {
        return isAccomplished;
    }
}
