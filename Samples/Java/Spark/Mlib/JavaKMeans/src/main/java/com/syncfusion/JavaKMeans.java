package com.syncfusion;

import java.util.regex.Pattern;

import org.apache.spark.SparkConf;
import org.apache.spark.api.java.JavaRDD;
import org.apache.spark.api.java.JavaSparkContext;
import org.apache.spark.api.java.function.Function;

import org.apache.spark.mllib.clustering.KMeans;
import org.apache.spark.mllib.clustering.KMeansModel;
import org.apache.spark.mllib.linalg.Vector;
import org.apache.spark.mllib.linalg.Vectors;

/**
 * Example using MLlib KMeans from Java.
 */
public final class JavaKMeans {

  private static class ParsePoint implements Function<String, Vector> {
    private static final Pattern SPACE = Pattern.compile(" ");

    @Override
    public Vector call(String line) {
      String[] tok = SPACE.split(line);
      double[] point = new double[tok.length];
      for (int i = 0; i < tok.length; ++i) {
        point[i] = Double.parseDouble(tok[i]);
      }
      return Vectors.dense(point);
    }
  }

  public static void main(String[] args) {
    if (args.length < 3) {
      args = new String[3];
      //For remote cluster set remote host_name:port instead of localhost:9000
      args[0] = "hdfs://localhost:9000/Data/Spark/MLLib/KMeans_Data.txt";
      args[1] = "5";
      args[2] = "3";
    }
    String inputFile = args[0];
    int k = Integer.parseInt(args[1]);
    int iterations = Integer.parseInt(args[2]);
    int runs = 1;

    if (args.length >= 4) {
      runs = Integer.parseInt(args[3]);
    }
    SparkConf sparkConf = new SparkConf().setAppName("JavaKMeans");
    //To run this jar with 'spark-submit' set master to 'yarn-cluster'
    sparkConf.setMaster("local");
    JavaSparkContext sc = new JavaSparkContext(sparkConf);
    JavaRDD<String> lines = sc.textFile(inputFile);

    JavaRDD<Vector> points = lines.map(new ParsePoint()).cache();

    KMeansModel model = KMeans.train(points.rdd(), k, iterations, runs, KMeans.K_MEANS_PARALLEL());

    System.out.println("Cluster centers:");
    for (Vector center : model.clusterCenters()) {
      System.out.println(" " + center);
    }
    double cost = model.computeCost(points.rdd());
    System.out.println("Cost: " + cost);

    sc.stop();
  }
}
