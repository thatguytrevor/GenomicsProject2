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

        struct sequence
        {
            public string name;
            public string sequenceString;
        }

        //Sequence Variables
        static sequence s1;

        static string[] alphabet;


        static string inputFile = "";
        static string alphabetFile = "";

        static void Main(string[] args)
        {
            inputFile = args[0];
            alphabetFile = args[1];

            populateSequences();

            populateAlphabet();

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
            }

        }

        static void populateAlphabet()
        {
            StreamReader alphabetReader = new StreamReader("./" + alphabetFile);
            string line;

            line = alphabetReader.ReadLine().Trim().ToLower();
            alphabet = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

        }
    }
}
