package com.syncfusion;

import org.apache.spark.SparkConf;
import org.apache.spark.SparkContext;
import org.apache.spark.api.java.JavaPairRDD;
import org.apache.spark.api.java.JavaRDD;
import org.apache.spark.api.java.JavaSparkContext;

import scala.Tuple2;

import org.apache.spark.api.java.function.Function2;
import org.apache.spark.api.java.function.PairFunction;

public final class FindReplace {
    public static void main(String[] args) throws Exception {
	
		// Creates sparkConf to set Configuration for a Spark application
		SparkConf sparkConf = new SparkConf().setAppName("FindReplace");
		////To run this jar with 'spark-submit' set master to 'yarn'
		sparkConf.setMaster("local");
        JavaSparkContext ctx = new JavaSparkContext(sparkConf);
		// Read the input file 
        //For remote cluster set remote host_name:port instead of localhost:9000 
        String cluster = "hdfs://localhost:9000";
        JavaRDD<String> lines = ctx.textFile(cluster + "/Data/NASA_Access_Log");
		// Creates the Key-Value pair with the Internet Address as the key and special characters, non-alphabetic characters removed input line as the value
        JavaPairRDD<String, String> counts = lines.mapToPair(
            new PairFunction<String, String, String>() {
            public Tuple2<String, String> call(String x) { return new Tuple2(GetKey(x), GetReplace(x)); }
            });
        
		// Saves the created Key-Value pair in text file
        counts.saveAsTextFile(cluster + "/Data/Output/FindReplaceJava");
        ctx.stop();
		}
		// Method to split the input line and returns the word at the index 0
		public static String GetKey(String line) {
		// Split the input line by white space unless text enclosed with in double quotes and '[]' and stores the each field as string array
			String[] words = line.split(" (?=([^\"]*\"[^\"]*\")*[^\"]*$)+(?![^\\[]*\\])");
			return words[0];
		}
		// Method to find the special characters and non-alphabetic characters from input and replace it with empty string value. 
		public static String GetReplace(String line) {
			String words = line.replaceAll("[^0-9a-zA-Z]+", "");
			return words;
		}
}
