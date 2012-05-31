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
          private double[] learnedData;
          int featureCount;
          int parameterCount;

          public bool IsTrained { get { return trained; } }

          public BayesClassifier(List<int[]> trainingFeatures)
          {
               featureCount = trainingFeatures.Count;
               parameterCount = trainingFeatures[0].Length;
               learnedData = new double[parameterCount];
               
               //count where last column, if fortune or not, is 1, and add 1 to act as a Dirichlet Prior.
               learnedData[parameterCount - 1] = (trainingFeatures.Count(f => f[parameterCount - 1] == 1) + 1) / (double)featureCount;
          }
     }
}
