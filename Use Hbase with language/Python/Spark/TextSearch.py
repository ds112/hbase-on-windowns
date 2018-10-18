from pyspark import SparkContext, SparkConf

conf = SparkConf()
# To run this file with 'spark-submit' set master to 'yarn-cluster'
# conf.setMaster("local")
conf.setAppName("TextSearch")
# Creates SparkContext for the Main entry point of Spark functionality
sc = SparkContext(conf=conf)

# Read the input file
# For remote cluster set remote host_name:port instead of localhost:9000
text_file = sc.textFile("/Data/NASA_Access_Log")

# Filters the input lines that contains the Internet Address "lya.colorado.edu"
errors = text_file.filter(lambda line: "lya.colorado.edu" in line)
# Saves the filtered lines in text file
errors.saveAsTextFile("/Data/Output/TextSearch")