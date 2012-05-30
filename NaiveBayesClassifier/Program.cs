using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NaiveBayesClassifier
{
     class Program
     {
          static void Main(string[] args)
          {
               string stoplist;
               string trainingData;
               string trainingLabel;
               string testingData;
               string testingLabel;

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
          }

          private static void exit()
          {
               Console.WriteLine("Press any key to close.");
               Console.ReadKey();
               System.Environment.Exit(0);
          }
     }
}
