package com.syncfusion

import java.util.Random
import org.apache.spark.{SparkContext, SparkConf}
import org.apache.spark.sql.{SQLContext, SparkSession}
import org.elasticsearch.spark.sql._

object SparkSQL {

  def main(args: Array[String]): Unit = {
      var sparkConf = new SparkConf()
	  sparkConf.setMaster("local")
      sparkConf.setAppName("Elasticsearch_SparkSQL")
	  var sparkContext = new SparkContext(sparkConf)
	  val sparkSession = SparkSession.builder().master("local").appName("Elasticsearch_SparkSQL").getOrCreate()
	  val record1 = """{"id":"1","sources":["source1"],"name":{"fname":"Andy"},"age":{"age":"25"}}"""      
      val record2 = """{"id":"2","sources":["source1"],"name":{"fname":"Justin"},"age":{"age":"30"}}"""
      val jsonRdd = sparkContext.makeRDD(Seq(record1, record2))
      val jsonDf = sparkSession.read.json(jsonRdd).toDF.select("id", "name","age")
	  // Set the valid Elasticsearch configuration 
	  //es.nodes -> http://IP:port
	  //es.net.http.auth.user -> username
	  //es.net.http.auth.pass -> password
	  val config = Map("es.nodes" -> "http://localhost:9200","es.net.http.auth.user" -> "elastic","es.net.http.auth.pass"-> "changeme")
	  // specify the name of the index to be created format: indexName/typeName
	  val elasticsearchIndex="index/type";
	  jsonDf.saveToEs(elasticsearchIndex, config)	  
	  val dataFrameLayout = sparkSession.read.format("org.elasticsearch.spark.sql").options(config).load(elasticsearchIndex)
      dataFrameLayout.createOrReplaceTempView("people")
	  println("#####################################################")
      sparkSession.sql("select * from people").collect().foreach(println)
	  println("#####################################################")
  }
}