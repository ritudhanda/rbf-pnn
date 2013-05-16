using System;
using System.Collections.Generic;
using System.Text;

namespace RBF_PNN
{
    class RBF
    {
        private int NumberOfData;
        private int DimensionOfData;
        private int NumberOfCenter;
        private double Spread;
        private List<Vector> TrainingData;
        private List<int> TargetData;
        private Matrix DataMatrix;
        private Matrix CenterMatrix;
        private Matrix TargetMatrix;
        private Matrix WeightMatrix;
        private Matrix PhiMatrix;

        public RBF(int NumberOfData, int DimensionOfData, int NumberOfCenter, double Spread)
        {
            this.DimensionOfData = DimensionOfData;
            this.NumberOfData = NumberOfData;
            this.NumberOfCenter = NumberOfCenter;
            this.Spread = Spread;

            this.DataMatrix = new Matrix(NumberOfData, DimensionOfData);
            this.CenterMatrix = new Matrix(NumberOfCenter, DimensionOfData);
            this.TargetMatrix = new Matrix(NumberOfData, 1);

            TrainingData = new List<Vector>();
            TargetData = new List<int>();
        }

        public double GaussRBF(Vector X, Vector Center)
        {
            return Math.Exp(-Math.Pow((X - Center).Norm(), 2) / Math.Pow((2 * this.Spread), 2));
        }

        public void AddTrainingData(int ClassLabel, Vector Data)
        {
            TargetData.Add(ClassLabel);
            TrainingData.Add(Data);
        }

        public void SetCenter(Matrix CenterMatrix)
        {
            this.CenterMatrix = CenterMatrix;
        }

        public void Train()
        {
            // Vector to matrices
            for (int i = 0; i < TrainingData.Count; i++)
            {
                for (int j = 0; j < TrainingData[i].Length; j++)
                {
                    DataMatrix[i, j] = TrainingData[i][j];
                }
                TargetMatrix[i, 0] = TargetData[i];
            }

            // TODO: Center calculation using K-means

            // Compute phi
            PhiMatrix = new Matrix(NumberOfData, NumberOfCenter);
            for (int i = 0; i < NumberOfData; i++)
            {
                for (int j = 0; j < NumberOfCenter; j++)
                {
                    double[] dataRow = DataMatrix.GetRow(i);
                    double[] centerRow = CenterMatrix.GetRow(j);
                    Vector dataVector = new Vector(dataRow);
                    Vector centerVector = new Vector(centerRow);

                    PhiMatrix[i, j] = GaussRBF(dataVector, centerVector);
                }
            }

            // Calculate weight
            WeightMatrix = new Matrix(NumberOfCenter, 1);
            WeightMatrix = (PhiMatrix.Transpose() * PhiMatrix).Invert() * PhiMatrix.Transpose() * TargetMatrix;
        }

        public int Simulate(Vector TestData)
        {
            double y = 0.0;
            for (int i = 0; i < NumberOfCenter; i++)
            {
                double[] centerRow = CenterMatrix.GetRow(i);
                Vector centerVector = new Vector(centerRow);

                y += WeightMatrix.GetRow(i)[0] * GaussRBF(TestData, centerVector);
            }

            return (int)Math.Round(y);
        }
    }
}
