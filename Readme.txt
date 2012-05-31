Austin Valeske 5/31/12
This is a Naive Bayes Classifier for my Intro AI class.
It classifies a string from a fortune cookie as either a fortune or a wise saying, given an input of already classified phrases.

As input, it takes <stoplist file> <training data file> <training label file> <testing data file> <testing label file>
Examples of these files are in the data folder of the project.

It also doesn't do any checking of the files to ensure they're in the correct format, it's more of a proof of concept than anything. It's not terribly well encapsulated, either, as it passes the a reference to the same vocabulary around for a bit...

After a vocabulary is developed based on the training data sans the stopwords, each phrase is converted into a feature, which is an array where each index corresponds to a word in the vocabulary, and a 1 at that index means the phrase has that word. The final index in the feature array is the class varaible, which is the label corresponding to whether or not the phrase is a fortune.

The training data is converted to features, and the in a seperate array the probabilty of each word appearing given that a phrase is or is not a fortune is calculated and stored. I use Dirichlet Priors because otherwise, when we attempt to add up probabilities later, we'll have zeros as some of the probabilities instead of each word being equally likely in the absence of data.

The words to be classified are then also converted to features, and the logs of each of the probabilities that a word does or does not appear, given that it is or is not a fortune, are added up. This is preferable to multipling the probabilities together, as floating points are unstable for small values. The larger of the two, fortune or not fortune, is determined to be the more likely result.

So yeah, simple project, I haven't yet taken the time to clean it up much, though.