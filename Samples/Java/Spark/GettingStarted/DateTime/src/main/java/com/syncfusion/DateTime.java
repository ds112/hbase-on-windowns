package com.syncfusion;

import org.apache.spark.SparkConf;
import org.apache.spark.SparkContext;
import org.apache.spark.api.java.JavaPairRDD;
import org.apache.spark.api.java.JavaRDD;
import org.apache.spark.api.java.JavaSparkContext;
import scala.Tuple2;
import org.apache.spark.api.java.function.Function2;
import org.apache.spark.api.java.function.PairFunction;
import org.apache.spark.api.java.function.Function;

public final class DateTime {
    public static void main(String[] args) throws Exception {
    	
   
		// Creates sparkConf to set Configuration for a Spark application
		SparkConf sparkConf = new SparkConf().setAppName("DateTime");
		//To run this jar with 'spark-submit' set master to 'yarn' 
		sparkConf.setMaster("local");
        JavaSparkContext ctx = new JavaSparkContext(sparkConf);
		// Read the input file
        //For remote cluster set remote host_name:port instead of localhost:9000 
        String Cluster = "hdfs://localhost:9000";
        JavaRDD<String> lines = ctx.textFile(Cluster + "/Data/NASA_Access_Log");
		// Creates the Key-Value pair with the hour value as the key and the corresponding count as the value
        JavaPairRDD<String, Integer> counts = lines.mapToPair(
            new PairFunction<String, String, Integer>() {
            public Tuple2<String, Integer> call(String x) { return new Tuple2(GetKey(x) + "-" +(Integer.parseInt(GetKey(x))+1) + " Hours: ", 1); }
            }).reduceByKey(
            new Function2<Integer, Integer, Integer>() {
            public Integer call(Integer a, Integer b) { return a + b; }
            });
		// Creates the sorted Key-Value pair
        JavaPairRDD<String, Integer> dateTime = counts.sortByKey();
		// Merge the dateTime key-value pair
		JavaRDD<String> dateArray=dateTime.map(
				   new Function<Tuple2<String,Integer>,String>() {
						public String call(Tuple2<String,Integer> s)	{
							String result = s._1 + " " + s._2;
							return result;
						}
						});
		// Saves the created Key-Value pair in text file
		//For remote cluster set remote host_name:port instead of localhost:9000 
        dateArray.saveAsTextFile(Cluster + "/Data/Output/DateTimeJava");
		ctx.stop();
		}   
		// Method to split the input line and returns hour value
		public static String GetKey(String line) {
			line = line.replace("- ", "");
			// Split the input line by white space unless text enclosed with in double quotes and '[]' and stores the each field as string array
			String[] words = line.split(" (?=([^\"]*\"[^\"]*\")*[^\"]*$)+(?![^\\[]*\\])");
			// Get the timestamp from the splitted words
			String dateValue = words[1];
			// Split the timestamp by colon
			String[] dateArray = dateValue.replace("[", "").split(":");
			// Store the hour value 
			String date = dateArray[1];
			return date;
		}
}
			