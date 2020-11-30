using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W_KNN
{
    public class ClsWt
    {
        public string cls { get; set; }
        public double weight { get; set; }
        
    }
    class Predictor
    {
      
        SortedDictionary<string,double> clsNwet = new SortedDictionary<string, double>();
        HashSet<string> classes;
        List<List<string>> train, test;
        int k;
       
        public Predictor(List<List<string>> train, List<List<string>> test,int k, HashSet<string> classes) {
            this.train = train;
            this.test = test;
            this.k = k;
            this.classes = classes;

            foreach (string str in classes) {
                clsNwet.Add(str,0.0);
            }
            //beginTest();
        }

        public void beginTest() {

            int[,] crossMatrix = new int[classes.Count, classes.Count];
            CrossValidation cxv = new CrossValidation();
            double sum;
            ClsWt myClass;
            ClsWt prdClass = new ClsWt() ;
            var classArray = new List<ClsWt>();

            foreach (List<string> te in test)
            {
                sum = 0;
                prdClass.cls = null;
                prdClass.weight = 0;
                
                classArray.Clear();

                for (int i = 0; i < clsNwet.Count; i++) {

                    clsNwet[clsNwet.Keys.ElementAt(i)] = 0.0;
                }
                foreach (List<string> tr in train)
                {
                    myClass = new ClsWt();
                    myClass.cls = tr[8];
                    myClass.weight = 1 / getDistance(te, tr);

                    if (classArray.Count == 0) classArray.Add(myClass);
                    else
                    {
                        for (int i = 0; i < classArray.Count; i++)
                        {

                            if (classArray[i].weight < myClass.weight)
                            {

                                if (classArray.Count == k) 
                                    classArray.RemoveAt(k - 1);

                                classArray.Insert(i, myClass);
                            }
                        }
                    }

                }

                try
                {
                    for (int i = 0; i < k; i++)
                    {
                        sum += classArray[i].weight;
                    }


                    for (int i = 0; i < k; i++)
                    {
                        classArray[i].weight /= sum;
                    }
                    bool found;

                    for (int i = 0; i < k; i++)
                    {

                        found = false;

                        for (int j = 0; j < clsNwet.Count; j++)
                        {

                            if (String.Equals(clsNwet.Keys.ElementAt(j), classArray[i].cls))
                            {

                                found = true;
                                clsNwet[clsNwet.Keys.ElementAt(j)] += classArray[i].weight;
                            }
                            if (found) break;
                        }
                    }
                }
                catch (System.ArgumentOutOfRangeException e)
                {
                    Console.WriteLine("");
                }

                for (int i = 0; i < clsNwet.Count; i++) {
                    if (clsNwet[clsNwet.Keys.ElementAt(i)] > prdClass.weight) {
                        prdClass.cls = clsNwet.Keys.ElementAt(i);
                        prdClass.weight = clsNwet[clsNwet.Keys.ElementAt(i)];
                    }
                }
                Console.WriteLine("Predicted Class for {0} is {1} whereas it is {2}", te[0], prdClass.cls, te[8]);
                
                int ci = -1, cj = -1;
                bool bi = false, bj = false;

                foreach (var setItem in classes) {

                    if(!bi)
                        ci++;
                    if(!bj)
                        cj++;

                    if (setItem.Equals(prdClass.cls)) bi = true;

                    if (setItem.Equals(te[8])) bj = true;
                        
                }
                if (String.Equals(prdClass.cls, te[8]))
                {
                    crossMatrix[ci, ci]++;
                }
                else {

                    crossMatrix[ci, cj]++;
                }
                  //  accuracy++;
            }
            double accuracy = Math.Round(cxv.getAccuracy(crossMatrix,test.Count),5);
            Console.WriteLine("\nAccuracy: {0}%", accuracy * 100);
            double precision = Math.Round(cxv.getPrecision(crossMatrix, classes.Count), 5);
            Console.WriteLine("\nPrecision: {0}%", precision * 100);
            double recall = Math.Round(cxv.getRecall(crossMatrix, classes.Count), 5);
            Console.WriteLine("\nRecall: {0}%", recall * 100);
            double f1 = Math.Round(cxv.getF1(crossMatrix, classes.Count), 5);
            Console.WriteLine("\nf-measure: {0}%", f1 * 100);
          
            //return accuracy;
            
        }

        public double getDistance(List<string> testPoint, List<string> knownPoint) {

            double sqSum=0;

            for (int i = 1; i < testPoint.Count - 1; i++)
                sqSum += Math.Pow((Convert.ToDouble(testPoint[i]) - Convert.ToDouble(knownPoint[i])), 2);

            return Math.Sqrt(sqSum);
        }
    }
}
