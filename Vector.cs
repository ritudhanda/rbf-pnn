using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace RBF_PNN
{
    class Vector
    {
        private double[] Value;
        public int Length {
            get { return Value.Length; }
        }
        public double this[int index] {
            get { return Value[index]; }
            set { Value[index] = value; } 
        }

        public Vector(params double[] InValue)
        {
            Value = new double[InValue.Length];
            for (int i = 0; i < InValue.Length; i++)
            {
                Value[i] = InValue[i];
            }
        }

        public Vector(int length)
        {
            Value = new double[length];
        }

        public double Get(int index)
        {
            return Value[index];
        }

        public void Set(int index, double Value)
        {
            this.Value[index] = Value;
        }

        public double[] GetValueArray()
        {
            return Value;
        }

        public static Vector operator +(Vector V1, Vector V2)
        {
            if (V1.Length != V2.Length) throw new Exception("Vector dimension mismatch!");
            double[] Temp = new double[V1.Length];
            for (int i = 0; i < V1.Length; i++)
            {
                Temp[i] = V1[i] + V2[i];
            }
            return new Vector(Temp);
        }

        public static Vector operator -(Vector V1, Vector V2)
        {
            if (V1.Length != V2.Length) throw new Exception("Vector dimension mismatch!");
            double[] Temp = new double[V1.Length];
            for (int i = 0; i < V1.Length; i++)
            {
                Temp[i] = V1[i] - V2[i];
            }
            return new Vector(Temp);
        }

        public static Vector operator *(Vector V, double S)
        {
            for (int i = 0; i < V.Length; i++)
            {
                V[i] = V[i] * S;
            }
            return V;
        }

        public static Vector operator *(double S, Vector V)
        {
            return V * S;
        }

        // Dot product
        public static double operator *(Vector V1, Vector V2)
        {
            return V1.Dot(V2);
        }

        public double Dot(Vector V)
        {
            if (this.Length != V.Length) throw new Exception("Vector dimension mismatch!");
            double sum = 0.0;
            for (int i = 0; i < this.Length; i++)
            {
                sum += Value[i] * V[i];
            }
            return sum;
        }

        // Euclidean (2-Norm)
        public double Norm()
        {
            return Math.Sqrt(this * this);
        }

        public void Print()
        {
            for (int i = 0; i < Value.Length; i++)
            {
                Console.Write(Value[i] + " ");
            }
            Console.WriteLine();
        }
    }
}
