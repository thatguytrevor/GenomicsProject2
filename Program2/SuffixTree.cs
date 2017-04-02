using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program2
{
    public class SuffixTree
    {
        // Member Variables
        public Node root;
        string originalString;
        char[] _alphabet;

        /// <summary>
        /// Constructor taking the string as input and assigns the root node with it's pointers.
        /// </summary>
        /// <param name="input"></param>
        public SuffixTree(string input, char[] alphabet)
        {
            originalString = input;
            _alphabet = alphabet;
            root = new Node(alphabet);
        }

        /// <summary>
        // Method that builds suffix tree based on alphabet / Quadratic Time. 
        // TODO: Change this to 'FindPath' method, create linear algorithm.
        /// </summary>
        public void buildTree(Node n)
        {
            Node nodePtr = n;
            int startIndex = 0;
            for (int i = 0; i < originalString.Length; i++)    // Loop through characters in the string
            {
                // Check string[i]
                string subString = originalString.Substring(i);
                char c = subString[0];

                if (nodePtr.pointers[c] == null)    // if the character doesn't exist
                {
                    Node temp = new Node(_alphabet);
                    temp.StringDepth = nodePtr.StringDepth + 1;
                    temp.setEdgeLabels(i, originalString.Length);
                    temp.parent = nodePtr;
                    nodePtr.pointers[c] = temp;
                }

                else //if character pointer does exist
                {
                    Node temp = nodePtr.pointers[c];
                    int counter = 0;
                    foreach (char ch in subString)
                    {
                        string s = originalString.Substring(temp.edgeLabel[0], temp.edgeLabel[1] - temp.edgeLabel[0]);
                        if (ch == s[counter])
                        {
                            counter++;
                        }
                        else
                        {
                            Node newNode = new Node(_alphabet);
                            newNode.StringDepth = temp.StringDepth;
                            newNode.parent = temp.parent;
                            newNode.setEdgeLabels(temp.edgeLabel[0], temp.edgeLabel[0] + counter);

                            temp.parent = newNode;
                            temp.setEdgeLabels(newNode.edgeLabel[1], temp.edgeLabel[1]);
                            temp.StringDepth++;

                            newNode.pointers[s[counter]] = temp;

                            Node newerNode = new Node(_alphabet);
                            newerNode.setEdgeLabels(i + counter, originalString.Length - 1);
                            newerNode.StringDepth = newNode.StringDepth + 1;
                            newerNode.parent = newNode;
                            newNode.pointers[ch] = newerNode;


                        }
                    }
                }

            }
        }
    }
}
