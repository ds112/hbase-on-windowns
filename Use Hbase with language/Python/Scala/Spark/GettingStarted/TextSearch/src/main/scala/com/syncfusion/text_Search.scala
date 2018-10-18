package com.syncfusion

import org.apache.spark.{SparkContext, SparkConf}

//Textsearchapplication: Filters the lines contains "lya.colorado.edu" from input 'NASA_Access_Log'
object textSearch {
  def main(args: Array[String]) {
    //Initialize Spark Config, Spark Context and SqlContext
    var conf = new SparkConf()
    //To run this jar with 'spark-submit' comment out the below line and re-build the jar.
    conf.setMaster("local")
    conf.setAppName("text Search")
    var sc = new SparkContext(conf)



    //For remote cluster set remote host_name:port instead of localhost:9000
    val textFile = sc.textFile("hdfs://localhost:9000/Data/NASA_Access_Log")

    val errors = textFile.filter(line => line.contains("lya.colorado.edu"))

    errors.collect().foreach(println)
    sc.stop()
  }
}
