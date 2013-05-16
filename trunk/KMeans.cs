using System;
using System.Collections.Generic;
using System.Text;

namespace RBF_PNN
{
    class KMeans
    {
        private int K;
        private int Dim;
        private List<Vector> Centroid;
        private List<Vector> Data;

        public KMeans(int k)
        {
            this.K = k;
            Data = new List<Vector>();
            Centroid = new List<Vector>();
        }

        public void AddData(Vector Data)
        {
            this.Data.Add(Data);
            this.Dim = Data.Length();
        }

        private void InitCentroid()
        {
            // null centroids
            Random r = new Random();
            double[] nulls = new double[Dim];
            for (int i = 0; i < K; i++)
            {
                Centroid.Add(new Vector(nulls));
            }
        }

        public void PrintCentroid()
        {
            for (int i = 0; i < K; i++)
            {
                Centroid[i].Print();
            }
        }

        private double DistanceToCentroid(int dataIndex, int centroidIndex)
        {
            double sum = 0.0;
            for (int i = 0; i < Dim; i++)
            {
                sum = sum + Math.Pow((Data[dataIndex].Get(i) - Centroid[centroidIndex].Get(i)), 2);
            }
            return Math.Sqrt(sum);
        }

        private int[] FindNearestCentroid()
        {
            // find nearest centroid for each data
            int nData = Data.Count;
            int[] indexArray = new int[nData];
            for (int i = 0; i < nData; i++)
            {
                double minDist = DistanceToCentroid(i, 0);
                int minIdx = 0;
                for (int j = 0; j < K; j++)
                {
                    double dist = DistanceToCentroid(i, j);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        minIdx = j;
                    }
                }
                indexArray[i] = minIdx;
            }
            return indexArray;
        }

        private void RecalculateCentroid()
        {
            Random r = new Random();
            int nData = Data.Count;
            double[] indexCount = new double[K];
            int[] indexArray = FindNearestCentroid();

            // nullify centroid
            for (int i = 0; i < K; i++)
            {
                for (int j = 0; j < Dim; j++)
                {
                    Centroid[i].Set(j, 0.0);
                }
            }

            for (int i = 0; i < nData; i++)
            {
                indexCount[indexArray[i]]++;
                for (int j = 0; j < Dim; j++)
                {
                    double newVal = Centroid[indexArray[i]].Get(j) + Data[i].Get(j);
                    Centroid[indexArray[i]].Set(j, newVal);
                }
            }
            
            for (int i = 0; i < K; i++)
            {
                if (indexCount[i] != 0)
                {
                    for (int j = 0; j < Dim; j++)
                    {
                        double newVal = Centroid[i].Get(j) / (double)indexCount[i];
                        Centroid[i].Set(j, newVal);
                    }
                } else {
                    for (int j = 0; j < Dim; j++)
                    {
                        double newVal = r.NextDouble();
                        Centroid[i].Set(j, newVal);
                    }                    
                }
            }
        }

        public void DoClustering(int maxIter)
        {
            InitCentroid();

            for (int i = 0; i < maxIter; i++)
            {
                RecalculateCentroid();
            }
        }

        public List<Vector> GetCentroid()
        {
            return Centroid;
        }

    }
}
