package com.syncfusion
import org.apache.spark.{SparkContext, SparkConf}

object Wordcount{
  def main(args: Array[String]){
    //Initialize Spark Config and Spark Context
    var conf = new SparkConf()
    //To run this jar with 'spark-submit' comment out the below line and re-build the jar.
    conf.setMaster("local")
    conf.setAppName("Word Count")
    var sc = new SparkContext(conf)

    //WordCount: Normal wordcount program from input 'warpeace.txt'
    //For remote cluster set remote host_name:port instead of localhost:9000
    val textFile = sc.textFile("hdfs://localhost:9000/Data/WarPeace.txt")
    val counts = textFile.flatMap(line => line.split(" ")).map(word => (word, 1)).reduceByKey(_ + _)
    counts.collect().foreach(println)
    sc.stop()
  }
}
