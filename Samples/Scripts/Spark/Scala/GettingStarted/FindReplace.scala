//Findreplace:  Remove special characters and symbols from the input 'NASA_Access_Log' and print the output as keyvalue pair with the internet address(1)
val input = sc.textFile("/Data/NASA_Access_Log")
	
val regex = "[^0-9a-zA-Z]+".r
	
val pairs = input.map(line => (line.split(" (?=([^\"]*\"[^\"]*\")*[^\"]*$)+(?![^\\[]*\\])")(0),regex.replaceAllIn(line,"")))

pairs.collect().foreach(println)