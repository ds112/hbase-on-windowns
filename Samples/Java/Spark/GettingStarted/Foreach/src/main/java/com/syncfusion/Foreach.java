package com.syncfusion;
import org.apache.spark.SparkConf;
import org.apache.spark.api.java.JavaPairRDD;
import org.apache.spark.api.java.JavaRDD;
import org.apache.spark.api.java.JavaSparkContext;
import scala.Tuple2;
import org.apache.spark.api.java.function.Function2;
import org.apache.spark.api.java.function.PairFunction;

public final class Foreach {
    public static void main(String[] args) throws Exception {
	
		// Creates sparkConf to set Configuration for a Spark application
		SparkConf sparkConf = new SparkConf().setAppName("Foreach");
		//To run this jar with 'spark-submit' set master to 'yarn'
		sparkConf.setMaster("local");
		// Creates JavaSparkContext for the Main entry point of Spark functionality
        JavaSparkContext ctx = new JavaSparkContext(sparkConf);
		// Read the input file 
		//For remote cluster set remote host_name:port instead of localhost:9000 
        String cluster = "hdfs://localhost:9000";
        JavaRDD<String> lines = ctx.textFile(cluster + "/Data/NASA_Access_Log");
		// Creates the Key-Value pair with the Internet Address as the key and the corresponding HTTP code as the value
        JavaPairRDD<String, String> counts = lines.mapToPair(
            new PairFunction<String, String, String>() {
            public Tuple2<String, String> call(String x) { return new Tuple2(GetKey(x, 1), GetKey(x, 3)); }
            });
        
		// Saves the created Key-Value pair in text file
        counts.saveAsTextFile(cluster + "/Data/Output/ForeachJava");
        ctx.stop();
		}
		// Method to split the input line and returns the word 
		public static String GetKey(String line, Integer index) {
		// Replace '- ' with empty value, so input lines are separated only by white space
        line = line.replace("- ", "");
		// Split the input line by white space unless text enclosed with in double quotes and '[]' and stores the each field as string array
        String[] words = line.split(" (?=([^\"]*\"[^\"]*\")*[^\"]*$)+(?![^\\[]*\\])");
		return words[index];
		}
}
