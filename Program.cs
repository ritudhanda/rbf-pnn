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

            // build the RBF
            RBF rbf = new RBF(data.Count, 30, 2, 1);
            for (int i = 0; i < data.Count; i++)
            {
                rbf.AddTrainingData(classLabel[i], data[i]);
            }

            // center calculated using K-means
            Matrix rbfCenter = new Matrix(2, 30);
            rbfCenter.SetRow(0, new double[] { 0.504835626751323, 0.395603294455027, 0.505786614275132, 0.363765759428571, 0.469887315973545, 0.422263022137566, 0.418386615259259, 0.469280349650794, 0.458997381253968, 0.299458864306878, 0.190930854798942, 0.191120727730159, 0.179034326084656, 0.130864324005291, 0.180179621169312, 0.258901261629630, 0.125424750164021, 0.309427791698413, 0.190072001978836, 0.132669747687831, 0.480474477619048, 0.451073713084656, 0.465530203322751, 0.314605973021164, 0.498688167502646, 0.363914605708995, 0.390272917830688, 0.658271968603174, 0.337522958476190, 0.260413868629630 });
            rbfCenter.SetRow(1, new double[] { 0.255353579886842, 0.288334549594737, 0.246964164255263, 0.143883685905263, 0.357430759794737, 0.180194708760526, 0.103447755023684, 0.130660301331579, 0.340118288142105, 0.255916064347368, 0.064274850631579, 0.188430427860526, 0.059756631960527, 0.0287010784605263, 0.181586282905263, 0.132429405794737, 0.0582152751315790, 0.180693362981579, 0.172210566321053, 0.0840399590052632, 0.205240596160526, 0.320690017942105, 0.192421382889474, 0.0994344640500000, 0.357111501926316, 0.148739346226316, 0.131422875005263, 0.262313628155263, 0.226394119534211, 0.154373537139474 });

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
