from __future__ import print_function
import sys
from pyspark.context import SparkContext
from pyspark.mllib.tree import RandomForest
from pyspark.mllib.util import MLUtils

def testClassification(trainingData, testData):    
	# Train a RandomForest model.    
	#  Empty categoricalFeaturesInfo indicates all features are continuous.    
	#  Note: Use larger numTrees in practice.    
	#  Setting featureSubsetStrategy="auto" lets the algorithm choose.    
	model = RandomForest.trainClassifier(trainingData, numClasses=2,categoricalFeaturesInfo={},numTrees=3, featureSubsetStrategy="auto",impurity='gini', maxDepth=4, maxBins=32)
	# Evaluate model on test instances and compute test error    
	predictions = model.predict(testData.map(lambda x: x.features))
	labelsAndPredictions = testData.map(lambda lp: lp.label).zip(predictions)
	testErr = labelsAndPredictions.filter(lambda v_p: v_p[0] != v_p[1]).count()/float(testData.count())
	print('Test Error = ' + str(testErr))
	print('Learned classification forest model:')
	print(model.toDebugString())

def testRegression(trainingData, testData):    
	# Train a RandomForest model.    
	#  Empty categoricalFeaturesInfo indicates all features are continuous.    
	#  Note: Use larger numTrees in practice.    
	#  Setting featureSubsetStrategy="auto" lets the algorithm choose.    
	model = RandomForest.trainRegressor(trainingData, categoricalFeaturesInfo={},numTrees=3, featureSubsetStrategy="auto",impurity='variance', maxDepth=4, maxBins=32)
	# Evaluate model on test instances and compute test error    
	predictions = model.predict(testData.map(lambda x: x.features))
	labelsAndPredictions = testData.map(lambda lp: lp.label).zip(predictions)
	testMSE = labelsAndPredictions.map(lambda v_p1: (v_p1[0] - v_p1[1]) * (v_p1[0] - v_p1[1])).sum() / float(testData.count())
	print('Test Mean Squared Error = ' + str(testMSE))
	print('Learned regression forest model:')
	print(model.toDebugString())

# Load and parse the data file into an RDD of LabeledPoint.    
data = MLUtils.loadLibSVMFile(sc, '/Data/Spark/MLLib/Sample_LibSVM_Data.txt')
# Split the data into training and test sets (30% held out for testing)    
(trainingData, testData) = data.randomSplit([0.7, 0.3])    

print('\nRunning example of classification using RandomForest\n')    
testClassification(trainingData, testData)    

print('\nRunning example of regression using RandomForest\n')    
testRegression(trainingData, testData)