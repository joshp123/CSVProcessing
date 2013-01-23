using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_3_Warmup
{
    class Program
    {
        // copyright josh palmer 2k13 don't steal my shit guys!!!!
        
        // Declaring instance of Random outside of the Hop() function, because:
        // otherwise when calling Hop to create a new Random in close succession, they'll have the same system
        // time as seed and thus will not be random.

        static Random rand = new Random();

        protected static double GetNextDouble()
        {
            return rand.NextDouble();
        }


        static void Main(string[] args)
        {
            int max_number_of_random_walks = 50; // 5 mil walks
            int[] positions_array = new int[max_number_of_random_walks];
            int max_time = 48; // T = 5000
            
            Console.WriteLine("Working...");
            
            // begin getting positions
            
            for (int i = 0; i < max_number_of_random_walks; i++)
            {
                positions_array[i] = RandomWalk(max_time);
                // do a random walk, store the position returned in the array
                
                // Console.WriteLine(i); // debug shit
            }
            
            Console.WriteLine("Writing file");
            
            // take our array of positions and convert them to an array of frequencies that can be used for generating
            // a histogram, and write this to a csv file with the currently logged in users name appeneded

            var histogram = ArrayToHistogram(positions_array);
            
            DictionaryToCSV(histogram);

        }

        static int RandomWalk(int max_time)
        {
            // This function simulates a random walk up to a maximum time
            // Takes maximum time as an argument
            // returns the position of the object after the maximum time
            
            int position = 0; // position, x, start at the origin
            double probability = 0.5;
            int time = 0; // time counter

            while (time <= max_time)
            {

                // start of a "hop", increase time by 1 unit

                // flip coin to see if you hop
                if (GetNextDouble() < probability)
                    position++;
                else
                    position--;

                time++;
            }

            return position;
        }

        static int DictionaryToCSV<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            // Generic function that writes a CSV file from a Dictionary
            // (by generic it takes literally any type of variable as an argument which is pretty damn awesome; strings, floats ints, custom types, you name it, it works)
            // C# is the best

            // Limitations: if your dictionary has over 1m key value pairs, it will hit excel's row limit
            // If any of the objects in Dictionary contain "," this will break the CSV
            
            // TODO: implement code to check the sanity of the input dictionary and avoid these limitations

            string userName = System.Environment.UserName;
            // Get the username of the currently logged in user

            // userName = userName.Substring(3);

            Console.WriteLine(userName);

            string pathToCSV;
            if (userName == "py11j3p")
            {
                pathToCSV = "M:\\Computing2\\Project3\\Project3Warmup\\data_" + userName + ".csv";
            }

            else
            {
                pathToCSV = System.IO.Directory.GetCurrentDirectory() + "\\Data_" + userName + ".csv";
                // create file in current working directory with username appended for creating data
            }
            
            Console.WriteLine("Your data will be saved at :" + pathToCSV);
            
            // these few lines are the actual heavy lifting: 
            // take every item in the histogram and 
            String csv = String.Join(
                Environment.NewLine,
                dictionary.Select(d => d.Key + "," + d.Value + ",")
            );
            // this will break if any of the arguments have "," in them

            System.IO.File.WriteAllText(pathToCSV, csv);

            // TODO: add exceptions here to catch when a file is open and unable to be overwritten
            // so i don't end up with runtime errors everywhere when i'm debugging and forget to close excel laffo
            
            // TODO: update this with proper return codes
            // TODO: prompt the user to see if they want to open the file after writing
            
            return -1;
        }
        
        private static Dictionary<int, int> ArrayToHistogram(int[] array)
        {
            // this can be generic'd by using IList<T> array instead of int[] array and Dictionary<TKey, int>

            // count occurence of everything using a LINQ query, group them by number, then count the occurence of each number
            var query = from item in array
                        group item by item into g
                        orderby g.Key
                        select new { Count = g.Count(), Value = g.Key };


            Dictionary<int, int> histogram = query.ToDictionary(var => var.Value, var => var.Count);
            // Take the previous IEnumerable object "query" and turn it into a Dictionary of key/value pairs (value and count)

            // using dictionary structure here to make ordering by value easy (esp when values are negative, as that wouldn't work with arrays
            // also if values are non-continuous it's just loads easier and better

            return histogram;
        }

    }

}
