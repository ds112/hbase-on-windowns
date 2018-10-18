package com.syncfusion

import org.apache.spark.{SparkContext, SparkConf}

//Sorting: Sorting and counts the internet address from input 'NASA_Access_Log'

object sorting {
  def main(args: Array[String]) {
    //Initialize Spark Config, Spark Context and SqlContext
    var conf = new SparkConf()
    //To run this jar with 'spark-submit' comment out the below line and re-build the jar.
    conf.setMaster("local")
    conf.setAppName("sorting")
    var sc = new SparkContext(conf)


    //For remote cluster set remote host_name:port instead of localhost:9000
    val input = sc.textFile("hdfs://localhost:9000/Data/NASA_Access_Log")

    val pairs = input.map(line => (line.split(" (?=([^\"]*\"[^\"]*\")*[^\"]*$)+(?![^\\[]*\\])")(0), 1))

    val counts = pairs.reduceByKey((a, b) => a + b)

    val sortedValue = counts.sortByKey()

    sortedValue.collect().foreach(println)
    sc.stop()
  }
}