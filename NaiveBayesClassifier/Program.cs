using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NaiveBayesClassifier
{
     class Program
     {    
          private const string OUTPUT_FILE = "output.csv";
          static string stoplist;
          static string trainingData;
          static string trainingLabel;
          static string testingData;
          static string testingLabel;

          static void Main(string[] args)
          {
               try
               {
                    foreach (var file in args)
                    {
                         if(!File.Exists(file))
                              throw new FileNotFoundException(string.Format("The file {0} could not be found.", file));
                    }

                    stoplist = args[0];
                    trainingData = args[1];
                    trainingLabel = args[2];
                    testingData = args[3];
                    testingLabel = args[4];
               }
               catch (Exception e)
               {

                    Console.WriteLine("There was a problem reading one or more of the input files.");
                    Console.WriteLine("Double check your file paths and try again.");
                    Console.WriteLine(e.Message);
                    exit();
               }

               var preProcessor = new PreProcessor(trainingData, stoplist);
               var trainingFeatures = preProcessor.GenerateAndPrintFeatures(trainingData, trainingLabel, OUTPUT_FILE);

               var classifier = new BayesClassifier(trainingFeatures);
               var testedFeatures = classifier.ClassifyFeatureSet(preProcessor.GetVocabulary.ToList<string>(), testingData);

               Console.WriteLine("It was {0}% accurate", classifier.CheckClassificationAccuracy(testedFeatures, testingLabel));
               exit();
          }

          private static void exit()
          {
               Console.WriteLine("Press any key to close.");
               Console.ReadKey();
               System.Environment.Exit(0);
          }
     }
}
