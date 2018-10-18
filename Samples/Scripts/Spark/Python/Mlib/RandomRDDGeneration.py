from __future__ import print_function
import sys
from pyspark import SparkContext
from pyspark.mllib.random import RandomRDDs

# number of examples to generate    
numExamples = 10000  
	
# fraction of data to sample   
fraction = 0.1  
  
# Example: RandomRDDs.normalRDD
normalRDD = RandomRDDs.normalRDD(sc, numExamples)
    
print('Generated RDD of %d examples sampled from the standard normal distribution'% normalRDD.count())   
print('  First 5 samples:')
    
for sample in normalRDD.take(5):
        
	print('    ' + str(sample))
  

  
# Example: RandomRDDs.normalVectorRDD   
normalVectorRDD = RandomRDDs.normalVectorRDD(sc, numRows=numExamples, numCols=2)
    
print('Generated RDD of %d examples of length-2 vectors.' % normalVectorRDD.count()) 
print('  First 5 samples:')
    
for sample in normalVectorRDD.take(5):
    
	print('    ' + str(sample))
