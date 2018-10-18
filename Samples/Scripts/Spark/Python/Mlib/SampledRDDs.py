from __future__ import print_function
import sys
from pyspark import SparkContext
from pyspark.mllib.util import MLUtils

datapath = '/Data/Spark/MLLib/Sample_Binary_Classification_Data.txt'

# fraction of data to sample
fraction = 0.1
examples = MLUtils.loadLibSVMFile(sc, datapath)

numExamples = examples.count()

if numExamples == 0:
	print("Error: Data file had no samples to load.", file=sys.stderr)
	exit(1)

print('Loaded data with %d examples from file: %s' % (numExamples, datapath))

# Example: RDD.sample() and RDD.takeSample()
expectedSampleSize = int(numExamples * fraction)

print('Sampling RDD using fraction %g.  Expected sample size = %d.'% (fraction, expectedSampleSize))

sampledRDD = examples.sample(withReplacement=True, fraction=fraction)

print('  RDD.sample(): sample has %d examples' % sampledRDD.count())

sampledArray = examples.takeSample(withReplacement=True, num=expectedSampleSize)

print('  RDD.takeSample(): sample has %d examples' % len(sampledArray))
print()

# Example: RDD.sampleByKey()
keyedRDD = examples.map(lambda lp: (int(lp.label), lp.features))

print('  Keyed data using label (Int) as key ==> Orig')
#  Count examples per label in original data.

keyCountsA = keyedRDD.countByKey()
#  Subsample, and count examples per label in sampled data.

fractions = {}

for k in keyCountsA.keys():
	fractions[k] = fraction

sampledByKeyRDD = keyedRDD.sampleByKey(withReplacement=True, fractions=fractions)

keyCountsB = sampledByKeyRDD.countByKey()

sizeB = sum(keyCountsB.values())

print('  Sampled %d examples using approximate stratified sampling (by label). ==> Sample'% sizeB)

#  Compare samples
print('   \tFractions of examples with key')

print('Key\tOrig\tSample')

for k in sorted(keyCountsA.keys()):
	fracA = keyCountsA[k] / float(numExamples)
	if sizeB != 0:
		fracB = keyCountsB.get(k, 0) / float(sizeB)
	else:
		fracB = 0
	print('%d\t%g\t%g' % (k, fracA, fracB))
