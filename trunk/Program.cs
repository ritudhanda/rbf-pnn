using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace RBF_PNN
{
    class Program
    {
        static void Main(string[] args)
        {
            // read the data
            string line;
            StreamReader dataFile = new StreamReader("E:\\documents\\com\\pasca\\patternrec\\RBF_PNN\\wdbc-normal.data");
            List<Vector> data = new List<Vector>();
            List<int> classLabel = new List<int>();

            while ((line = dataFile.ReadLine()) != null)
            {
                if (line.IndexOf('?') == -1)
                {
                    string[] dataLine = line.Split(',');
                    double[] dataLineNum = new double[30];
                    for (int i = 0; i < 30; i++) dataLineNum[i] = double.Parse(dataLine[i + 2]);

                    data.Add(new Vector(dataLineNum));

                    // 1 = Benign, 2 = Malignant
                    int c = (dataLine[1] == "B") ? 1 : 2;
                    classLabel.Add(c);
                }
            }
            dataFile.Close();

            // K-means for centroid calculation

            int k = 2;
            KMeans kmeans = new KMeans(k);
            for (int i = 0; i < data.Count; i++)
            {
                kmeans.AddData(data[i]);
            }
            kmeans.DoClustering(100, 0.001);

            // build the RBF
            RBF rbf = new RBF(data.Count, 30, k, 1);
            for (int i = 0; i < data.Count; i++)
            {
                rbf.AddTrainingData(classLabel[i], data[i]);
            }

            // center calculated using K-means
            Matrix rbfCenter = new Matrix(k, 30);
            for (int i = 0; i < k; i++)
            {
                rbfCenter.SetRow(i, kmeans.GetCentroid()[i].GetValue());
            }

            rbf.SetCenter(rbfCenter);
            rbf.Train();

            // testing
            List<int> rbfActualClass = classLabel;
            List<int> rbfPredictedClass = new List<int>();
            for (int i = 0; i < data.Count; i++)
            {
                rbfPredictedClass.Add(rbf.Simulate(data[i]));
            }

            Console.WriteLine("RBF:\n");

            ConfusionMatrix rbfConfusionMat = new ConfusionMatrix(rbfPredictedClass, rbfActualClass);
            rbfConfusionMat.Compute();
            rbfConfusionMat.Print();

            Console.WriteLine("Accuracy: " + rbfConfusionMat.GetAccuracy());

            // build the PNN
            PNN pnn = new PNN(2, new int[] { 1, 2 });
            for (int i = 0; i < data.Count; i++)
            {
                pnn.AddTrainingData(classLabel[i], data[i]);
            }
            pnn.Train();

            // testing
            List<int> pnnActualClass = classLabel;
            List<int> pnnPredictedClass = new List<int>();
            for (int i = 0; i < data.Count; i++)
            {
                pnnPredictedClass.Add(pnn.Simulate(data[i], 1));
            }

            Console.WriteLine("\n\nPNN:\n");

            ConfusionMatrix pnnConfusionMat = new ConfusionMatrix(pnnPredictedClass, pnnActualClass);
            pnnConfusionMat.Compute();
            pnnConfusionMat.Print();

            Console.WriteLine("Accuracy: " + pnnConfusionMat.GetAccuracy());
            
            Console.Read();
        }
    }
}
