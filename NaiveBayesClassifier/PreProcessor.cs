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

          public void GenerateVocabulary(string sourceFileName, string stopListFileName)
          {
               
               using (var stopListFile = new StreamReader(stopListFileName))
               {
                    string line;
                    while ((line = stopListFile.ReadLine()) != null)
                    {
                         stopList.Add(line.Trim().ToLower());
                    }
               }

               using (var sourceFile = new StreamReader(sourceFileName))
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
     }
}
