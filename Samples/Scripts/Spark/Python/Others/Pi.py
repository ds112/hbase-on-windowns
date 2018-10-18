from __future__ import print_function
import sys
from random import random
from operator import add
from pyspark import SparkContext

def f(_):
	x = random() * 2 - 1
	y = random() * 2 - 1
	return 1 if x ** 2 + y ** 2 < 1 else 0

partitions = 2
n = 100000 * partitions
count = sc.parallelize(range(1, n + 1), partitions).map(f).reduce(add)
print("Pi is roughly %f" % (4.0 * count / n))