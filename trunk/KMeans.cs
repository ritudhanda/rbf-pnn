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
            this.Dim = Data.Length;
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
                sum = sum + Math.Pow((Data[dataIndex][i] - Centroid[centroidIndex][i]), 2);
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
                    Centroid[i][j] = 0.0;
                }
            }

            for (int i = 0; i < nData; i++)
            {
                indexCount[indexArray[i]]++;
                for (int j = 0; j < Dim; j++)
                {
                    Centroid[indexArray[i]][j] = Centroid[indexArray[i]][j] + Data[i][j];
                }
            }
            
            for (int i = 0; i < K; i++)
            {
                if (indexCount[i] != 0)
                {
                    for (int j = 0; j < Dim; j++)
                    {
                        Centroid[i][j] = Centroid[i][j] / (double)indexCount[i];
                    }
                } else {
                    for (int j = 0; j < Dim; j++)
                    {
                        Centroid[i][j] = r.NextDouble();
                    }                    
                }
            }
        }

        public void DoClustering(int maxIter, double diffThreshold)
        {
            InitCentroid();

            for (int i = 0; i < maxIter; i++)
            {
                List<Vector> prevCentroid = new List<Vector>();
                List<Vector> currCentroid = new List<Vector>();

                for (int a = 0; a < K; a++)
                {
                    prevCentroid.Add(new Vector(Dim));
                    for (int b = 0; b < Dim; b++)
                    {
                        prevCentroid[a][b] = Centroid[a][b];
                    }
                }

                RecalculateCentroid();
                for (int a = 0; a < K; a++)
                {
                    currCentroid.Add(new Vector(Dim));
                    for (int b = 0; b < Dim; b++)
                    {
                        currCentroid[a][b] = Centroid[a][b];
                    }
                }

                double diffSum = 0.0;
                for (int j = 0; j < K; j++)
                {
                    double diff = (currCentroid[j] - prevCentroid[j]).Norm();
                    diffSum += diff;
                }

                if (diffSum <= diffThreshold) break;
            }
        }

        public List<Vector> GetCentroid()
        {
            return Centroid;
        }

    }
}
