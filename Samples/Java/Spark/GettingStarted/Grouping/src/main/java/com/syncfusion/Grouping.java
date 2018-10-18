package com.syncfusion;

import org.apache.spark.SparkConf;
import org.apache.spark.api.java.JavaPairRDD;
import org.apache.spark.api.java.JavaRDD;
import org.apache.spark.api.java.JavaSparkContext;
import scala.Tuple2;
import org.apache.spark.api.java.function.Function2;
import org.apache.spark.api.java.function.PairFunction;

public final class Grouping {
    public static void main(String[] args) throws Exception {
   
		// Creates sparkConf to set Configuration for a Spark application
		SparkConf sparkConf = new SparkConf().setAppName("Grouping");
		//To run this jar with 'spark-submit' set master to 'yarn'
		sparkConf.setMaster("local");
		// Creates JavaSparkContext for the Main entry point of Spark functionality
        JavaSparkContext ctx = new JavaSparkContext(sparkConf);
		// Read the input file 
        //For remote cluster set remote host_name:port instead of localhost:9000 
        String cluster = "hdfs://localhost:9000";
        JavaRDD<String> lines = ctx.textFile(cluster + "/Data/NASA_Access_Log");
		// Creates the Key-Value pair with the Internet Address, date as key grouped into brackets and corresponding count as value 
        JavaPairRDD<String, Integer> counts = lines.mapToPair(
            new PairFunction<String, String, Integer>() {
            public Tuple2<String, Integer> call(String x) { return new Tuple2("(" + x.split(" (?=([^\"]*\"[^\"]*\")*[^\"]*$)+(?![^\\[]*\\])")[0] + "," + GetKey(x)+ ")" , 1); }
            }).reduceByKey(
            new Function2<Integer, Integer, Integer>() {
            public Integer call(Integer a, Integer b) { return a + b; }
            });
		// Creates the sorted Key-Value pair
		JavaPairRDD<String, Integer> sortedValue = counts.sortByKey();
		// Saves the created Key-Value pair in text file
        sortedValue.saveAsTextFile(cluster + "/Data/Output/GroupingJava");
        ctx.stop();
		}
		// Method to split the input line and returns the date
		public static String GetKey(String line) {
			// Replace '- ' with empty value, so input lines are separated only by white space
			line = line.replace("- ", "");
			// Split the input line by white space unless text enclosed with in double quotes and '[]' and stores the each field as string array
			String[] words = line.split(" (?=([^\"]*\"[^\"]*\")*[^\"]*$)+(?![^\\[]*\\])");
			// Get the timestamp from the splitted words
			String dateValue = words[1];
			// Split the timestamp by colon
			String[] dateArray = dateValue.replace("[", "").split(":");
			// Store the date
			String date = dateArray[0];
			// Replaces the date contains "/" by "-"
			date = date.replace("/", "-");
			// Replaces the date contains "Aug" by "08"
			date = date.replace("Aug", "08");
			return date;
		}
}
