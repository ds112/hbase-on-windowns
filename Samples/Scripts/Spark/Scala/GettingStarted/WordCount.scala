//WordCount: Normal wordcount program from input 'WarPeace.txt'
val textFile = sc.textFile("/Data/WarPeace.txt")
	
val counts = textFile.flatMap(line => line.split(" ")).map(word => (word, 1)).reduceByKey(_ + _)

counts.collect().foreach(println)