using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NaiveBayesClassifier
{
     class PreProcessor
     {
          SortedSet<string> vocabulary = new SortedSet<string>();
          SortedSet<string> stopList = new SortedSet<string>();
          List<int[]> features = new List<int[]>();

          public PreProcessor(string vocabularyFileName, string stopListFileName)
          {
               using (var stopListFile = new StreamReader(stopListFileName))
               {
                    string line;
                    while ((line = stopListFile.ReadLine()) != null)
                    {
                         stopList.Add(line.Trim().ToLower());
                    }
               }

               using (var sourceFile = new StreamReader(vocabularyFileName))
               {
                    string line;
                    while((line = sourceFile.ReadLine()) != null)
                    {
                         var words = line.ToLower().Trim().Split(' ');
                         foreach (var word in words)
                         {
                              if(!stopList.Contains(word.Trim()))
                              {
                                   //SortedSets take care of duplicates
                                   vocabulary.Add(word.Trim());
                              }
                         }
                    }
               }
          }

          public List<int[]> GenerateAndPrintFeatures(string trainingDataFileName, string trainingLabelFileName, string featureOutputFile)
          {
               var generator = new FeatureGenerator(vocabulary.ToList<string>());
               features = generator.GenerateAllFeatures(trainingDataFileName, trainingLabelFileName);

               //print
               using (var outFile = new StreamWriter(featureOutputFile))
               {
                    foreach (var word in vocabulary)
                    {
                         outFile.Write("{0}, ", word);
                    }
                    outFile.WriteLine();
                    foreach (var feature in features)
                    {
                         foreach (var value in feature)
                         {
                              outFile.Write("{0}, ", value);
                         }
                         outFile.WriteLine();
                    }
               }
               return features;
          }
     }
}
