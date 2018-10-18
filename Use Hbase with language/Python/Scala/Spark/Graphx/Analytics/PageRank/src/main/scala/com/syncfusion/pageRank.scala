package com.syncfusion

import scala.collection.mutable
import org.apache.spark._
import org.apache.spark.storage.StorageLevel
import org.apache.spark.graphx._
import org.apache.spark.graphx.lib._
import org.apache.spark.graphx.PartitionStrategy._

/**
 * Driver program for running graph algorithms.
 */
object pageRank{
  def main(args: Array[String]) {
    //For remote cluster set remote host_name:port instead of localhost:9000
    val args = Array("pagerank", "hdfs://localhost:9000/Data/Spark/MLLib/PageRank_Data.txt", "--numEPart=3")
    val taskType = args(0)
    val fname = args(1)
    val optionsList = args.drop(2).map { arg =>
      arg.dropWhile(_ == '-').split('=') match {
        case Array(opt, v) => (opt -> v)
        case _ => throw new IllegalArgumentException("Invalid argument: " + arg)
      }
    }
    val options = mutable.Map(optionsList: _*)

    //Initialize SparkConf
    val conf = new SparkConf()
    //To run this jar with 'spark-submit' comment out the below line and re-build the jar.
    conf.setMaster("local")
    conf.setAppName("page rank")
    GraphXUtils.registerKryoClasses(conf)

    val numEPart = options.remove("numEPart").map(_.toInt).getOrElse {
      println("Set the number of edge partitions using --numEPart.")
      sys.exit(1)
    }
    val partitionStrategy: Option[PartitionStrategy] = options.remove("partStrategy").map(PartitionStrategy.fromString(_))
    val edgeStorageLevel = options.remove("edgeStorageLevel").map(StorageLevel.fromString(_)).getOrElse(StorageLevel.MEMORY_ONLY)
    val vertexStorageLevel = options.remove("vertexStorageLevel").map(StorageLevel.fromString(_)).getOrElse(StorageLevel.MEMORY_ONLY)

    val tol = options.remove("tol").map(_.toFloat).getOrElse(0.001F)
    val outFname = options.remove("output").getOrElse("")
    val numIterOpt = options.remove("numIter").map(_.toInt)

    options.foreach {
      case (opt, _) => throw new IllegalArgumentException("Invalid option: " + opt)
    }

    println("======================================")
    println("|             PageRank               |")
    println("======================================")
    var sc = new SparkContext(conf)
    val unpartitionedGraph = GraphLoader.edgeListFile(sc, fname,
      numEdgePartitions = numEPart,
      edgeStorageLevel = edgeStorageLevel,
      vertexStorageLevel = vertexStorageLevel).cache()
    val graph = partitionStrategy.foldLeft(unpartitionedGraph)(_.partitionBy(_))

    println("GRAPHX: Number of vertices " + graph.vertices.count)
    println("GRAPHX: Number of edges " + graph.edges.count)

    sc.stop()
  }
}
