using System.Collections.Generic;
using System.Linq;
using Improx.Utility;
using UnityEngine;

namespace Improx.WordMaps
{
    public class WordMapNode
    {
        public ObservableList<WordMapNode> Next;

        public int NodeId;
        private int _occurrences;
        public int Occurrences
        {
            get { return _occurrences; }
            set
            {
                _occurrences = value;

                if (Previous == null)
                {
                    return;
                }

                foreach (WordMapNode node in Previous)
                {
                    node.CalculateWeightOfChildren();
                }
            }
        }
        public ObservableList<WordMapNode> Previous;
        public int TotalCumulativeWeightOfChildren; // Total sum of children's occurrences
        public string Word;

        public WordMapNode(string word, int nodeId)
        {
            NodeId = nodeId;
            Word = word;
            Occurrences = 1;
            TotalCumulativeWeightOfChildren = 0;
            Previous = new ObservableList<WordMapNode>();
            Next = new ObservableList<WordMapNode>();

            Next.OnUpdated += CalculateWeightOfChildren;

        }

        /// <summary>
        /// Sums weights of all children and saves it for later use
        /// </summary>
        private void CalculateWeightOfChildren()
        {
            TotalCumulativeWeightOfChildren = CalculateWeightOfNodes(Next);
        }

        /// <summary>
        /// Sums weights of nodes.
        /// </summary>
        /// <param name="nodes">Nodes whose occurrences to sum</param>
        /// <returns>Total weight</returns>
        private int CalculateWeightOfNodes(ObservableList<WordMapNode> nodes)
        {
            var sum = 0;

            foreach (WordMapNode node in nodes)
                sum += node.Occurrences;

            return sum;
        }

        /// <summary>
        /// Finds child named childWord. If not found, returns null.
        /// </summary>
        /// <param name="childWord">Word to find</param>
        /// <returns>Found node</returns>
        public WordMapNode FindChild(string childWord)
        {
            WordMapNode foundNode = Next.Find(x => x.Word == childWord);
            return foundNode;
        }

        /// <summary>
        /// Get random child node.
        /// </summary>
        /// <returns>Random child node</returns>
        public WordMapNode GetWeightedRandomChild()
        {
            WordMapNode foundNode = null;

            int rand = Random.Range(0, TotalCumulativeWeightOfChildren-1);
            ObservableList<WordMapNode> availableNodes = Next;

            availableNodes.Sort((x, y) => x.Occurrences - y.Occurrences);

            var cumulativeWeight = 0;
            foreach (WordMapNode availableNode in availableNodes)
            {
                cumulativeWeight += availableNode.Occurrences;

                if (cumulativeWeight <= rand) continue;

                foundNode = availableNode;
                break;
            }

            return foundNode;
        }

        public List<WordMapNode> GetWeightedRandomChildren(int howMany)
        {
            var foundNodes = new List<WordMapNode>();

            // Copy elements to new list
            ObservableList<WordMapNode> availableNodes = new ObservableList<WordMapNode>(Next);

            availableNodes.Sort((x, y) => x.Occurrences - y.Occurrences);

            // Limit amount to get to availableNodes count
            howMany = Mathf.Min(howMany, availableNodes.Count);

            for (var i = 0; i < howMany; i++)
            {
                var cumulativeWeight = 0;
                var totalCumulativeWeightOfAvailableNodes = CalculateWeightOfNodes(availableNodes);

                int rand = Random.Range(0, totalCumulativeWeightOfAvailableNodes - 1);

                for (int j = availableNodes.Count - 1; j >= 0; j--)
                {
                    cumulativeWeight += availableNodes[j].Occurrences;

                    if (cumulativeWeight <= rand) continue;

                    foundNodes.Add(availableNodes[j]);
                    availableNodes.RemoveAt(j);
                    break;
                }
            }

            Debug.Assert(foundNodes.Distinct().Count() == foundNodes.Count());
            Debug.Assert(foundNodes.Count == Mathf.Min(howMany, Next.Count), $"{Word} ({availableNodes.Count}): {foundNodes.Count} != {Mathf.Min(howMany, Next.Count)}");
            return foundNodes;
        }

        public WordMapNode GetRandomChild()
        {
            WordMapNode foundNode = null;

            if (Next.Count > 0)
            {
                foundNode = Next[Random.Range(0, Next.Count - 1)];
            }

            return foundNode;
        }
    }
}