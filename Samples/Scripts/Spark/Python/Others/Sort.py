from __future__ import print_function
import sys
from pyspark import SparkContext

lines = sc.textFile("/Data/Spark/Resources/People.txt")
sortedCount = lines.map(lambda x: x.split(' ')[1]).map(lambda x: (int(x), 1)).sortByKey(lambda x: x)

# This is just a demo on how to bring all the sorted data back to a single node.
# In reality, we wouldn't want to collect all the data to the driver node.
output = sortedCount.collect()

for (num, unitcount) in output:
	print(num)
