using System.Collections.Generic;
using Improx.WordMaps;
using UnityEngine;

public class OracleController : MonoBehaviour
{
    private readonly List<string> _trainingPhrases = new List<string>
    {
        "Your soul shall suffer!",
        "The devil shall claim your soul.",
        "Your people will suffer.",
        "You will lose your kingdom.",
        "Hell shall rain upon you from the skies.",
        "Misery is all you will ever earn.",
        "Love of money and nothing else will ruin you.",
        "You should found a city in Libya.",
        "Yes.",
        "No.",
        "First sacrifice to the warriors who once had their home in this island.",
        "You have many allies in your city.",
        "Flee.",
        "Get out of my sanctum and drown your spirits in woe.",
        "Await not in quiet the coming of the horses.",
        "Slip away.",
        "Turn your back.",
        "The strength of bulls or lions cannot stop the foe.",
        "Your great town must be sacked by Perseus' sons.",
        "Your whole land shall mourn the death of a king.",
        "Pray to the winds.",
        "With silver spears you may conquer the world.",
        "You are invincible.",
        "Make your own nature your guide in life."
    };

    private WordMapNode _currentNode;

    private WordBank _wordBank;
    [SerializeField] private WordChoiceSpawner _wordChoiceSpawner;
    [SerializeField] private ProphecyDisplay _prophecyDisplay;

    private void Start()
    {
        _wordBank = new WordBank(_trainingPhrases);
        NextWord();
    }

    /// <summary>
    ///     Get next word.
    /// </summary>
    public void NextWord()
    {
        List<WordMapNode> foundNodes = null;

        if (_currentNode == null)
        {
            WordMapNode newNode = _wordBank.StartingNodes[Random.Range(0, _wordBank.StartingNodes.Count - 1)];
            SetCurrentNodeAndGetNextChoices(newNode);
        }
        else
        {
            foundNodes = _currentNode.GetWeightedRandomChildren(3);

            if (foundNodes.Count > 0)
            {
                _wordChoiceSpawner.SpawnWords(foundNodes, SetCurrentNodeAndGetNextChoices);
            }
            else
            {
                print("End of prophecy!");
                _currentNode = null;
                _prophecyDisplay.ClearProphecy();
                NextWord();
            }
        }

        if (foundNodes == null) return;
        foreach (WordMapNode foundNode in foundNodes)
        {
            print($"Choice: {foundNode.Word} id: {foundNode.NodeId}");
        }
    }

    private void SetCurrentNodeAndGetNextChoices(WordMapNode newNode)
    {
        _currentNode = newNode;
        _prophecyDisplay.AddToProphecy(newNode.Word);
        NextWord();
    }
}