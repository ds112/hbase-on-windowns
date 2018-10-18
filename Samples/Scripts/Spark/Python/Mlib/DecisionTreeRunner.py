from __future__ import print_function
import numpy
import os
import sys
from operator import add
from pyspark import SparkContext
from pyspark.mllib.regression import LabeledPoint
from pyspark.mllib.tree import DecisionTree
from pyspark.mllib.util import MLUtils

def getAccuracy(dtModel, data):
	seqOp = (lambda acc, x: acc + (x[0] == x[1]))
	predictions = dtModel.predict(data.map(lambda x: x.features))
	truth = data.map(lambda p: p.label)
	trainCorrect = predictions.zip(truth).aggregate(0, seqOp, add)
	if data.count() == 0:
		return 0
	return trainCorrect / (0.0 + data.count())

def getMSE(dtModel, data):
	seqOp = (lambda acc, x: acc + numpy.square(x[0] - x[1]))
	predictions = dtModel.predict(data.map(lambda x: x.features))
	truth = data.map(lambda p: p.label)
	trainMSE = predictions.zip(truth).aggregate(0, seqOp, add)
	if data.count() == 0:
		return 0
	return trainMSE / (0.0 + data.count())

def reindexClassLabels(data):
	# classCounts: class --> # examples in class
	classCounts = data.map(lambda x: x.label).countByValue()
	numExamples = sum(classCounts.values())
	sortedClasses = sorted(classCounts.keys())
	numClasses = len(classCounts)
	# origToNewLabels: class --> index in 0,...,numClasses-1
	if (numClasses < 2):
		print("Dataset for classification should have at least 2 classes.The given dataset had only %d classes." % numClasses, file=sys.stderr)
		exit(1)
	origToNewLabels = dict([(sortedClasses[i], i) for i in range(0, numClasses)])
	print("numClasses = %d" % numClasses)
	print("Per-class example fractions, counts:")
	print("Class\tFrac\tCount")
	for c in sortedClasses:
		frac = classCounts[c] / (numExamples + 0.0)
		print("%g\t%g\t%d" % (c, frac, classCounts[c]))
	if (sortedClasses[0] == 0 and sortedClasses[-1] == numClasses - 1):
		return (data, origToNewLabels)
	else:
		reindexedData = data.map(lambda x: LabeledPoint(origToNewLabels[x.label], x.features))
		return (reindexedData, origToNewLabels)

# Load data.
dataPath = '/Data/Spark/MLLib/Sample_LibSVM_Data.txt'
points = MLUtils.loadLibSVMFile(sc, dataPath)

# Re-index class labels if needed.
(reindexedData, origToNewLabels) = reindexClassLabels(points)
numClasses = len(origToNewLabels)

# Train a classifier.
categoricalFeaturesInfo = {}  # no categorical features
model = DecisionTree.trainClassifier(reindexedData, numClasses=numClasses,categoricalFeaturesInfo=categoricalFeaturesInfo)

# Print learned tree and stats.
print("Trained DecisionTree for classification:")
print("  Model numNodes: %d" % model.numNodes())
print("  Model depth: %d" % model.depth())
print("  Training accuracy: %g" % getAccuracy(model, reindexedData))
  
if model.numNodes() < 20:
	print(model.toDebugString())
else:
	print(model)
