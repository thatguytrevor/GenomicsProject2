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
        public void buildTree()
        {
            Node nodePtr = root;
            int startIndex = 0;
            for(int i=0; i < originalString.Length; i++)    // Loop through characters in the string
            {
                // Check string[i]
                string subString = originalString.Substring(startIndex, originalString.Length - startIndex);

                // compare each character in the substring we are inserting
                foreach (char c in subString)
                {
                    // see if our nodePtr's pointer with label "c" is null. If it is, create it.
                    // If not null, move to it
                }
            }
        }
    }
}
