package com.syncfusion

import java.util.Random
import org.apache.spark.{SparkContext, SparkConf}
import com.mongodb.spark._
import org.bson.Document
import com.mongodb.spark.config._
import org.apache.spark.sql.{SQLContext, SparkSession}
import org.apache.spark.sql.SQLContext

object SparkSQL {

  def main(args: Array[String]): Unit = {
   var sparkConf = new SparkConf()
	  sparkConf.setMaster("local")
      sparkConf.setAppName("MongoDB_SparkSQL")
	  // Pass the valid MongoDB uri. format: mongodb://local(or)RemoteIP/databaseName.collectionName
	  sparkConf.set("spark.mongodb.output.uri", "mongodb://127.0.0.1/db.col") 
	  var sparkContext = new SparkContext(sparkConf)
	  
   val documents = """{"name": "Bilbo Baggins", "age": 50}
	  {"name": "Gandalf", "age": 1000}
	  {"name": "Balin", "age": 178}
	  {"name": "Kili", "age": 77}
	  {"name": "Dwalin", "age": 169}
	  {"name": "Thorin", "age": 195}""".trim.stripMargin.split("[\\r\\n]+").toSeq
     sparkContext.parallelize(documents.map(Document.parse)).saveToMongoDB()	  
     val sparkSession = SparkSession.builder().master("local").appName("MongoDB_SparkSQL").config("spark.mongodb.input.uri", "mongodb://127.0.0.1/db.col").getOrCreate()
	 val data = MongoSpark.load[SparkSQL.Character](sparkSession)
     data.createOrReplaceTempView("characters") 
	 val rows = sparkSession.sql("SELECT name, age FROM characters")
     rows.show()
     MongoSpark.save(rows.write.option("collection", "testCol"))
     MongoSpark.load[SparkSQL.Character](sparkSession, ReadConfig(Map("collection" -> "testCol"),Some(ReadConfig(sparkSession)))).show()
  }
    case class Character(name: String, age: Int)
}









