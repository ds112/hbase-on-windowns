
package com.syncfusion

import org.apache.spark.{SparkContext, SparkConf}

//Regex: Filters the lines which has HTTP reply code "200" from input 'NASA_Access_Log'
object regex {
  def main(args: Array[String]) {
    //Initialize Spark Config, Spark Context and SqlContext
    var conf = new SparkConf()
    //To run this jar with 'spark-submit' comment out the below line and re-build the jar.
    conf.setMaster("local")
    conf.setAppName("regex")
    var sc = new SparkContext(conf)

    val input = sc.textFile("hdfs://localhost:9000/Data/NASA_Access_Log")

    val regex = "- ".r

    val result = input.map(line => regex.replaceAllIn(line, ""))

    val regexPattern = ".*200.*".r

    val matchedPattern = result.filter(line => regexPattern.pattern.matcher(line.split(" (?=([^\"]*\"[^\"]*\")*[^\"]*$)+(?![^\\[]*\\])")(3)).matches)
    matchedPattern.collect().foreach(println)
    sc.stop()
  }
}