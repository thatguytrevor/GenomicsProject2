using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program2
{
    public class Node
    {
        // Member Variables
        public int[] edgeLabel;    // used to hold indices of string from parent to here
        int _stringDepth;     // index through the string
        public Dictionary<char, Node> pointers;
        public Node parent;
        public Node child;

        public int StringDepth
        {
            get
            {
                return _stringDepth;
            }

            set
            {
                _stringDepth = value;
            }
        }
        

        public Node()
        {
            edgeLabel = new int[2];
            StringDepth = 0;
            pointers = new Dictionary<char, Node>();
            parent = null;
            child = null;
        }

        /// <summary>
        /// Constructor accepting a character for label and an alphabet to fill Node's pointers
        /// </summary>
        /// <param name="l"></param>
        /// <param name="alphabet"></param>
        public Node(char[] alphabet) // Constructor accepting an alphabet for pointers
        {
            //Label = l;
            edgeLabel = new int[2];
            StringDepth = 0;
            pointers = new Dictionary<char, Node>();

            foreach (char s in alphabet)  // build the pointers
            {
                pointers.Add(s, null);
            }
            pointers.Add('$', null);
        }

        public void setEdgeLabels(int a, int b)
        {
            edgeLabel[0] = a;
            edgeLabel[1] = b;
        }

    }
}
