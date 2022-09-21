using System.Diagnostics;

namespace Lottery
{
    class Program
    {
        public static int activeThreadCount = 0; //how many games of the lottery are being played now
        public static int totalThreadCount = 0; //how many total games have been started
        public static int attempts = 32; //total number of games meant to be played
        public static double[] allAttempts = new double[attempts]; //array of doubles with one index for every game
        public static string display = ""; //final display
        public static int togo = attempts; //how many games still need to be completed; starts at total attempt number
        public static Stopwatch SW = new(); //timer for how long completion takes
        public static int current = -1;
        static void Main()
        {
            SW.Start();
            while (totalThreadCount < attempts) //haven't started enough threads for every attempt
            {
                if (activeThreadCount < 16 && (totalThreadCount != current)) /* limits it to a max of 16 games being played at once
                                                                              * second condition makes sure counts have been updated so as to not cause race conditions*/
                {
                    current = totalThreadCount;
                    Thread my_thread = new(() => StartZero(totalThreadCount)); /*starts a thread to run the StartZero function 
                                                                                * with the number of threads before it as the index */
                    my_thread.Start();
                }
            }
        }

        static void StartZero(int index)
        {
            activeThreadCount++;
            totalThreadCount++;
            Console.WriteLine("Starting thread #" + index + " " + activeThreadCount + " threads are active");
            double runs = 0; //number of times an attempt has been made to win
            Random random = new(); //creates random number generator
            while (true)
            {
                runs++; //every loop tries to get a 0
                int coin1 = random.Next(0, 292201338); //odds of winning the lottery
                if (coin1 == 0) //if success
                {
                    Console.WriteLine("adding " + runs + " to allattempts[" + (index) + "]");
                    allAttempts[index] = runs; //each thread should have a unique index
                    togo--; //success means 1 game is done
                    Console.WriteLine("Thread #" + index + " finished after " + runs + " runs"); //shows what thread finished and how many attempts
                    if (togo == 0) //if all games have been completed
                    {
                        // Array.Sort(allAttempts); //sorts array into luckiest to least lucky
                        display = String.Join(Environment.NewLine, allAttempts); //creates a string of all attempts
                        Console.WriteLine(display); //writes the string to console
                        SW.Stop(); //stops timer
                        Console.WriteLine(SW.Elapsed); //writes how long program took
                        Console.WriteLine(allAttempts.Average()); //shows average number of runs to succeed
                    }
                    activeThreadCount--; //thread is done so active number goes down
                    break; //ends while loop terminating thread
                }
            }
        }
    }
}