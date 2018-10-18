from __future__ import print_function
import sys
from pyspark import SparkContext
from pyspark.mllib.regression import LabeledPoint
from pyspark.mllib.stat import Statistics
from pyspark.mllib.util import MLUtils

filepath = '/Data/Spark/MLLib/Sample_Linear_Regression_Data.txt'
corrType = 'pearson'
points = MLUtils.loadLibSVMFile(sc, filepath).map(lambda lp: LabeledPoint(lp.label, lp.features.toArray()))


print('Summary of data file: ' + filepath)
print('%d data points' % points.count())

# Statistics (correlations)

print('Correlation (%s) between label and each feature' % corrType)
print('Feature\tCorrelation')

numFeatures = points.take(1)[0].features.size
labelRDD = points.map(lambda lp: lp.label)

for i in range(numFeatures):
    
	featureRDD = points.map(lambda lp: lp.features[i]) 
	corr = Statistics.corr(labelRDD, featureRDD, corrType)
    
	print('%d\t%g' % (i, corr))
