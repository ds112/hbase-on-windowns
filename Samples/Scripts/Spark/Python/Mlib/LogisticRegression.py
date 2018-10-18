from __future__ import print_function
import sys
from pyspark import SparkContext
from pyspark.mllib.regression import LabeledPoint
from pyspark.mllib.classification import LogisticRegressionWithLBFGS

def parsePoint(line):
	values = [float(s) for s in line.split(' ')]
	# Convert -1 labels to 0 for MLlib  
	if values[0] == -1:
		values[0] = 0
	return LabeledPoint(values[0], values[1:])

points = sc.textFile("/Data/Spark/MLLib/LR_Data.txt").map(parsePoint)

iterations = 10

model = LogisticRegressionWithLBFGS.train(points, iterations)

print("Final weights: " + str(model.weights))
print("Final intercept: " + str(model.intercept))