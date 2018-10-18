package com.syncfusion

import org.apache.spark.mllib.classification.NaiveBayes
import org.apache.spark.mllib.util.MLUtils
import org.apache.spark.{SparkContext, SparkConf}


object sparseNaive {
  def main(args: Array[String]) {
    //Initialize Spark Config and Spark Context
    var conf = new SparkConf()
    //To run this jar with 'spark-submit' comment out the below line and re-build the jar.
    conf.setMaster("local")
    conf.setAppName("Sparse Naive bayes")
    var sc = new SparkContext(conf)
    val minPartition: Int = 0
    val numFeatures: Int = -1
    val lambda: Double = 1.0

    val minPartitions =
      if (minPartition > 0) minPartition else sc.defaultMinPartitions

    //For remote cluster set remote host_name:port instead of localhost:9000
    val examples =
      MLUtils.loadLibSVMFile(sc, "hdfs://localhost:9000/Data/Spark/MLLib/Sample_LibSVM_Data.txt", numFeatures, minPartitions)
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

    sc.stop()
  }
}