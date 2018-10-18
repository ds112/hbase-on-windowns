import org.apache.spark.mllib.regression.LinearRegressionWithSGD
import org.apache.spark.mllib.util.MLUtils
import org.apache.spark.mllib.optimization.{SimpleUpdater, SquaredL2Updater, L1Updater}

  object RegType extends Enumeration {
    type RegType = Value
    val NONE, L1, L2 = Value
  }

  import RegType._
  
    val numIterations: Int = 100
    val stepSize: Double = 1.0
    val regParam: Double = 0.01

	
    val examples = MLUtils.loadLibSVMFile(sc, "/Data/Spark/MLLib/Sample_Linear_Regression_Data.txt").cache()

    val splits = examples.randomSplit(Array(0.8, 0.2))
    val training = splits(0).cache()
    val test = splits(1).cache()

    val numTraining = training.count()
    val numTest = test.count()
    println(s"Training: $numTraining, test: $numTest.")

    examples.unpersist(blocking = false)

 

    val algorithm = new LinearRegressionWithSGD()
    algorithm.optimizer.setNumIterations(numIterations).setStepSize(stepSize).setRegParam(regParam)

    val model = algorithm.run(training)

    val prediction = model.predict(test.map(_.features))
    val predictionAndLabel = prediction.zip(test.map(_.label))

    val loss = predictionAndLabel.map { case (p, l) =>
      val err = p - l
      err * err
    }.reduce(_ + _)
    val rmse = math.sqrt(loss / numTest)

    println(s"Test RMSE = $rmse.")