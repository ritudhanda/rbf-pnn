using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace RBF_PNN
{
    class ConfusionMatrix
    {
        private List<int> PredictedClasses;
        private List<int> ActualClasses;
        private List<int> Classes;
        private Dictionary<Tuple<int, int>, int> Matrix;

        public ConfusionMatrix(List<int> PredictedClasses, List<int> ActualClasses)
        {
            this.PredictedClasses = PredictedClasses;
            this.ActualClasses = ActualClasses;

            if (PredictedClasses.Count != ActualClasses.Count) throw new Exception("Data size mismatch!");

            Matrix = new Dictionary<Tuple<int, int>, int>();

            HashSet<int> set = new HashSet<int>(ActualClasses.ToArray());
            Classes = new List<int>(set);
            Classes.Sort();

            foreach (int i in Classes)
            {
                foreach (int j in Classes)
                {
                    Tuple<int, int> key = new Tuple<int, int>(i, j);
                    Matrix[key] = 0;
                }
            }
        }

        public void Compute()
        {
            for (int i = 0; i < ActualClasses.Count; i++)
            {
                Tuple<int, int> key = new Tuple<int, int>(PredictedClasses[i], ActualClasses[i]);
                if (Matrix.ContainsKey(key))
                    Matrix[key]++;
            }
        }

        public void Print()
        {
            Console.Write("P \\ A\t");
            foreach (int i in Classes)
            {
                Console.Write("C" + i + "\t");
            }
            Console.WriteLine();
            foreach (int i in Classes)
            {
                Console.Write("C" + i + "\t");
                foreach (int j in Classes)
                {
                    Tuple<int, int> key = new Tuple<int, int>(i, j);
                    Console.Write(Matrix[key] + "\t");
                }
                Console.WriteLine();
            }
        }

        public double GetAccuracy()
        {
            int diagonal = 0;
            foreach (int i in Classes)
            {
                Tuple<int, int> key = new Tuple<int, int>(i, i);
                diagonal += Matrix[key];
            }
            return (double)diagonal / (double)ActualClasses.Count * 100;
        }

    }
}
