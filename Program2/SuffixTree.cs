using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program2
{
    class SuffixTree
    {
        // Member Variables
        Node root;
        string originalString;
        string[] alphabet;

        /// <summary>
        /// Constructor taking the string as input and assigns the root node with it's pointers.
        /// </summary>
        /// <param name="input"></param>
        SuffixTree(string input, string[] alphabet)
        {
            originalString = input;
            root = new Node(originalString[0].ToString(), alphabet);
        }

        /// <summary>
        // Method that builds suffix tree based on alphabet / Quadratic Time. 
        // TODO: Change this to 'FindPath' method, create linear algorithm.
        /// </summary>
        static void buildTree()
        {

        }
    }
}
