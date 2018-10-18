from __future__ import print_function
import sys
from operator import add
from pyspark import SparkContext

lines = sc.textFile("/Data/WarPeace.txt", 1)    
counts = lines.flatMap(lambda x: x.split(' ')).map(lambda x: (x, 1)).reduceByKey(add)    
output = counts.collect()
for (word, count) in output:
	print("%s: %i" % (word.encode("UTF-8"), count))
