import org.apache.spark.mllib.classification.NaiveBayes
import org.apache.spark.mllib.util.MLUtils

     val minPartition: Int = 0
     val numFeatures: Int = -1
     val lambda: Double = 1.0

    val minPartitions =
      if (minPartition > 0) minPartition else sc.defaultMinPartitions
	  
    val examples =
      MLUtils.loadLibSVMFile(sc, "/Data/Spark/MLLib/Sample_LibSVM_Data.txt", numFeatures, minPartitions)
    // Cache examples because it will be used in both training and evaluation.
    examples.cache()

    val splits = examples.randomSplit(Array(0.8, 0.2))
    val training = splits(0)
    val test = splits(1)

    val numTraining = training.count()
    val numTest = test.count()

    println(s"numTraining = $numTraining, numTest = $numTest.")

    val model = new NaiveBayes().setLambda(lambda).run(training)

    val prediction = model.predict(test.map(_.features))
    val predictionAndLabel = prediction.zip(test.map(_.label))
    val accuracy = predictionAndLabel.filter(x => x._1 == x._2).count().toDouble / numTest

    println(s"Test accuracy = $accuracy.")