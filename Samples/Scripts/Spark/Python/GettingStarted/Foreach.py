from pyspark import SparkContext
import re 

# Read the input file 
lines = sc.textFile("/Data/NASA_Access_Log")

# Method to split the input line and returns the word
def getKey(line, index):
	# Replace '- ' with empty value, so input lines are separated only by white space
	str = line.replace("- "," ")
	# Replace multiple spaces by single space
	str = ' '.join(str.split())
	# Split the input line by white space unless text enclosed with in double quotes and '[]' and stores the each field as string array
	str = re.findall('\[[^\]]*\]|\"[^\"]*\"|\S+', str)
	return str[index]

# Creates the Key-Value pair with the Internet Address as the key and the corresponding HTTP code as the value
pairs = lines.map(lambda s: (getKey(s,1), getKey(s,3)))
# Saves the created Key-Value pair in text file
pairs.saveAsTextFile("/Data/Output/ForeachPython"); 