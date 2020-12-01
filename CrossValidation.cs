using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W_KNN
{
    public class validationMetrices
    {

        public static double accuracy = 0;
        public static double precision = 0;
        public static double recall = 0;
        public static double f1 = 0;
    }

    class CrossValidation
    {
        public double getAccuracy(int[,] crossMat, int testCount)
        {
            double result = 0;

            for (int i = 0; i < crossMat.GetLength(0); i++)
                result += crossMat[i, i];

            result /= testCount;

            validationMetrices.accuracy += result;

            return result;
        }

        public double getPrecision(int[,] crossMat, int classCount) {

            double result = 0, denom;

            for (int i = 0; i < crossMat.GetLength(0); i++)
            {
                denom = 0;

                for (int j = 0; j < crossMat.GetLength(1); j++)
                    denom += crossMat[i, j];

                if (denom == 0)
                    classCount -= 1;
                else
                    result += crossMat[i, i] / denom;
            }

            result /= classCount;

            validationMetrices.precision += result;

            return result;

        }

        public double getRecall(int[,] crossMat, int classCount)
        {

            double result = 0, denom;

            for (int i = 0; i < crossMat.GetLength(0); i++)
            {
                denom = 0;

                for (int j = 0; j < crossMat.GetLength(1); j++)
                    denom += crossMat[j, i];

                if (denom == 0)
                    classCount-=1;
                else
                    result += crossMat[i, i] / denom;

            }
            result /= classCount;

            validationMetrices.recall += result;

            return result;

        }

        public double getF1(int[,] crossMat, int classCount)
        {

            double result = 0, denomP, denomR, precision=0, recall=0;

            for (int i = 0; i < crossMat.GetLength(0); i++)
            {
                denomP = denomR = 0;

                for (int j = 0; j < crossMat.GetLength(1); j++)
                {
                    denomP += crossMat[i,j];
                    denomR += crossMat[j,i];
                }
                if (denomP == 0)
                    continue;
                else
                    precision = crossMat[i, i] / denomP;

                if (denomR == 0)
                    continue;
                else
                    recall = crossMat[i, i] / denomR;

                result += 2 * precision * recall / (precision + recall);
            }

            result /= classCount;

            validationMetrices.f1 += result;

            return result;
        }
    }
}
