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
          int featureCount;
          int trueClassFeatureCount;
          int parameterCount;

          public bool IsTrained { get { return trained; } }

          public BayesClassifier(List<int[]> trainingFeatures)
          {
               featureCount = trainingFeatures.Count;
               parameterCount = trainingFeatures[0].Length - 1;
               int classIndex = parameterCount;
               //count where last column of feature, if fortune or not, is 1.
               trueClassFeatureCount = trainingFeatures.Count(f => f[classIndex] == 1);
               learnedData = new double[2, parameterCount + 1];
               
               //Add 1 to act as a Dirichlet Prior
               learnedData[1, classIndex] = (trueClassFeatureCount + 1) / (double)featureCount;
               learnedData[0, classIndex] = (featureCount - trueClassFeatureCount + 1) / (double)featureCount;

               for (int i = 0; i < parameterCount; i++)
               {
                    //i is the parameter index, parameterCount - 1 is the class variable index
                    int trueCount = trainingFeatures.Count(f => (f[i] == 1) && (f[classIndex] == 1));
                    int falseCount = trainingFeatures.Count(f => (f[i] == 1) && (f[classIndex] == 0));
                    
                    learnedData[0, i] = (falseCount + 1) / (double)(featureCount - trueClassFeatureCount);
                    learnedData[1, i] = (trueCount + 1) / (double)trueClassFeatureCount;
               }
          }
     }
}
