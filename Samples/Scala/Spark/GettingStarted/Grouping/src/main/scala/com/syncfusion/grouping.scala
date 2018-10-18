package com.syncfusion

import org.apache.spark.{SparkContext, SparkConf}

//Grouping: Just Group the internet address(1), Date and corresponding count based on internet address. and printed inside brackets from input 'NASA_Access_Log'
object grouping {
  def main(args: Array[String]) {
    //Initialize Spark Config, Spark Context and SqlContext
    var conf = new SparkConf()
    //To run this jar with 'spark-submit' comment out the below line and re-build the jar.
    conf.setMaster("local")
    conf.setAppName("grouping")
    var sc = new SparkContext(conf)


    //For remote cluster set remote host_name:port instead of localhost:9000
    val input = sc.textFile("hdfs://localhost:9000/Data/NASA_Access_Log")

    val regex = "- ".r

    val result = input.map(line => regex.replaceAllIn(line, ""))


    def getDate(line: String): String = {
      var date = line.split(" (?=([^\"]*\"[^\"]*\")*[^\"]*$)+(?![^\\[]*\\])")(1);
      date = date.replace("[", "").split(":")(0);
      date = date.replace("/", "-").replace("Aug", "08");

      return date
    }


    val group = result.map(line => ("(" + line.split(" (?=([^\"]*\"[^\"]*\")*[^\"]*$)+(?![^\\[]*\\])")(0) + "," + getDate(line) + ")", 1))

    val groupPairs = group.reduceByKey((a, b) => a + b).sortByKey()


    groupPairs.collect().foreach(println)
    sc.stop()
  }
}