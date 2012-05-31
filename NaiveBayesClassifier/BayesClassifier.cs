using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NaiveBayesClassifier
{
     class BayesClassifier
     {
          private bool trained = false;
          private double[,] learnedData; // [0,i] is class == false, [1,i] is true.
          int parameterCount;
          int classIndex;

          public bool IsTrained { get { return trained; } }

          public BayesClassifier(List<int[]> trainingFeatures)
          {
               //constructor trains the classifier with probabilities of each parameter, given the class varaible
               int featureCount = trainingFeatures.Count;
               parameterCount = trainingFeatures[0].Length - 1;
               classIndex = parameterCount;

               //count where class variable is 1.
               int classTrueFeatureCount = trainingFeatures.Count(f => f[classIndex] == 1);
               learnedData = new double[2, parameterCount + 1];
               
               //Add 1 to act as a Dirichlet Prior
               learnedData[1, classIndex] = (classTrueFeatureCount + 1) / (double)featureCount;
               learnedData[0, classIndex] = (featureCount - classTrueFeatureCount + 1) / (double)featureCount;

               for (int i = 0; i < parameterCount; i++)
               {
                    //count where parameter == true and class == true or parameter == true and class == false
                    //and divide by total with class == true or with class == false, respectively
                    int trueCount = trainingFeatures.Count(f => (f[i] == 1) && (f[classIndex] == 1));
                    int falseCount = trainingFeatures.Count(f => (f[i] == 1) && (f[classIndex] == 0));
                    
                    learnedData[0, i] = (falseCount + 1) / (double)(featureCount - classTrueFeatureCount);
                    learnedData[1, i] = (trueCount + 1) / (double)classTrueFeatureCount;
               }
               trained = true;
          }

          public List<int[]> ClassifyFeatureSet(List<string> vocabulary, string testDataFileName)
          {
               if (trained == false)
               {
                    throw new Exception("The classifier must be trained before attempting to classify a feature set.");
               }

               var generator = new FeatureGenerator(vocabulary);
               var testFeatures = generator.GenerateAllFeatures(testDataFileName);

               //then fill in class varaible in new set of test features
               foreach (var feature in testFeatures)
               {
                    double fortune = Math.Log(learnedData[1,classIndex]);
                    double notFortune = Math.Log(learnedData[0, classIndex]);
                    
                    //P(class | p1, p2, ... pn) = aP(class)P(p1|class)P(p2|class)... a is ignored as both
                    //the fortune and the notFortune would have it.
                    //Using sums and logs to account for instability of small floating points
                    //If we want the probability a paramater's false, subract that it's true from 1
                    for (int i = 0; i < parameterCount; i++)
                    {
                         fortune += Math.Log((feature[i] == 1) ? learnedData[1,i] : 1 - learnedData[1,i]);
                         notFortune += Math.Log((feature[i] == 1) ? learnedData[0, i] : 1 - learnedData[0, i]);
                    }
                    feature[classIndex] = (fortune > notFortune) ? 1 : 0;
               }
               return testFeatures;
          }

          public double CheckClassificationAccuracy(List<int[]> features, string testLabelFileName)
          {
               double percentCorrect = 0;
               using (var labelFile = new StreamReader(testLabelFileName))
               {
                    string line;
                    int index = 0;
                    int numCorrect = 0;
                    while ((line = labelFile.ReadLine()) != null && index < features.Count)
                    {
                         if (features[index][classIndex] == int.Parse(line.Trim()))
                         {
                              numCorrect++;
                         }
                         index++;
                    }
                    percentCorrect = (numCorrect / (double)features.Count) * 100;
               }
               return percentCorrect;
          }
     }
}
