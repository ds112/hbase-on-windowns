package com.syncfusion;

import org.apache.spark.SparkConf;
import org.apache.spark.SparkContext;
import org.apache.spark.api.java.JavaRDD;
import org.apache.spark.api.java.JavaSparkContext;
import org.apache.spark.api.java.function.Function;

import org.apache.spark.mllib.recommendation.ALS;
import org.apache.spark.mllib.recommendation.MatrixFactorizationModel;
import org.apache.spark.mllib.recommendation.Rating;

import java.util.Arrays;
import java.util.regex.Pattern;

import scala.Tuple2;

/**
 * Example using MLlib ALS from Java.
 */
public final class JavaALS {

  static class ParseRating implements Function<String, Rating> {
    private static final Pattern COMMA = Pattern.compile(",");

    @Override
    public Rating call(String line) {
      String[] tok = COMMA.split(line);
      int x = Integer.parseInt(tok[0]);
      int y = Integer.parseInt(tok[1]);
      double rating = Double.parseDouble(tok[2]);
      return new Rating(x, y, rating);
    }
  }

  static class FeaturesToString implements Function<Tuple2<Object, double[]>, String> {
    @Override
    public String call(Tuple2<Object, double[]> element) {
      return element._1() + "," + Arrays.toString(element._2());
    }
  }

  public static void main(String[] args) {

    if (args.length < 4) {
      args = new String[4];
      //For remote cluster set remote host_name:port instead of localhost:9000
      args[0] = "hdfs://localhost:9000/Data/Spark/MLLib/ALS/Test.data";
      args[1] = "2";
      args[2] = "5";
      args[3] = "hdfs://localhost:9000/Data/Output/ALSJavaOutput";
    }
    SparkConf sparkConf = new SparkConf().setAppName("JavaALS");
    //To run this jar with 'spark-submit' set master to 'yarn-cluster'
    sparkConf.setMaster("local");
    int rank = Integer.parseInt(args[1]);
    int iterations = Integer.parseInt(args[2]);
    String outputDir = args[3];
    int blocks = -1;
    if (args.length == 5) {
      blocks = Integer.parseInt(args[4]);
    }

    JavaSparkContext sc = new JavaSparkContext(sparkConf);
    JavaRDD<String> lines = sc.textFile(args[0]);

    JavaRDD<Rating> ratings = lines.map(new ParseRating());

    MatrixFactorizationModel model = ALS.train(ratings.rdd(), rank, iterations, 0.01, blocks);

    model.userFeatures().toJavaRDD().map(new FeaturesToString()).saveAsTextFile(
        outputDir + "/userFeatures");
    model.productFeatures().toJavaRDD().map(new FeaturesToString()).saveAsTextFile(
        outputDir + "/productFeatures");
    System.out.println("Final user/product features written to " + outputDir);

    sc.stop();
  }
}
