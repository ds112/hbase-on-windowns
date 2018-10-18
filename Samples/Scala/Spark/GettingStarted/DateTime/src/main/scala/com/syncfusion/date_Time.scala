package com.syncfusion

import org.apache.spark.sql.SQLContext
import org.apache.spark.{SparkContext, SparkConf}

//DateTime: Count the number of visitor based on time from given input 'NASA_Access_Log'
object dateTime {
  def main(args: Array[String]) {
    //Initialize Spark Config, Spark Context and SqlContext
    var conf = new SparkConf()
    //To run this jar with 'spark-submit' comment out the below line and re-build the jar.
    conf.setMaster("local")
    conf.setAppName("date Time")
    var sc = new SparkContext(conf)
    var sqlContext = new SQLContext(sc)

    //For remote cluster set remote host_name:port instead of localhost:9000
    val input = sc.textFile("hdfs://localhost:9000/Data/NASA_Access_Log")

    val regex = "- ".r

    val result = input.map(line => regex.replaceAllIn(line, ""))
    def getDate(line: String): String = {
      var date = line.replace("[", "").replace("]", "");
      date = date.split(":")(1);
      return date
    }
    val dateTime = result.map(line => (getDate(line.split(" (?=([^\"]*\"[^\"]*\")*[^\"]*$)+(?![^\\[]*\\])")(1)), 1)).reduceByKey((a, b) => a + b).sortByKey();
    val format = dateTime.map(line => line._1 + "-" + ((line._1).toInt + 1) + " Hours :  " + line._2);
    format.collect().foreach(println)
    sc.stop()
  }
}