using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace RBF_PNN
{
    class PNN
    {
        private int NumberOfClass;
        private int[] ClassLabel;
        private double Spread;
        private Dictionary<int, List<Vector>> TrainingData;
        private double[] ClassProbability; // indexed 0, 1, etc

        public PNN(int Nc, int[] InClassLabel)
        {
            NumberOfClass = Nc;
            if (Nc != InClassLabel.Length) throw new Exception("Number of class mismatch");
            TrainingData = new Dictionary<int, List<Vector>>();
            for (int i = 0; i < Nc; i++)
            {
                TrainingData.Add(InClassLabel[i], new List<Vector>());
            }
            ClassProbability = new double[Nc];
            ClassLabel = new int[Nc];
            InClassLabel.CopyTo(ClassLabel, 0);
        }

        public void AddTrainingData(int ClassLabel, Vector Data)
        {
            TrainingData[ClassLabel].Add(Data);
        }

        public Vector GetTrainingData(int ClassLabel, int Index)
        {
            return TrainingData[ClassLabel][Index];
        }

        public int GetNumberOfInstances(int ClassLabel)
        {
            return TrainingData[ClassLabel].Count;
        }

        public void Train()
        {
            // Do nothing, PNN does not need training
        }

        // Simulate with test data, output: predicted class label
        public int Simulate(Vector TestData, double Spread)
        {
            this.Spread = Spread;

            for (int i = 0; i < NumberOfClass; i++)
            {
                int classLabel = this.ClassLabel[i];
                for (int j = 0; j < GetNumberOfInstances(classLabel); j++)
                {
                    ClassProbability[i] += Math.Exp(-Math.Pow((TrainingData[classLabel][j] - TestData).Norm(), 2) / Math.Pow((2 * Spread), 2));
                }
                ClassProbability[i] /= GetNumberOfInstances(classLabel);
            }

            double Max = ClassProbability[0];
            int MaxIndex = 0;

            for (int i = 1; i < NumberOfClass; i++)
            {
                if (ClassProbability[i] > Max)
                {
                    MaxIndex = i;
                    Max = ClassProbability[i];
                }
            }

            return ClassLabel[MaxIndex];
        }
    }
}
