using System;
using System.Collections;
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
        Node previousNode;  // Used to keep track of "i-1" inserted node.
        int leafCounter = 1;
        Dictionary<int, Node> bwtIndex;

        /// <summary>
        /// Constructor taking the string as input and assigns the root node with it's pointers.
        /// </summary>
        /// <param name="input"></param>
        public SuffixTree(string input, char[] alphabet)
        {
            originalString = input;
            _alphabet = alphabet;
            root = new Node(alphabet);
            root.suffixLink = root;
            previousNode = null;
            bwtIndex = new Dictionary<int, Node>();
        }

        /// <summary>
        // Method that builds suffix tree from a given node given an input
        /// </summary>
        public void findPath(Node n, string input)
        {
            Node nodePtr = n;
            int startIndex = 0;

            int length = originalString.Length - input.Length;

            // Check string[i]

            char c = input[0];

            if (nodePtr.pointers[c] == null)    // if the character doesn't exist
            {
                Node temp = new Node(_alphabet);
                temp.StringDepth = originalString.Length;
                temp.setEdgeLabels(length, originalString.Length);
                temp.parent = nodePtr;
                nodePtr.pointers[c] = temp;
                temp.nodeID = leafCounter;
                leafCounter++;
                previousNode = temp;
            }

            else //if character pointer does exist
            {
                Node temp = nodePtr.pointers[c];
                int counter = 0;
                for (int i = 0; i < input.Length - 1; i++)
                {
                    char ch = input[i];
                    string s = originalString.Substring(temp.edgeLabel[0], temp.edgeLabel[1] - temp.edgeLabel[0]);
                    if (ch == s[counter])
                    {
                        counter++;
                        if (s.Length <= counter)
                        {
                            if (temp.pointers[input[i+1]] == null)
                            {


                                Node newerNode = new Node(_alphabet);
                                newerNode.setEdgeLabels(length + counter, originalString.Length);
                                newerNode.StringDepth = originalString.Length;
                                newerNode.parent = temp;
                                temp.pointers[input[i+1]] = newerNode;

                                previousNode = newerNode;
                                newerNode.nodeID = leafCounter;
                                leafCounter++;
                                break;
                            }
                            else
                            {
                                temp = temp.pointers[input[i + 1]];
                                c = input[i + 1];
                                counter = 0;
                            }
                        }
                    }
                    else
                    {
                        Node newNode = new Node(_alphabet);
                        newNode.StringDepth = temp.edgeLabel[0] + counter;
                        newNode.parent = temp.parent;
                        newNode.setEdgeLabels(temp.edgeLabel[0], temp.edgeLabel[0] + counter);
                        newNode.nodeID = -1;

                        temp.parent.pointers[c] = newNode;
                        temp.parent = newNode;
                        temp.setEdgeLabels(newNode.edgeLabel[1], temp.edgeLabel[1]);
                        temp.StringDepth = temp.edgeLabel[1];


                        newNode.pointers[s[counter]] = temp;

                        Node newerNode = new Node(_alphabet);
                        newerNode.setEdgeLabels(length + counter, originalString.Length);
                        newerNode.StringDepth = originalString.Length;
                        newerNode.parent = newNode;
                        newNode.pointers[ch] = newerNode;

                        // Make suffix link right here
                        if (previousNode.parent != root)
                        {
                            previousNode.parent.suffixLink = newNode;
                        }

                        previousNode = newerNode;
                        newerNode.nodeID = leafCounter;
                        leafCounter++;
                        break;

                    }
                }


            }
        }

        /// <summary>
        /// Method to use McCreight's algorithm to build suffix tree
        /// </summary>
        public void buildTree()
        {
            for (int i = 0; i < originalString.Length; i++)
            {
                string subString = originalString.Substring(i);
                //findPath(root, subString);

                if (previousNode != null)   // if there is something in the tree
                {
                    if (root.pointers[subString[0]] == null)    // If we need to add from the root
                    {
                        findPath(root, subString);
                    }
                    // Case 1: previousNode.parent 'U' has a suffix link and is not the root
                    else if (previousNode.parent.suffixLink != null && root != previousNode.parent)
                    {
                        int alpha = previousNode.parent.edgeLabel[1] - previousNode.parent.edgeLabel[0] + 1;
                        string newString = subString.Substring(alpha);
                        findPath(previousNode.parent.suffixLink, newString);
                    }

                    // Case 2: previousNode.parent 'U' has a suffix link but IS the root
                    else if (previousNode.parent.suffixLink != null && root == previousNode.parent)
                    {
                        findPath(root, subString);
                    }

                    // Case 3: previousNode.parent 'U' has no suffix link, 'U'.parent is not the root
                    else if (previousNode.parent.suffixLink == null && root != previousNode.parent.parent)
                    {
                        Node U = previousNode.parent.parent;
                        Node hopper = U.suffixLink;

                        int betaLengh = previousNode.parent.edgeLabel[1] - previousNode.parent.edgeLabel[0];
                        string newString = subString.Substring(previousNode.parent.edgeLabel[0], betaLengh);
                        int betaStart = previousNode.parent.edgeLabel[0];
                        int hopLength = 0;

                        while (betaLengh != 0)  // Loop until we exhaust the length of Beta (newString)
                        {
                            char c = newString[0];
                            if (hopper.pointers[c] != null)
                            {
                                int lengthCheck = betaLengh - (hopper.pointers[c].edgeLabel[1] - hopper.pointers[c].edgeLabel[0]);

                                // Check if we would land in the middle of an edge
                                if (lengthCheck <= 0)
                                {
                                    findPath(hopper, subString.Substring(hopLength));
                                    betaLengh = 0;
                                }

                                else if (lengthCheck == 0)
                                {
                                    hopper = hopper.pointers[c];

                                    findPath(hopper, originalString.Substring(hopper.edgeLabel[1] + 1));
                                }

                                // Can get to next node
                                else
                                {
                                    hopper = hopper.pointers[c];
                                    betaStart = betaStart - (hopper.edgeLabel[1] - hopper.edgeLabel[0]);
                                    betaLengh = betaLengh - (hopper.edgeLabel[1] - hopper.edgeLabel[0]);
                                    hopLength = hopLength + (hopper.edgeLabel[1] - hopper.edgeLabel[0]);
                                }
                            }

                            else
                            {

                            }


                        }
                    }

                    // Case 4: Parent has no suffix link and U' is the root
                    else if (previousNode.parent.suffixLink == null && root == previousNode.parent.parent)
                    {
                        Node U = previousNode.parent.parent;
                        Node hopper = U.suffixLink; // hopper is the root

                        int betaLengh = previousNode.parent.edgeLabel[1] - previousNode.parent.edgeLabel[0] - 1;
                        string newString = originalString.Substring(previousNode.parent.edgeLabel[0] + 1, betaLengh);
                        int betaStart = previousNode.parent.edgeLabel[0] + 1;
                        int hopLength = 0;

                        if (betaLengh == 0)
                        {
                            findPath(hopper, subString);
                        }
                        else
                        {

                            do
                            {
                                char c = newString[0];
                                if (hopper.pointers[c] != null)
                                {
                                    int lengthCheck = betaLengh - (hopper.pointers[c].edgeLabel[1] - hopper.pointers[c].edgeLabel[0]);

                                    // Check if we would land in the middle of an edge
                                    if (lengthCheck <= 0)
                                    {
                                        findPath(hopper, subString.Substring(hopLength));
                                        betaLengh = 0;
                                    }

                                    else if (lengthCheck == 0)
                                    {
                                        hopper = hopper.pointers[c];

                                        findPath(hopper, originalString.Substring(hopper.edgeLabel[1] + 1));
                                    }

                                    // Can get to next node
                                    else
                                    {
                                        hopper = hopper.pointers[c];
                                        betaStart = betaStart - (hopper.edgeLabel[1] - hopper.edgeLabel[0]);
                                        betaLengh = betaLengh - (hopper.edgeLabel[1] - hopper.edgeLabel[0]);
                                        hopLength = hopLength + (hopper.edgeLabel[1] - hopper.edgeLabel[0]);
                                    }
                                }
                            } while (betaLengh != 0);
                        }
                    }
                }

                else
                {
                    findPath(root, subString);
                }
            }
        }

        /// <summary>
        /// Method that displays the children of a given node 'U'
        /// </summary>
        /// <param name="u"></param>
        public void displayChildren(Node u)
        {
            foreach (char c in _alphabet)
            {
                if (u.pointers[c] != null)
                {
                    Console.WriteLine(u.pointers[c].nodeID);
                }
            }
        }


        public void dfsTraversal(Node n)
        {
            Console.WriteLine(n.StringDepth);
            foreach (char c in _alphabet)
            {
                if (n.pointers[c] != null)
                {
                    dfsTraversal(n.pointers[c]);
                }
            }
        }

        public void printBWT(Node n)
        {
            if (n.nodeID > 0)
            {
                //Console.WriteLine(originalString[n.nodeID - 1]);
                Console.WriteLine(originalString[n.edgeLabel[0]]);
            }
            foreach (char c in _alphabet)
            {
                if (n.pointers[c] != null)
                {
                    printBWT(n.pointers[c]);
                }
            }
        }
        
    }
}
