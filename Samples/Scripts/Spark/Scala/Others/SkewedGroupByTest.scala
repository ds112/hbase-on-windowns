import java.util.Random   
    var numMappers = 2
    var numKVPairs = 1000
    var valSize = 1000
    var numReducers = numMappers    
    val pairs1 = sc.parallelize(0 until numMappers, numMappers).flatMap { p =>
      val ranGen = new Random

      // map output sizes lineraly increase from the 1st to the last
      numKVPairs = (1.0 * (p + 1) / numMappers * numKVPairs).toInt
      var arr1 = new Array[(Int, Array[Byte])](numKVPairs)
      for (i <- 0 until numKVPairs) {
        val byteArr = new Array[Byte](valSize)
        ranGen.nextBytes(byteArr)
        arr1(i) = (ranGen.nextInt(Int.MaxValue), byteArr)
      }
      arr1
    }.cache()

    // Enforce that everything has been calculated and in cache
    pairs1.count()
    println(pairs1.groupByKey(numReducers).count()) 