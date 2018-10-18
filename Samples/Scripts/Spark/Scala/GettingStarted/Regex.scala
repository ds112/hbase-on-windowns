//Regex: Filters the lines which has HTTP reply code "200" from input 'NASA_Access_Log'
val input = sc.textFile("/Data/NASA_Access_Log")
	
val regex = "- ".r
	
val result = input.map(line => regex.replaceAllIn(line,""))
	
val regexPattern = ".*200.*".r
	
val matchedPattern = result.filter(line => regexPattern.pattern.matcher(line.split(" (?=([^\"]*\"[^\"]*\")*[^\"]*$)+(?![^\\[]*\\])")(3)).matches)
matchedPattern.collect().foreach(println)