from __future__ import print_function
import pprint
import sys
from pyspark import SparkContext
from pyspark.ml.classification import LogisticRegression
from pyspark.ml.linalg import DenseVector
from pyspark.mllib.regression import LabeledPoint
from pyspark.sql import Row, SparkSession

if __name__ == "__main__":
    spark = SparkSession \
        .builder \
        .appName("SimpleParamsExample") \
        .getOrCreate()

# prepare training data.
# We create an RDD of LabeledPoints and convert them into a DataFrame.
# Spark DataFrames can automatically infer the schema from named tuples
# and LabeledPoint implements __reduce__ to behave like a named tuple.
training = spark.createDataFrame([Row(label=1.0, features=DenseVector([0.0, 1.1, 0.1])), Row(label=0.0, features=DenseVector([2.0, 1.0, -1.0])),Row(label=0.0, features=DenseVector([2.0, 1.3, 1.0])), Row(label=1.0, features=DenseVector([0.0, 1.2, -0.5]))])

# Create a LogisticRegression instance with maxIter = 10.
# This instance is an Estimator.
lr = LogisticRegression(maxIter=10)

# Print out the parameters, documentation, and any default values.
print("LogisticRegression parameters:\n" + lr.explainParams() + "\n")

# We may also set parameters using setter methods.
lr.setRegParam(0.01)

# Learn a LogisticRegression model.  This uses the parameters stored in lr.
model1 = lr.fit(training)

# Since model1 is a Model (i.e., a Transformer produced by an Estimator),
# we can view the parameters it used during fit().
# This prints the parameter (name: value) pairs, where names are unique IDs for this
# LogisticRegression instance.
print("Model 1 was fit using parameters:\n")
pprint.pprint(model1.extractParamMap())

# We may alternatively specify parameters using a parameter map.
# paramMap overrides all lr parameters set earlier.
paramMap = {lr.maxIter: 20, lr.threshold: 0.55, lr.probabilityCol: "myProbability"}

# Now learn a new model using the new parameters.
model2 = lr.fit(training, paramMap)
print("Model 2 was fit using parameters:\n")
pprint.pprint(model2.extractParamMap())

# prepare test data.
test = spark.createDataFrame([Row(label=1.0, features=DenseVector([-1.0, 1.5, 1.3])),Row(label=0.0, features=DenseVector([3.0, 2.0, -0.1])),Row(label=0.0, features=DenseVector([0.0, 2.2, -1.5]))])

# Make predictions on test data using the Transformer.transform() method.
# LogisticRegressionModel.transform will only use the 'features' column.
# Note that model2.transform() outputs a 'myProbability' column instead of the usual
# 'probability' column since we renamed the lr.probabilityCol parameter previously.
result = model2.transform(test).select("features", "label", "myProbability", "prediction").collect()

for row in result:
	print("features=%s,label=%s -> prob=%s, prediction=%s" % (row.features, row.label, row.myProbability, row.prediction))
