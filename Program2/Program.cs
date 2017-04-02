/*
* Trevor Larson & Josh Korn
* Genomics Project 2
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program2
{
    class Program
    {
        //Sequence Variables
        static sequence s1;
        SuffixTree ST;

        static char[] alphabet;

        struct sequence
        {
            public string name;
            public string sequenceString;
        }

        static string inputFile = "";
        static string alphabetFile = "";

        static void Main(string[] args)
        {
            inputFile = args[0];
            alphabetFile = args[1];
            

            populateSequences();

            populateAlphabet();

            SuffixTree ST = new SuffixTree(s1.sequenceString, alphabet);
            ST.buildTree(ST.root);


            Console.WriteLine("Finished Execution");

        }

        static void populateSequences()
        {
            StreamReader FastaReader = new StreamReader("./" + inputFile);
            string line;

            line = FastaReader.ReadLine().Trim().ToLower();
            if (line.StartsWith(">"))
            {
                s1.name = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[0].Substring(1);
                s1.sequenceString = "";
            }

            while ((line = FastaReader.ReadLine()) != null)
            {
                line = line.Trim().ToLower();
                if (line.StartsWith(">"))
                    break;
                s1.sequenceString += line;

                // Need '$' at the end of each string
                s1.sequenceString += '$';
            }

        }

        static void populateAlphabet()
        {
            StreamReader alphabetReader = new StreamReader("./" + alphabetFile);
            string line;

            line = alphabetReader.ReadLine().Trim().ToLower();
            string[] stringAlph = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            alphabet = new char[stringAlph.Length];
            int count = 0;
            foreach (string a in stringAlph)
            {
                alphabet[count] = a[0];
                count++;
            }

        }

        public void buildSuffixTree(string inputString)
        {
            // make sure string isn't empty
            if (String.IsNullOrEmpty(inputString))
            {
                throw new ArgumentNullException();
            }


        }
    }
}
