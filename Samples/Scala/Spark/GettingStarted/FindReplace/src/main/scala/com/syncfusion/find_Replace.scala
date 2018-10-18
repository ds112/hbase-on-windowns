package com.syncfusion

import org.apache.spark.{SparkContext, SparkConf}

//Findreplace:  Remove special characters and symbols from the input 'nasa_access_log' and print the output as keyvalue pair with the internet address(1)
object findReplace {
  def main(args: Array[String]) {
    //Initialize Spark Config, Spark Context and SqlContext
    var conf = new SparkConf()
    //To run this jar with 'spark-submit' comment out the below line and re-build the jar.
    conf.setMaster("local")
    conf.setAppName("find Replace")
    var sc = new SparkContext(conf)

    //For remote cluster set remote host_name:port instead of localhost:9000
    val input = sc.textFile("hdfs://localhost:9000/Data/NASA_Access_Log")

    val regex = "[^0-9a-zA-Z]+".r

    val pairs = input.map(line => (line.split(" (?=([^\"]*\"[^\"]*\")*[^\"]*$)+(?![^\\[]*\\])")(0), regex.replaceAllIn(line, "")))

    pairs.collect().foreach(println)
    sc.stop()
  }
}