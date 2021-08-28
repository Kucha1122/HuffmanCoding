using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace huffman
{
    public class Huffman
    {
        public Dictionary<char, float> numbers = new Dictionary<char, float>();
        public Dictionary<char, float> prob = new Dictionary<char, float>();
        public Dictionary<char, int> sortedProb = new Dictionary<char, int>();
        public Dictionary<char, string> codes = new Dictionary<char, string>();
        public Dictionary<char, int> bits = new Dictionary<char, int>();
        public List<Node> tree = new List<Node>();
        public List<Node> secondTree = new List<Node>();
        public List<double> PartEntropyList = new List<Double>();
        public List<double> PartAvgWorldValueList = new List<Double>();
        public double Entropy { get; set; }
        public double AvgWordValue { get; set; }
        public Dictionary<char, float> getNumbers(string input)
        {
            var x = 0;
            var len = input.Length;

            while(x < len)
            {
                char chr = input[x];
                if(numbers.ContainsKey(chr))
                {
                    numbers[chr] = numbers[chr]+1;
                }
                else
                {
                    numbers.Add(chr, 1);
                }
                x++;
            }

            return numbers;
        }
        public Dictionary<char, float> getProbabilities(string input)
        {
            int len = input.Length;
            numbers = getNumbers(input);

            foreach(var elem in numbers)
            {
                prob[elem.Key] = elem.Value / len;
            }

            return prob;
        }

        public Dictionary<char, string> getCodes()
        {

            var sortedProb = from entry in prob orderby entry.Value ascending select entry;
            foreach (var elem in sortedProb)
            {
                tree.Add(new Node { Prob = elem.Value, Value = elem.Key});
            }

            while(tree.Count + secondTree.Count > 1)
            {
                Node left = getNext();
                Node right = getNext();
                Node newNode = new Node();
                newNode.Left = left;
                newNode.Right = right;
                newNode.Prob = left.Prob + right.Prob;
                newNode.Left.Parent = newNode;
                newNode.Right.Parent = newNode;
                secondTree.Add(newNode);
            }

            Node currentNode = secondTree[0];
            string code = " ";
            while (!(currentNode is null))
            {
                if (currentNode.Value != default(char))
                {
                    codes[currentNode.Value] = code;
                    code = code.Substring(0, code.Length-1);
                    currentNode.Visited = true;
                    currentNode = currentNode.Parent;
                }else if (!currentNode.Left.Visited)
                {
                    currentNode = currentNode.Left;
                    code += "0";
                }else if (!currentNode.Right.Visited)
                {
                    currentNode = currentNode.Right;
                    code += "1";
                }
                else
                {
                    currentNode.Visited = true;
                    currentNode = currentNode.Parent;
                    code = code.Substring(0, code.Length-1);
                }
            }

            return codes;
        }

        public List<EncodedText> compressHuffman(string input)
        {
            EncodedText encodedText;

            List <EncodedText> encodedTextsList = new List<EncodedText>();
            for(var i=0; i<input.Length; i++)
            {

                encodedText = new EncodedText();
                encodedText.Sign = input[i];
                encodedText.Code = codes[input[i]];
                encodedTextsList.Add(encodedText);
            }

            return encodedTextsList;

        }

        public double getEntropy()
        {
            foreach(var p in prob)
            {
                Entropy += p.Value * Math.Log(1 / p.Value,2);
                PartEntropyList.Add(Entropy);
            }

            return Math.Round(Entropy, 5);
        }

        public Dictionary<char, int> getBits()
        {
            foreach(var p in codes)
            {
                bits[p.Key] = p.Value.Length;
            }

            return bits;
        }
        public double getAvgWordValue()
        {
            foreach (var p in codes)
            {
                AvgWordValue += (prob[p.Key] * p.Value.Length);
                PartAvgWorldValueList.Add(AvgWordValue);
            }

            return Math.Round(AvgWordValue, 5)-1;
        }

        public Node getNext()
        {
            if(tree.Count > 0 && secondTree.Count > 0 && (tree[0].Prob < secondTree[0].Prob))
            {
                return shiftArray(tree);
            }

            if(tree.Count > 0 && secondTree.Count > 0 && (tree[0].Prob > secondTree[0].Prob))
            {
                return shiftArray(secondTree);
            }

            if (tree.Count > 0)
                return shiftArray(tree);

            return shiftArray(secondTree);
        }

        public List<Node> push(List<Node> _tree, Node elem)
        {
            var tempList = _tree.ToList();
            tempList.Add(elem);
            return tempList;
        }


        public Node shiftArray(List<Node> _array)
        {
            var shifted = _array[_array.Count - 1];
            var t = _array.First();
            _array.RemoveAt(0);
            Console.WriteLine("shifted: " + t.Value);
            return t;
        }
    }
}
