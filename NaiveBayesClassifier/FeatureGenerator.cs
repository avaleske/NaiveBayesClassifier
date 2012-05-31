using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NaiveBayesClassifier
{
     class FeatureGenerator
     {
          List<string> vocabulary;

          public FeatureGenerator(List<string> vocabulary)
          {
               this.vocabulary = vocabulary;
          }

          public int[] GenerateFeature(string phrase)
          {
               var parts = phrase.Trim().ToLower().Split(' ');
               var union = vocabulary.Intersect(parts);
               int[] feature = new int[vocabulary.Count + 1]; //+1 for class variable
               foreach (var word in union.OrderBy(word => word))
               {
                    int index;
                    if ((index = vocabulary.IndexOf(word)) != -1)
                    {
                         feature[index] = 1;
                    }
               }
               return feature;
          }
          
          public List<int[]> GenerateAllFeatures(string dataFileName)
          {
               return GenerateAllFeatures(dataFileName, null);
          }

          public List<int[]> GenerateAllFeatures(string dataFileName, string labelFileName)
          {
               List<int[]> features = new List<int[]>();
               using (var dataFile = new StreamReader(dataFileName))
               {
                    string line;
                    while ((line = dataFile.ReadLine()) != null)
                    {
                         features.Add(GenerateFeature(line));
                    }
               }
               if (labelFileName != null) //add class variable if provided
               {
                    using(var labelFile = new StreamReader(labelFileName))
                    {
                         string line;
                         int index = 0;
                         while ((line = labelFile.ReadLine()) != null)
                         {
                              //last index of every feature is the class label
                              features[index][vocabulary.Count] = int.Parse(line.Trim());
                              index++;
                         }
                    }
               }
               return features;
          }
     }
}
