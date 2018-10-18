package com.syncfusion;

import org.apache.spark.SparkConf;
import org.apache.spark.api.java.JavaPairRDD;
import org.apache.spark.api.java.JavaRDD;
import org.apache.spark.api.java.JavaSparkContext;
import org.apache.spark.api.java.function.Function;


public final class TextSearch {
    public static void main(String[] args) throws Exception {
   
	// Creates sparkConf to set Configuration for a Spark application
    SparkConf sparkConf = new SparkConf().setAppName("TextSearch");
    //To run this jar with 'spark-submit' set master to 'yarn'
    sparkConf.setMaster("local");

	// Creates JavaSparkContext for the Main entry point of Spark functionality
    JavaSparkContext ctx = new JavaSparkContext(sparkConf);
	// Read the input file 
    //For remote cluster set remote host_name:port instead of localhost:9000
    String cluster = "hdfs://localhost:9000";
    JavaRDD<String> textFile = ctx.textFile(cluster + "/Data/NASA_Access_Log");
	// Creates JavaRDD and filter the input lines that contains the Internet Address "lya.colorado.edu"
    JavaRDD<String> errors = textFile.filter(new Function<String, Boolean>() {
		public Boolean call(String s) { return s.contains("lya.colorado.edu"); }
		});
		
	// Saves the created JavaRDD in text file
	errors.saveAsTextFile(cluster + "/Data/Output/TextSearch");	
    ctx.stop();
  }
}
