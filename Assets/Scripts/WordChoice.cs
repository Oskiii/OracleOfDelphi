using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Improx.WordMaps;

public class WordChoice : MonoBehaviour
{

    [SerializeField] private Text _wordText;
    public WordMapNode choiceNode;

    public void SetNode(WordMapNode newNode)
    {
        _wordText.text = newNode.Word;
        choiceNode = newNode;
    }
}
