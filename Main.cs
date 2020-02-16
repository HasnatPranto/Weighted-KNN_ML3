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
    SortedSet<string> classes = new SortedSet<string>();
    int fold, k;
    public Main()
    {
        Console.WriteLine("Enter fold count & Nearest Neighbour count: ");
        fold = Convert.ToInt32(Console.ReadLine());
        k = Convert.ToInt32(Console.ReadLine());
        
        arrangeData();
        setTrainTest();
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
    public void setTrainTest() {

        Random rnd=new Random();
        int ind, lim = data.Count/fold;
    
        for (int i = 0; i < lim; i++) {

            ind = rnd.Next(1, data.Count);
            
            testData.Add(data[ind]);
            
            data.RemoveAt(ind);
        }
    }
} 

