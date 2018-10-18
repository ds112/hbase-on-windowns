//Foreach: For every internet address(1), get the HTTP reply code(4) and print it as keyvalue pair from input 'NASA_Access_Log'
val input = sc.textFile("/Data/NASA_Access_Log")
	
val regex = "- ".r
	
val result = input.map(line => regex.replaceAllIn(line,""))
	
val pairs = result.map(line => (line.split(" (?=([^\"]*\"[^\"]*\")*[^\"]*$)+(?![^\\[]*\\])")(1),line.split(" (?=([^\"]*\"[^\"]*\")*[^\"]*$)+(?![^\\[]*\\])")(3)))

pairs.collect().foreach(println)