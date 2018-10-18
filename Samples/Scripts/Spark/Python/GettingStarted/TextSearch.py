from pyspark import SparkContext

# Read the input file 
text_file = sc.textFile("/Data/NASA_Access_Log")

# Filters the input lines that contains the Internet Address "lya.colorado.edu"
errors = text_file.filter(lambda line: "lya.colorado.edu" in line)

# Saves the filtered lines in text file
errors.saveAsTextFile("/Data/Output/TextSearchPython")