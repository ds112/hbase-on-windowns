//Sorting: Sorting and counts the internet address from input 'NASA_Access_Log'
val input = sc.textFile("/Data/NASA_Access_Log")
	
val pairs = input.map(line => (line.split(" (?=([^\"]*\"[^\"]*\")*[^\"]*$)+(?![^\\[]*\\])")(0), 1))
	
val counts = pairs.reduceByKey((a, b) => a + b)
	
val sortedValue = counts.sortByKey()

sortedValue.collect().foreach(println)