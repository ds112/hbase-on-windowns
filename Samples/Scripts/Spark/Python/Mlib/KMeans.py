from __future__ import print_function
import sys
import numpy as np
from pyspark import SparkContext
from pyspark.mllib.clustering import KMeans
def parseVector(line):    
	return np.array([float(x) for x in line.split(' ')])

lines = sc.textFile("/Data/Spark/MLLib/KMeans_Data.txt") 
data = lines.map(parseVector)  
k = 2   
model = KMeans.train(data, k)
    
print("Final centers: " + str(model.clusterCenters))   
print("Total Cost: " + str(model.computeCost(data)))