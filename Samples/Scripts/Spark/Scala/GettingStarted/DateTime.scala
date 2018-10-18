//DateTime: Count the number of visitor based on time from given input 'NASA_Access_Log'
val input = sc.textFile("/Data/NASA_Access_Log")
	
val regex = "- ".r
	
val result = input.map(line => regex.replaceAllIn(line,""))
def getDate(line:String) : String = {var date = line.replace("[", "").replace("]", "");date = date.split(":")(1);       return date   }
val dateTime = result.map(line => (getDate(line.split(" (?=([^\"]*\"[^\"]*\")*[^\"]*$)+(?![^\\[]*\\])")(1)), 1)).reduceByKey((a, b) => a + b).sortByKey();	
val format = dateTime.map(line => line._1 + "-" + ((line._1).toInt + 1) + " Hours :  "+line._2);	
format.collect().foreach(println)