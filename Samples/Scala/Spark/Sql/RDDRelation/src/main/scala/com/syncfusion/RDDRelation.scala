package com.syncfusion

import org.apache.spark.sql.SQLContext
import org.apache.spark.sql.functions._
import org.apache.spark.{SparkContext, SparkConf}

// One method for defining the schema of an RDD is to make a case class with the desired column
// names and types.
case class Record(key: Int, value: String)

    // Importing the SQL context gives access to all the SQL functions and implicit conversions.

object RDD {
    def main(args: Array[String]) {
        //Initialize Spark Config and Spark Context
        var conf = new SparkConf()
        //To run this jar with 'spark-submit' comment out the below line and re-build the jar.
        conf.setMaster("local")
        conf.setAppName("RDD")
        var sc = new SparkContext(conf)
        val sqlContext = new SQLContext(sc)
        import sqlContext.implicits._

        val df = sc.parallelize((1 to 100).map(i => Record(i, s"val_$i"))).toDF()
        // Any RDD containing case classes can be registered as a table.  The schema of the table is
        // automatically inferred using scala reflection.
        df.registerTempTable("records")

        // Once tables have been registered, you can run SQL queries over them.
        println("Result of SELECT *:")
        sqlContext.sql("SELECT * FROM records").collect().foreach(println)

        // Aggregation queries are also supported.
        val count = sqlContext.sql("SELECT COUNT(*) FROM records").collect().head.getLong(0)
        println(s"COUNT(*): $count")

        // The results of SQL queries are themselves RDDs and support all normal RDD functions.  The
        // items in the RDD are of type Row, which allows you to access each column by ordinal.
        val rddFromSql = sqlContext.sql("SELECT key, value FROM records WHERE key < 10")

        println("Result of RDD.map:")
        rddFromSql.map(row => s"Key: ${row(0)}, Value: ${row(1)}").collect().foreach(println)

        // Queries can also be written using a LINQ-like Scala DSL.
        df.where($"key" === 1).orderBy($"value".asc).select($"key").collect().foreach(println)

        // Write out an RDD as a parquet file.
        //For remote cluster set remote host_name:port instead of localhost:9000
        df.write.parquet("hdfs://localhost:9000/pair.parquet")

        // Read in parquet file.  Parquet files are self-describing so the schmema is preserved.

        //For remote cluster set remote host_name:port instead of localhost:9000
        val parquetFile = sqlContext.read.parquet("hdfs://localhost:9000/pair.parquet")

        // Queries can be run using the DSL on parequet files just like the original RDD.
        parquetFile.where($"key" === 1).select($"value".as("a")).collect().foreach(println)

        // These files can also be registered as tables.
        parquetFile.registerTempTable("parquetFile")
        sqlContext.sql("SELECT * FROM parquetFile").collect().foreach(println)
        sc.stop()
    }
}

