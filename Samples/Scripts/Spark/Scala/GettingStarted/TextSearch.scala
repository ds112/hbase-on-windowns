//Textsearchapplication: Filters the lines contains "lya.colorado.edu" from input 'NASA_Access_Log'
val textFile = sc.textFile("/Data/NASA_Access_Log")
    
val errors = textFile.filter(line => line.contains("lya.colorado.edu"))

errors.collect().foreach(println)