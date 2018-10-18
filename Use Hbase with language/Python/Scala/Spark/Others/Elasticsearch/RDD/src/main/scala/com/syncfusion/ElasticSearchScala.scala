package com.syncfusion

import java.util.Random
import org.apache.spark.{SparkContext, SparkConf}
import org.elasticsearch.spark._


object Scala {

  def main(args: Array[String]): Unit = {
      var sparkConf = new SparkConf()
	  sparkConf.setMaster("local")
      sparkConf.setAppName("ElasticSearch_Scala")
	  var sparkContext = new SparkContext(sparkConf)
	  val record1 = Map("id"->"1","name" -> "Andy", "age" -> "25")
	  val record2 = Map("id"->"2","name" -> "Justin", "age" -> "30")
	  //Set the valid Elasticsearch configuration 
	  //es.nodes -> http://IP:port
	  //es.net.http.auth.user -> username
	  //es.net.http.auth.pass -> password
	  val config=Map("es.nodes" -> "http://localhost:9200","es.net.http.auth.user" -> "elastic","es.net.http.auth.pass"-> "changeme")
	  //specify the name of the index to be created format: indexName/typeName
	  val elasticsearchIndex="index/type";
	  sparkContext.makeRDD(Seq(record1, record2)).saveToEs(elasticsearchIndex,config)
	  val rdd=sparkContext.esRDD(elasticsearchIndex, "?q=*:*",config)
	  println("######################################")
	  rdd.foreach(println)
	  println("######################################")
  }
}