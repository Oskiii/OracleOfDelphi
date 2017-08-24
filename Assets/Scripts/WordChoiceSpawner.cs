using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Improx.WordMaps;
using UnityEngine.Events;
using UnityEngine.UI;

public class WordChoiceSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform _wordChoiceParent;
    [SerializeField]
    private GameObject _wordChoicePrefab;

    private readonly List<WordChoice> _choiceObjects = new List<WordChoice>();

    public void SpawnWords(List<WordMapNode> nodes, UnityAction<WordMapNode> onChoose)
    {
        // Clear previous choices
        for (var i = _choiceObjects.Count-1; i >= 0; i--)
        {
            WordChoice choiceObject = _choiceObjects[i];
            Destroy(choiceObject.gameObject);
            _choiceObjects.Remove(choiceObject);
        }

        // For each node, spawn a choice object and fill in its info
        foreach (WordMapNode wordMapNode in nodes)
        {
            var obj = Instantiate(_wordChoicePrefab).GetComponent<WordChoice>();
            _choiceObjects.Add(obj);
            obj.transform.SetParent(_wordChoiceParent);

            WordMapNode node = wordMapNode;
            obj.GetComponent<Button>().onClick.AddListener(() => onChoose(node));

            obj.SetNode(wordMapNode);
        }
    }
}
