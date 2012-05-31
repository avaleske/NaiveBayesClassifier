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
               int featureCount = trainingFeatures.Count;
               parameterCount = trainingFeatures[0].Length - 1;
               classIndex = parameterCount;
               //count where last column of feature, if fortune or not, is 1.
               int trueClassFeatureCount = trainingFeatures.Count(f => f[classIndex] == 1);
               learnedData = new double[2, parameterCount + 1];
               
               //Add 1 to act as a Dirichlet Prior
               learnedData[1, classIndex] = (trueClassFeatureCount + 1) / (double)featureCount;
               learnedData[0, classIndex] = (featureCount - trueClassFeatureCount + 1) / (double)featureCount;

               for (int i = 0; i < parameterCount; i++)
               {
                    int trueCount = trainingFeatures.Count(f => (f[i] == 1) && (f[classIndex] == 1));
                    int falseCount = trainingFeatures.Count(f => (f[i] == 1) && (f[classIndex] == 0));
                    
                    learnedData[0, i] = (falseCount + 1) / (double)(featureCount - trueClassFeatureCount);
                    learnedData[1, i] = (trueCount + 1) / (double)trueClassFeatureCount;
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
               var features = generator.GenerateAllFeatures(testDataFileName);

               foreach (var feature in features)
               {
                    double fortune = Math.Log(learnedData[1,classIndex]);
                    double notFortune = Math.Log(learnedData[0, classIndex]);
                    
                    for (int i = 0; i < parameterCount; i++)
                    {
                         fortune += Math.Log((feature[i] == 1) ? learnedData[1,i] : 1 - learnedData[1,i]);
                         notFortune += Math.Log((feature[i] == 1) ? learnedData[0, i] : 1 - learnedData[0, i]);
                    }
                    feature[classIndex] = (fortune > notFortune) ? 1 : 0;
               }
               return features;
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
