using System.Collections.Generic;
using Improx.Utility;
using UnityEngine;

namespace Improx.WordMaps
{
    public class WordBank
    {
        private WordMapNode _currentlyWorkingOn;
        private WordMapNode _previouslyWorkingOn;
        private int _nextNodeId;

        // Initialize
        public WordBank(IEnumerable<string> trainingPhrases)
        {
            StartingNodes = new List<WordMapNode>();
            AllNodes = new List<WordMapNode>();

            var trainingPhrasesSplit = new List<List<string>>();

            // Split phrases into words
            foreach (string t in trainingPhrases)
            {
                string[] words = t.ToLower().SplitAndKeepPunctuations();
                trainingPhrasesSplit.Add(new List<string>(words));
            }

            // Insert words into map
            foreach (List<string> phrase in trainingPhrasesSplit)
            {
                _previouslyWorkingOn = null;
                _currentlyWorkingOn = null;

                foreach (string word in phrase)
                {
                    InsertWord(word);
                }
            }
        }

        public List<WordMapNode> AllNodes { get; }
        public List<WordMapNode> StartingNodes { get; }

        /// <summary>
        ///     If word already exists anywhere in map, just increase its occurrence.
        ///     Otherwise inserts word into word map as the child of the node we're currently working on.
        /// </summary>
        /// <param name="word">Word to insert</param>
        private void InsertWord(string word)
        {
            _previouslyWorkingOn = _currentlyWorkingOn;

            // If map doesn't already contain word, add it
            if (AllNodes.Exists(x => x.Word == word) == false)
            {
                _currentlyWorkingOn = new WordMapNode(word, _nextNodeId);
                _nextNodeId++;

                if (_previouslyWorkingOn != null)
                {
                    //Debug.Log($"Inserted {_currentlyWorkingOn.Word}");
                    _previouslyWorkingOn.Next.Add(_currentlyWorkingOn);
                    _currentlyWorkingOn.Previous.Add(_previouslyWorkingOn);
                }
                else
                {
                    StartingNodes.Add(_currentlyWorkingOn);
                }

                AllNodes.Add(_currentlyWorkingOn);
            }
            else
            {
                // If map already contains word, find it and increase its occurrence rate
                _currentlyWorkingOn = AllNodes.Find(x => x.Word == word);

                if (_currentlyWorkingOn == null) return;

                _currentlyWorkingOn.Occurrences++;

                if (_previouslyWorkingOn == null) return;

                //Debug.Assert(_previouslyWorkingOn.Next.Contains(_currentlyWorkingOn) == false, $"{_previouslyWorkingOn.Word} already had {_currentlyWorkingOn.Word} as NEXT");
                //Debug.Assert(_currentlyWorkingOn.Previous.Contains(_previouslyWorkingOn) == false, $"{_currentlyWorkingOn.Word} already had {_previouslyWorkingOn.Word} as PREVIOUS");

                if (_previouslyWorkingOn.Next.Contains(_currentlyWorkingOn) == false)
                {
                    _previouslyWorkingOn.Next.Add(_currentlyWorkingOn);
                }

                if (_currentlyWorkingOn.Previous.Contains(_previouslyWorkingOn) == false)
                {
                    _currentlyWorkingOn.Previous.Add(_previouslyWorkingOn);
                }
            }
        }

        /// <summary>
        ///     Find node with word. Returns null if nothing found.
        /// </summary>
        /// <param name="word">Word to try and find</param>
        /// <returns>Found node</returns>
        public WordMapNode FindNode(string word)
        {
            return AllNodes.Find(x => x.Word == word);
        }

        /// <summary>
        ///     Get a number of random children from node with word. If can't find enough, return all children.'
        /// </summary>
        /// <param name="word">Word whose node to look at</param>
        /// <param name="howMany">How many nodes to look for</param>
        /// <returns></returns>
        public List<string> GetWordsFrom(string word, int howMany)
        {
            WordMapNode targetNode = FindNode(word);

            var foundWords = new List<string>();

            // If word isn't in map, return empty list
            if (targetNode == null)
            {
                Debug.LogWarning("Word doesn't exist in map.");
                return foundWords;
            }

            return GetWordsFrom(targetNode, howMany);
        }

        /// <summary>
        ///     Get a number of random children from node. If can't find enough, return all children.
        /// </summary>
        /// <param name="node">Node whose children to get</param>
        /// <param name="howMany">How many children to find?</param>
        /// <returns></returns>
        public List<string> GetWordsFrom(WordMapNode node, int howMany)
        {
            var foundWords = new List<string>();

            // If word isn't in map, return empty list
            if (node == null || node.Next.Count == 0)
            {
                Debug.LogWarning("Node doesn't exist in map or it has no next nodes");
                return foundWords;
            }

            // Copy list for shuffling
            List<WordMapNode> availableNodes = node.Next;
            availableNodes.Shuffle();

            // Pop howMany (or all) words into list
            howMany = Mathf.Min(howMany, node.Next.Count);
            for (var i = 0; i < howMany; i++)
            {
                foundWords.Add(availableNodes[0].Word);
                availableNodes.RemoveAt(0);
            }


            return foundWords;
        }
    }
}