package com.syncfusion

import org.apache.spark.{SparkContext, SparkConf}

//Foreach: For every internet address(1), get the HTTP reply code(4) and print it as keyvalue pair from input 'NASA_Access_Log'
object foreach {
  def main(args: Array[String]) {
    //Initialize Spark Config, Spark Context and SqlContext
    var conf = new SparkConf()
    //To run this jar with 'spark-submit' comment out the below line and re-build the jar.
    conf.setMaster("local")
    conf.setAppName("foreach")
    var sc = new SparkContext(conf)



    //For remote cluster set remote host_name:port instead of localhost:9000
    val input = sc.textFile("hdfs://localhost:9000/Data/NASA_Access_Log")

    val regex = "- ".r

    val result = input.map(line => regex.replaceAllIn(line, ""))

    val pairs = result.map(line => (line.split(" (?=([^\"]*\"[^\"]*\")*[^\"]*$)+(?![^\\[]*\\])")(1), line.split(" (?=([^\"]*\"[^\"]*\")*[^\"]*$)+(?![^\\[]*\\])")(3)))

    pairs.collect().foreach(println)
    sc.stop()
  }
}