using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperisChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Results> Result = new List<Results>();
            List<Position> Matrix = new List<Position>();
            string textFile = "map.txt";
            string[] max = new string[2];
            if (args.Length > 1)
            {
                textFile = args[0];
            }
            if (string.IsNullOrEmpty(textFile))
            {
                Console.WriteLine("No data to analize");
                Console.ReadKey();
                return;
            }
            //If file exists
            if (File.Exists(textFile))
            {
                // Read all lines 
                string[] lines = File.ReadAllLines(textFile);
                //Get the maxtrix dimension
                max = lines[0].Split(' ');
                //Delete first line (matrix dimension)
                lines = lines.Skip(1).ToArray();
                //Define counter
                int counter = 0;
                //For every line found
                foreach (string line in lines)
                {
                    int column = 0;
                    string[] data = line.Split(' ');
                    //Create the Position list
                    foreach (string value in data)
                    {
                        Position pos = new Position
                        {
                            column = column++,
                            row = counter,
                            value = Int32.Parse(value)
                        };
                        Matrix.Add(pos);
                    }
                    counter++;
                }
            }

            //If no data on Matrix
            if (Matrix.Count == 0)
            {
                Console.WriteLine("No data on matrix to analize");
                Console.ReadKey();
                return;
            }
            //Order the matrix for starter Position
            List<Position> OrderedMatrix = Matrix.OrderByDescending(m => m.value).ToList();
            foreach (Position pos in OrderedMatrix)
            {
                //Gets all weights, if not defined
                if (pos.GetAllWeights)
                {
                    OrderedMatrix.GetNorthWeight(pos);
                    OrderedMatrix.GetWestWeight(pos);
                    OrderedMatrix.GetSouthWeight(pos, Int16.Parse(max[0]));
                    OrderedMatrix.GetEastWeight(pos, Int16.Parse(max[1]));
                }
                //Check every weight
                foreach (Weight weight in pos.weight.OrderBy(w => w.value))
                {
                    Results path = new Results();
                    OrderedMatrix.AddStep(pos, weight, max, ref path, true);
                    Result.Add(path);
                }
            }

            //Print the results
            Results FinalResult = Result.OrderByDescending(r => r.Drop).ThenByDescending(r => r.Steps).FirstOrDefault();
            Console.WriteLine(string.Join(" => ", FinalResult.trace.Select(x => x.value.ToString()).ToArray()));
            Console.WriteLine("Steps: " + FinalResult.Steps);
            Console.WriteLine("Drop: " + FinalResult.Drop);
            Console.WriteLine("Press a key to continue...");
            Console.ReadLine();
        }


    }
}
