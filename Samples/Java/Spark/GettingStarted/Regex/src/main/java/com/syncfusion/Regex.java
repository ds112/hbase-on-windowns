package com.syncfusion;

import org.apache.spark.SparkConf;
import org.apache.spark.api.java.JavaPairRDD;
import org.apache.spark.api.java.JavaRDD;
import org.apache.spark.api.java.JavaSparkContext;
import scala.Tuple2;
import org.apache.spark.api.java.function.Function;

public final class Regex {
    public static void main(String[] args) throws Exception {
		
		// Creates sparkConf to set Configuration for a Spark application
		SparkConf sparkConf = new SparkConf().setAppName("Regex");
		//To run this jar with 'spark-submit' set master to 'yarn'
		sparkConf.setMaster("local");
		// Creates JavaSparkContext for the Main entry point of Spark functionality
        JavaSparkContext ctx = new JavaSparkContext(sparkConf);
		// Read the input file 
        //For remote cluster set remote host_name:port instead of localhost:9000
        String cluster = "hdfs://localhost:9000";
        JavaRDD<String> lines = ctx.textFile(cluster + "/Data/NASA_Access_Log");
		// Creates JavaRDD by filtering input line that contains HTTP code "200"
        JavaRDD<String> counts = lines.filter(new Function<String, Boolean>() {
        public Boolean call(String s) { 
            String key = GetKey(s);
            return key.contains("200"); 
        }
        });
		
		// Saves the filtered JavaRDD in text file
        counts.saveAsTextFile(cluster + "/Data/Output/RegexJava");
        ctx.stop();
		}
		// Method to split the input line and returns the word at the index 3
		public static String GetKey(String line) {
			// Replace '- ' with empty value, so input lines are separated only by white space
			line = line.replace("- ", "");
			// Split the input line by white space unless text enclosed with in double quotes and '[]' and stores the each field as string array
			String[] words = line.split(" (?=([^\"]*\"[^\"]*\")*[^\"]*$)+(?![^\\[]*\\])");
			return words[3];
		}
}
