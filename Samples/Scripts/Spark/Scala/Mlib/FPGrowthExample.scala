import org.apache.spark.mllib.fpm.FPGrowth

  val transactions = sc.textFile("/Data/Spark/MLLib/Sample_FPGrowth.txt").map(_.split(" ")).cache()

    println(s"Number of transactions: ${transactions.count()}")

    val model = new FPGrowth().setMinSupport(0.8).setNumPartitions(2).run(transactions)

    println(s"Number of frequent itemsets: ${model.freqItemsets.count()}")

    model.freqItemsets.collect().foreach { itemset =>
      println(itemset.items.mkString("[", ",", "]") + ", " + itemset.freq)
    }