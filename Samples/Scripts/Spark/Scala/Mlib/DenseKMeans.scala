import org.apache.spark.mllib.clustering.KMeans
import org.apache.spark.mllib.linalg.Vectors

  val k: Int = 2
  val numIterations: Int = 10

    val examples = sc.textFile("/Data/Spark/MLLib/KMeans_Data.txt").map { line =>
      Vectors.dense(line.split(' ').map(_.toDouble))
    }.cache()

    val numExamples = examples.count()

    println(s"numExamples = $numExamples.")

    val model = new KMeans().setK(k).setMaxIterations(numIterations).run(examples)

    val cost = model.computeCost(examples)

    println(s"Total cost = $cost.")