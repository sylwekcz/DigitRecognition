//using Accord.MachineLearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigitRecognition
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            testKNN();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }






        static void testKNN()
        {

            List<double[]> inputs = new List<double[]>()
            { 
                // 0
                    new double[] { -5, -2, -1 },
                    new double[] { -5, -5, -6 },

                    // 1
                    new double[] {  2,  1,  1 },
                    new double[] {  1,  1,  2 },
                    new double[] {  1,  2,  2 },
                    new double[] {  3,  1,  2 },

                    // 2
                    new double[] { 11,  5,  4 },
                    new double[] { 15,  5,  6 },
                    new double[] { 10,  5,  6 },
            };
            List<int> outputs = new List<int>()
            {
                        0, 0,        // 0
                        1, 1, 1, 1,  // 1
                        2, 2, 2      // 2
            };



           // KNearestNeighbors knn = new KNearestNeighbors(k: 4, classes: 3, inputs: inputs.ToArray(), outputs: outputs.ToArray());


           // int answer = knn.Compute(new double[] { 11, 5, 4 }); // 2
           // int answer2 = knn.Compute(new double[] { -11, -5, -4 }); // 0

           // Console.WriteLine("dla: (11, 5, 4)  wynik:" + answer);
           // Console.WriteLine("dla: (-11, -5, -4)  wynik:" + answer2);
           // Console.ReadLine();
        }
    }


    
}
