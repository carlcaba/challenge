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
				//Get the result so far
                Results FR = Result.OrderByDescending(r => r.Steps).ThenByDescending(r => r.Drop).FirstOrDefault();
                if (FR != null)
                {
                    if (FR.Drop > pos.value)
                    {
                        continue;
                    }
                }
                else
                {
                    FR = new Results();
                }
				//If value is less than result's drop
				if (pos.value <= FR.Drop)
                {
                    break;
                }
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
					//If has a drop and weight value is greater 
                    if (FR.Drop > 0 && weight.value >= FR.Drop)
                    {
                        continue;
                    }
					//Process position
                    OrderedMatrix.AddStep(pos, weight, max, ref path, true);
					//If path has more steps 
                    if (path.Steps >= FR.Steps)
                    {
                        Result.Add(path);
                        Console.WriteLine("Max: " + string.Join(" => ", FR.trace.Select(x => x.value.ToString()).ToArray()));
                        Console.WriteLine("Max Steps: " + FR.Steps);
                        Console.WriteLine("Max Drop: " + FR.Drop);
                    }
                    Console.WriteLine("Current: " + string.Join(" => ", path.trace.Select(x => x.value.ToString()).ToArray()));
                    Console.WriteLine("Current Position: " + pos.value);
                    Console.WriteLine("Current Steps: " + path.Steps);
                    Console.WriteLine("Current Drop: " + path.Drop);
                }
            }

            //Print the results
            Results FinalResult = Result.OrderByDescending(r => r.Steps).ThenByDescending(r => r.Drop).FirstOrDefault();
            Console.WriteLine(string.Join(" => ", FinalResult.trace.Select(x => x.value.ToString()).ToArray()));
            Console.WriteLine("Steps: " + FinalResult.Steps);
            Console.WriteLine("Drop: " + FinalResult.Drop);
            Console.WriteLine("Press a key to continue...");
            Console.ReadLine();
        }


    }
}
