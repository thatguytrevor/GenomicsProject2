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
        int[] edgeLabel;    // used to hold indices of string from parent to here
        int _stringDepth;     // index through the string
        public List<Node> pointers;    // Node pointers, one for each letter in our alphabet + $
        string label;   // label to be used for string on the node

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

        public string Label
        {
            get
            {
                return label;
            }

            set
            {
                label = value;
            }
        }

        public Node()
        {
            Label = null;
            edgeLabel = null;
            StringDepth = 0;
            pointers = new List<Node>();
        }

        /// <summary>
        /// Constructor accepting a character for the label -- shouldn't really be used
        /// </summary>
        /// <param name="l"></param>
        public Node(string l)
        {
            Label = l;
            edgeLabel[0] = 0;
            edgeLabel[1] = 0;
            StringDepth = 0;
            pointers = new List<Node>();
        }

        /// <summary>
        /// Constructor accepting a character for label and an alphabet to fill Node's pointers
        /// </summary>
        /// <param name="l"></param>
        /// <param name="alphabet"></param>
        public Node(string l, string[] alphabet) // Constructor accepting an alphabet for pointers
        {
            Label = l;
            edgeLabel[0] = 0;
            edgeLabel[1] = 0;
            StringDepth = 0;

            foreach (string s in alphabet)  // build the pointers
            {
                Node newNode = new Node(s);
                pointers.Add(newNode);
            }
            Node moneyNode = new Program2.Node("$");
            pointers.Add(moneyNode);
        }

        public void setEdgeLabels(int a, int b)
        {
            edgeLabel[0] = a;
            edgeLabel[1] = b;
        }

    }
}
