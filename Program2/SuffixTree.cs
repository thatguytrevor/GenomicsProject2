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
        public int leafCounter = 1;
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

            int length = originalString.Length - input.Length;  // length that we are into the string from the suffix

            // Check string[i]

            char c = input[0];  // get the first character of the input

            if (nodePtr.pointers[c] == null)    // if the character doesn't exist
            {
                // Create new node and assign to node pointer's character pointer
                Node temp = new Node(_alphabet);
                temp.StringDepth = originalString.Length;
                temp.setEdgeLabels(length, originalString.Length);
                temp.parent = nodePtr;
                temp.nodeID = leafCounter;
                nodePtr.pointers[c] = temp;
                leafCounter++;
                previousNode = temp;
            }

            else //if character pointer does exist
            {
                Node temp = nodePtr.pointers[c];    // Move to that character pointer
                int counter = 0;

                for (int i = 0; i < input.Length; i++)
                {
                    char ch = input[i]; // Get the first character

                    // String 's' = the substring that is on the edge label of our current node
                    string s = originalString.Substring(temp.edgeLabel[0], temp.edgeLabel[1] - temp.edgeLabel[0]);


                    if (ch == s[counter])   // Character match
                    {
                        counter++;          // Increment counter so we move through the path label

                        if (ch == '$')
                        {
                            break;
                        }
                        if (s.Length <= counter)    // If we are out of edge label and need to move to a new node
                        {
                            if (temp.pointers[input[i + 1]] == null)      // If there is no node for us to move to
                            {

                                // Create new leaf node, set properties
                                Node newLeaf = new Node(_alphabet);
                                newLeaf.setEdgeLabels(length + counter, originalString.Length);
                                newLeaf.StringDepth = originalString.Length;
                                newLeaf.parent = temp;
                                temp.pointers[input[i + 1]] = newLeaf;


                                previousNode = newLeaf; // set previous node since it was just added
                                newLeaf.nodeID = leafCounter;
                                leafCounter++;
                                break;
                            }
                            else
                            {
                                // There is a node that we can move to, so go there.
                                temp = temp.pointers[input[i + 1]];

                                // Change our first character since we moved
                                c = input[i + 1];
                                counter = 0;
                            }
                        }
                    }

                    // Character mismatch, create new leaf node and internal node, adjust path labels
                    else
                    {
                        Node newInternal = new Node(_alphabet);
                        newInternal.StringDepth = temp.edgeLabel[0] + counter;
                        newInternal.parent = temp.parent;
                        newInternal.setEdgeLabels(temp.edgeLabel[0], temp.edgeLabel[0] + counter);
                        newInternal.nodeID = -1;

                        temp.parent.pointers[c] = newInternal;
                        temp.parent = newInternal;
                        temp.setEdgeLabels(newInternal.edgeLabel[1], temp.edgeLabel[1]);
                        temp.StringDepth = temp.edgeLabel[1];


                        newInternal.pointers[s[counter]] = temp;

                        Node newLeafNode = new Node(_alphabet);
                        newLeafNode.setEdgeLabels(length + counter, originalString.Length);
                        newLeafNode.StringDepth = originalString.Length;
                        newLeafNode.parent = newInternal;
                        newInternal.pointers[ch] = newLeafNode;



                        previousNode = newLeafNode;
                        newLeafNode.nodeID = leafCounter;
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
                if (i == 28)
                {
                    Console.WriteLine();
                }
                string subString = originalString.Substring(i);
                Console.WriteLine("Working:" + i);
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
                        int alpha = previousNode.parent.edgeLabel[1] - previousNode.parent.edgeLabel[0] - 1;
                        string newString = subString.Substring(alpha);
                        findPath(previousNode.parent.suffixLink, newString);
                    }

                    // Case 2: previousNode.parent 'U' has a suffix link but IS the root
                    else if (previousNode.parent.suffixLink != null && root == previousNode.parent)
                    {
                        findPath(root, subString);
                    }

                    // Case 4: Parent has no suffix link and U' is the root
                    else if (previousNode.parent.suffixLink == null)
                    {
                        Node U = previousNode.parent.parent;
                        Node hopper = U.suffixLink; // hopper is the root

                        subString = subString.Substring(hopper.edgeLabel[1]);

                        int betaLength;
                        string betaString;
                        int betaStart;
                        int hopLength;


                        if (root != previousNode.parent.parent) //if root does not equal u beta is full string
                        {
                            betaLength = previousNode.parent.edgeLabel[1] - previousNode.parent.edgeLabel[0];
                            betaString = originalString.Substring(previousNode.parent.edgeLabel[0], betaLength);
                            betaStart = previousNode.parent.edgeLabel[0];
                            hopLength = 0;
                        }
                        else //root does equal u, full string = x+beta, use only beta
                        {
                            betaLength = previousNode.parent.edgeLabel[1] - previousNode.parent.edgeLabel[0] - 1;
                            betaString = originalString.Substring(previousNode.parent.edgeLabel[0] + 1, betaLength);
                            betaStart = previousNode.parent.edgeLabel[0] + 1;
                            hopLength = 0;
                        }

                        if (betaLength == 0)
                        {
                            previousNode.parent.suffixLink = hopper;    // set suffix link
                            findPath(hopper, subString);
                            
                        }
                        else
                        {

                            do
                            {
                                char c = betaString[hopLength];
                                if (hopper.pointers[c] != null)
                                {
                                    int lengthCheck = betaLength - (hopper.pointers[c].edgeLabel[1] - hopper.pointers[c].edgeLabel[0]);

                                    // Check if we would land in the middle of an edge
                                    if (lengthCheck < 0)
                                    {
                                        int counter = 0;
                                        hopper = hopper.pointers[c];
                                        string path = originalString.Substring(hopper.edgeLabel[0], hopper.edgeLabel[1] - hopper.edgeLabel[0]);
                                        for (int j = 0; j < subString.Length; j++)
                                        {

                                            if (path.Length <= j)    // If we are out of edge label and need to move to a new node
                                            {
                                                if (hopper.pointers[subString[j]] == null)      // If there is no node for us to move to
                                                {

                                                    // Create new leaf node, set properties
                                                    Node newLeaf = new Node(_alphabet);
                                                    newLeaf.setEdgeLabels(originalString.Length - subString.Length + counter, originalString.Length);
                                                    newLeaf.StringDepth = originalString.Length;
                                                    newLeaf.parent = hopper;
                                                    hopper.pointers[subString[j]] = newLeaf;
                                                    previousNode.parent.suffixLink = hopper;

                                                    previousNode = newLeaf; // set previous node since it was just added
                                                    newLeaf.nodeID = leafCounter;
                                                    leafCounter++;
                                                    break;
                                                }
                                                else
                                                {
                                                    // There is a node that we can move to, so go there.
                                                    hopper = hopper.pointers[subString[j]];

                                                    // Change our first character since we moved
                                                    c = subString[j];
                                                    counter = 0;
                                                }
                                            }

                                            else if (subString[j] == path[j])
                                            {
                                                counter++;
                                                continue;
                                            }

                                            else
                                            {
                                                // create new internal node
                                                Node newInternal = new Node(_alphabet);
                                                newInternal.StringDepth = hopper.edgeLabel[0] + j;
                                                newInternal.parent = hopper.parent;
                                                newInternal.setEdgeLabels(hopper.edgeLabel[0], hopper.edgeLabel[0] + j);
                                                newInternal.nodeID = -1;

                                                // create suffix link
                                                previousNode.parent.suffixLink = newInternal;

                                                hopper.parent.pointers[path[0]] = newInternal;
                                                hopper.parent = newInternal;
                                                newInternal.pointers[path[j]] = hopper;
                                                hopper.setEdgeLabels(newInternal.edgeLabel[1], hopper.edgeLabel[1]);
                                                hopper.StringDepth = hopper.edgeLabel[1];

                                                Node newLeafNode = new Node(_alphabet);
                                                newLeafNode.setEdgeLabels(originalString.Length - subString.Length + j, originalString.Length);
                                                newLeafNode.StringDepth = originalString.Length;
                                                newLeafNode.parent = newInternal;
                                                newInternal.pointers[subString[j]] = newLeafNode;

                                                previousNode = newLeafNode;
                                                newLeafNode.nodeID = leafCounter;
                                                leafCounter++;
                                                break;
                                            }
                                        }
                                        //findPath(hopper, subString.Substring(hopLength));
                                        betaLength = 0;
                                    }

                                    else if (lengthCheck == 0)
                                    {
                                        hopper = hopper.pointers[c];
                                        hopLength = hopLength + (hopper.edgeLabel[1] - hopper.edgeLabel[0]);
                                        previousNode.parent.suffixLink = hopper;
                                        findPath(hopper, subString.Substring(hopLength));
                                        break;
                                    }

                                    // Can get to next node
                                    else
                                    {
                                        hopper = hopper.pointers[c];
                                        betaStart = betaStart - (hopper.edgeLabel[1] - hopper.edgeLabel[0]);
                                        betaLength = betaLength - (hopper.edgeLabel[1] - hopper.edgeLabel[0]);
                                        hopLength = hopLength + (hopper.edgeLabel[1] - hopper.edgeLabel[0]);
                                        subString = subString.Substring(hopLength);
                                        //newString = newString.Substring(betaStart);
                                    }
                                }
                                else
                                {
                                    previousNode.parent.suffixLink = hopper;
                                    findPath(hopper, subString.Substring(hopLength));
                                    break;
                                }
                            } while (betaLength != 0);
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
                int index = n.nodeID - 1;
                //Console.WriteLine("Node ID: " + n.nodeID);
                if (index - 1 < 0)
                {
                    index = originalString.Length;
                }
                Console.WriteLine(originalString[index - 1]);
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
