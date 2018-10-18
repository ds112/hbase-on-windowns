package com.syncfusion

import org.apache.spark.sql.SQLContext
import org.apache.spark.{SparkContext, SparkConf}

object json{
  def main(args: Array[String]){
    //Initialize Spark Config, Spark Context and SqlContext
    var conf = new SparkConf()
    //To run this jar with 'spark-submit' comment out the below line and re-build the jar.
    conf.setMaster("local")
    conf.setAppName("Json")
    var sc = new SparkContext(conf)
    var sqlContext = new SQLContext(sc)

    // A JSON dataset is pointed to by path.
    // The path can be either a single text file or a directory storing text files.
    //For remote cluster set remote host_name:port instead of localhost:9000
val path = "hdfs://localhost:9000//Data/Spark/Resources/People.json"
val people = sqlContext.read.json(path)

// The inferred schema can be visualized using the printSchema() method.
people.printSchema()
// root
//  |-- age: integer (nullable = true)
//  |-- name: string (nullable = true)

// Register this DataFrame as a table.
people.registerTempTable("people")

// SQL statements can be run by using the sql methods provided by sqlContext.
val teenagers = sqlContext.sql("SELECT name FROM people WHERE age >= 13 AND age <= 19")

// Alternatively, a DataFrame can be created for a JSON dataset represented by
// an RDD[String] storing one JSON object per string.
val anotherPeopleRDD = sc.parallelize(
  """{"name":"Yin","address":{"city":"Columbus","state":"Ohio"}}""" :: Nil)
val anotherPeople = sqlContext.read.json(anotherPeopleRDD)

    sc.stop()
  }
}