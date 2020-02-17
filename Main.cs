using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using W_KNN;

public class Main
{
    string[] entry;
    List<List<string>> data = new List<List<string>>();
    List<List<string>> testData = new List<List<string>>();
    List<List<string>> dummyData = new List<List<string>>();
    SortedSet<string> classes = new SortedSet<string>();
    int fold, k;
    public Main()
    {
        Console.WriteLine("Enter fold count & Nearest Neighbour count: ");
        fold = Convert.ToInt32(Console.ReadLine());
        k = Convert.ToInt32(Console.ReadLine());
        
        arrangeData();
        run();
        new Predictor(data, testData, k, classes);

    }

    public void arrangeData()
    {
        string path = @"C:\projectCodes\.NET\W_knn\W_KNN\ecoli.data", line;

        StreamReader reader = new StreamReader(path);

        while ((line = reader.ReadLine()) != null)
        {

            line = Regex.Replace(line, @"\s+", " ");

            entry = line.Split(null);
            
            classes.Add(entry[8]);

            data.Add(entry.ToList());

            Array.Clear(entry, 0, entry.Length);

        }
    }

    public void printMe() {

        foreach (List<string> s in data)
        {
            foreach (string j in s)
            {
                Console.Write(j + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
        for (int i = 0; i < testData.Count; i++)
        {
            for (int j = 0; j < testData[i].Count; j++)
            {
                Console.Write(testData[i][j]);
            }
            Console.WriteLine();
        }

    }
    public void run() {

        Random rnd=new Random();
        int ind, foldElem = data.Count/fold, round=1;
        double myAccuracy = 0;

        for (int i = 0; i < data.Count; i++) {
            ind = rnd.Next(1, data.Count - 1);
            
            data.Insert(0,data[ind]);
            data.RemoveAt(ind + 1);
        }

        for (int run = 0; run < fold; run++) {

            foreach (List<string> str in data) dummyData.Add(str);

            for (int i = run * fold; i < (run * fold) + foldElem; i++) {

                testData.Add(dummyData[i]);
                dummyData.RemoveAt(i);
            }
            Predictor p=new Predictor(dummyData, testData, k, classes);
            
            Console.WriteLine("\n"+round++ +":");
            
            myAccuracy += p.beginTest();
            
            testData.Clear();
            
            dummyData.Clear();
        }
        Console.WriteLine("Net Accuracy: {0}%", Math.Round(myAccuracy/fold,5)*100);
       
    }
} 

