package com.syncfusion;

import scala.Tuple2;
import org.apache.spark.SparkConf;
import org.apache.spark.api.java.JavaPairRDD;
import org.apache.spark.api.java.JavaRDD;
import org.apache.spark.api.java.JavaSparkContext;
import org.apache.spark.api.java.function.FlatMapFunction;
import org.apache.spark.api.java.function.Function2;
import org.apache.spark.api.java.function.PairFunction;

import java.util.Arrays;
import java.util.List;
import java.util.regex.Pattern;

public final class WordCount {
	private static final Pattern SPACE = Pattern.compile(" ");
    public static void main(String[] args) throws Exception {

    // Creates sparkConf to set Configuration for a Spark application
    SparkConf sparkConf = new SparkConf().setAppName("WordCount");
    //To run this jar with 'spark-submit' set master to 'yarn'
    sparkConf.setMaster("local");
	// Creates JavaSparkContext for the Main entry point of Spark functionality
    JavaSparkContext ctx = new JavaSparkContext(sparkConf);
	// Read the input file 
    //For remote cluster set remote host_name:port instead of localhost:9000
    String cluster  = "hdfs://localhost:9000";
    JavaRDD<String> lines = ctx.textFile(cluster + "/Data/WarPeace.txt");
	// Creates JavaRDD by splitting each word by space and collect the words using flatMap function
    JavaRDD<String> words = lines.flatMap(new FlatMapFunction<String, String>() {
      @Override
      public Iterable<String> call(String s) {
        return Arrays.asList(SPACE.split(s));
      }
    });
	// Creates the Key-Value pair with each word as the key and the integer 1 as the value
    JavaPairRDD<String, Integer> ones = words.mapToPair(new PairFunction<String, String, Integer>() {
      @Override
      public Tuple2<String, Integer> call(String s) {
        return new Tuple2<String, Integer>(s, 1);
      }
    });
	// Creates the Key-Value pair with each word as the key and the corresponding word count as the value
    JavaPairRDD<String, Integer> counts = ones.reduceByKey(new Function2<Integer, Integer, Integer>() {
      @Override
      public Integer call(Integer i1, Integer i2) {
        return i1 + i2;
      }
    });
	
	// Saves the created Key-Value pair in text file
    counts.saveAsTextFile(cluster + "/Data/Output/WarPeaceCount");
    ctx.stop();
  }
}
