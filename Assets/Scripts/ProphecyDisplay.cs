using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProphecyDisplay : MonoBehaviour
{

    [SerializeField] private Text _prophecyText;

    public void AddToProphecy(string bitToAdd)
    {
        _prophecyText.text += " " + bitToAdd;
    }

    public void SetProphecy(string newProphecy)
    {
        _prophecyText.text = newProphecy;
    }

    public void ClearProphecy()
    {
        _prophecyText.text = string.Empty;
    }
}
