package com.syncfusion

import java.util.Random
import org.apache.spark.{SparkContext, SparkConf}


object GroupBy {
  def main(args: Array[String]) {
    //Initialize Spark Config and Spark Context
    var conf = new SparkConf()
    //To run this jar with 'spark-submit' comment out the below line and re-build the jar.
    conf.setMaster("local")
    conf.setAppName("GroupBy")
    var sc = new SparkContext(conf)


    var numMappers = 2
    var numKVPairs = 1000
    var valSize = 1000
    var numReducers = numMappers
    val pairs1 = sc.parallelize(0 until numMappers, numMappers).flatMap { p =>
      val ranGen = new Random
      var arr1 = new Array[(Int, Array[Byte])](numKVPairs)
      for (i <- 0 until numKVPairs) {
        val byteArr = new Array[Byte](valSize)
        ranGen.nextBytes(byteArr)
        arr1(i) = (ranGen.nextInt(Int.MaxValue), byteArr)
      }
      arr1
    }.cache()
    pairs1.count()
    println(pairs1.groupByKey(numReducers).count())
    sc.stop()
  }
}