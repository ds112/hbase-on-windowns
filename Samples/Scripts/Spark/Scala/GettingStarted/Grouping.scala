//Grouping: Just Group the internet address(1), Date and corresponding count based on internet address. and printed inside brackets from input 'NASA_Access_Log'
val input = sc.textFile("/Data/NASA_Access_Log")
	
val regex = "- ".r
	
val result = input.map(line => regex.replaceAllIn(line,""))

def getDate(line:String) : String = {var date = line.split(" (?=([^\"]*\"[^\"]*\")*[^\"]*$)+(?![^\\[]*\\])")(1);
date = date.replace("[", "").split(":")(0);
date = date.replace("/", "-").replace("Aug","08"); 

return date}
	
val group = result.map(line => ("(" + line.split(" (?=([^\"]*\"[^\"]*\")*[^\"]*$)+(?![^\\[]*\\])")(0) + "," + getDate(line)+ ")" , 1))
	
val groupPairs = group.reduceByKey((a, b) => a + b).sortByKey()

groupPairs.collect().foreach(println)