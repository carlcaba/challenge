using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperisChallenge
{
    /// <summary>
    /// Class to determine the weight between two positions
    /// </summary>
    public class Weight
    {
        /// <summary>
        /// Value of weight
        /// </summary>
        public int value { get; set; }
        /// <summary>
        /// Direction (North, West, South, East)
        /// </summary>
        public string direction { get; set; }
        /// <summary>
        /// Index of the column (zero-based)
        /// </summary>
        public int column { get; set; }
        /// <summary>
        /// Index of the row (zero-based)
        /// </summary>
        public int row { get; set; }
    }

    /// <summary>
    /// Class for any position on matrix
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Index of the column (zero-based)
        /// </summary>
        public int column { get; set; }
        /// <summary>
        /// Index of the row (zero-based)
        /// </summary>
        public int row { get; set; }
        /// <summary>
        /// Value of position
        /// </summary>
        public int value { get; set; }
        /// <summary>
        /// List of all weights
        /// </summary>
        public List<Weight> weight { get; set; }
        /// <summary>
        /// Property to determine if all weight has been already calculated
        /// </summary>
        public bool GetAllWeights
        {
            get
            {
                return this.weight.Count() == 0;
            }
        }
        /// <summary>
        /// Class constructor
        /// </summary>
        public Position()
        {
            //Assign weights by default
            this.weight = new List<Weight>();
        }
    }
    /// <summary>
    /// Class to get all posible paths
    /// </summary>
    public class Results
    {
        /// <summary>
        /// Trace of every position
        /// </summary>
        public List<Position> trace { get; set; }
        /// <summary>
        /// Property to count steps
        /// </summary>
        public int Steps
        {
            get
            {
                int result = 0;
                if (this.trace != null)
                {
                    result = this.trace.Count();
                }
                return result;
            }
        }
        /// <summary>
        /// Property to calculate drop
        /// </summary>
        public int Drop
        {
            get
            {
                int result = 0;
                if (this.trace != null && this.trace.Count() > 0)
                {
                    result = this.trace.Max(x => x.value) - this.trace.Min(x => x.value);
                }
                return result;
            }
        }
        /// <summary>
        /// Class constructor
        /// </summary>
        public Results()
        {
            this.trace = new List<Position>();
        }
    }

    /// <summary>
    /// Extension to list of class Position
    /// </summary>
    public static class PositionExtension
    {
        /// <summary>
        /// Get the North Limit and weight
        /// </summary>
        /// <param name="list">Reference to object</param>
        /// <param name="item">Position to analize</param>
        public static void GetNorthWeight(this List<Position> list, Position item)
        {
            int? result = null;
            //Add north
            if (item.row > 0)
            {
                Position limit_north = list.Where(x => x.row == (item.row - 1) && x.column == item.column).FirstOrDefault();
                result = (limit_north != null ? (item.value - limit_north.value) : result);
            }
            //Gets only numbers > 0
            if (result != null && result > 0) {
                item.weight.Add(new Weight { direction = "N", value = result.Value, row = item.row - 1, column = item.column });
            }
        }
        /// <summary>
        /// Get the South Limit and weight
        /// </summary>
        /// <param name="list">Reference to object</param>
        /// <param name="item">Position to analize</param>
        public static void GetSouthWeight(this List<Position> list, Position item, int max_row)
        {
            int? result = null;
            //Add south
            if (item.row < max_row)
            {
                Position limit_south = list.Where(x => x.row == (item.row + 1) && x.column == item.column).FirstOrDefault();
                result = (limit_south != null ? item.value - limit_south.value : result);
            }
            //Gets only numbers > 0
            if (result != null && result > 0)
            {
                item.weight.Add(new Weight { direction = "S", value = result.Value, row = item.row + 1, column = item.column });
            }
        }
        /// <summary>
        /// Get the West Limit and weight
        /// </summary>
        /// <param name="list">Reference to object</param>
        /// <param name="item">Position to analize</param>
        public static void GetWestWeight(this List<Position> list, Position item)
        {
            int? result = null;
            //Add west
            if (item.column > 0)
            {
                Position limit_west = list.Where(x => x.row == item.row && x.column == (item.column - 1)).FirstOrDefault();
                result = (limit_west != null ? item.value - limit_west.value : result);
            }
            //Gets only numbers > 0
            if (result != null && result > 0)
            {
                item.weight.Add(new Weight { direction = "W", value = result.Value, row = item.row, column = item.column - 1 });
            }
        }
        /// <summary>
        /// Get the East Limit and weight
        /// </summary>
        /// <param name="list">Reference to object</param>
        /// <param name="item">Position to analize</param>
        public static void GetEastWeight(this List<Position> list, Position item, int max_col)
        {
            int? result = null;
            //Add east
            if (item.column < max_col)
            {
                Position limit_east = list.Where(x => x.row == item.row && x.column == (item.column + 1)).FirstOrDefault();
                result = (limit_east != null ? item.value - limit_east.value : result);
            }
            //Gets only numbers > 0
            if (result != null && result > 0)
            {
                item.weight.Add(new Weight { direction = "E", value = result.Value, row = item.row, column = item.column + 1 });
            }
        }
        /// <summary>
        /// Recursively function to find a path
        /// </summary>
        /// <param name="list">Reference to object</param>
        /// <param name="initial">Initial position</param>
        /// <param name="lead">Direction to follow</param>
        /// <param name="max">Maximum items (matrix dimension)</param>
        /// <param name="result">By reference: List of position to trace</param>
        /// <param name="starter">Defines if function is called recursively</param>
        public static void AddStep(this List<Position> list, Position initial, Weight lead, string[] max, ref Results result, bool starter = false)
        {
            //Gets all weights, if not defined
            if (initial.GetAllWeights)
            {
                list.GetNorthWeight(initial);
                list.GetWestWeight(initial);
                list.GetSouthWeight(initial, Int16.Parse(max[0]));
                list.GetEastWeight(initial, Int16.Parse(max[1]));
            }
            //Add Position to result
            result.trace.Add(initial);
            //Variable for direction
            Weight direction = null;
            //Determine the direction
            if (starter)
            {
                direction = lead;
            }
            else
            {
                direction = initial.weight.GetMin();
            }
            //If there is no direction to lead
            if (direction != null)
            {
                //Detemine the next Position
                Position next = list.Where(n => n.row == direction.row && n.column == direction.column).FirstOrDefault();
                //If no path to lead
                if (next != null)
                {
                    //Calls recursively
                    list.AddStep(next, direction, max, ref result, false);
                }
            }
        }
    }

    /// <summary>
    /// Extension to list of class Weight
    /// </summary>
    public static class WeightExtension
    {
        /// <summary>
        /// Get the minimum weight
        /// </summary>
        /// <param name="list">Reference to object</param>
        /// <returns>Minimum weight object or null</returns>
        public static Weight GetMin(this List<Weight> list)
        {
            Weight result = null;
            foreach (Weight wght in list)
            {
                if (result == null)
                {
                    result = new Weight
                    {
                        column = wght.column,
                        direction = wght.direction,
                        row = wght.row,
                        value = wght.value
                    };
                }
                else
                {
                    if (wght.value < result.value)
                    {
                        result = new Weight
                        {
                            column = wght.column,
                            direction = wght.direction,
                            row = wght.row,
                            value = wght.value
                        };
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Get the maximum weight
        /// </summary>
        /// <param name="list">Reference to object</param>
        /// <returns>Maximum weight object or null</returns>
        public static Weight GetMax(this List<Weight> list)
        {
            Weight result = null;
            foreach (Weight wght in list)
            {
                if (result == null)
                {
                    result = new Weight
                    {
                        column = wght.column,
                        direction = wght.direction,
                        row = wght.row,
                        value = wght.value
                    };
                }
                else
                {
                    if (wght.value > result.value)
                    {
                        result = new Weight
                        {
                            column = wght.column,
                            direction = wght.direction,
                            row = wght.row,
                            value = wght.value
                        };
                    }
                }
            }
            return result;
        }
    }
}
