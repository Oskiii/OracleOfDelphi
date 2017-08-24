using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldspaceText : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _textObject;
    [SerializeField] private Object _owner;

    public void SetText(string newText)
    {
        _textObject.text = newText;
    }

    public void SetOwner(Object newOwner)
    {
        _owner = newOwner;
    }
}
