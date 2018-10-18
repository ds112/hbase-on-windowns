package com.syncfusion;

import java.util.regex.Pattern;

import org.apache.spark.SparkConf;
import org.apache.spark.api.java.JavaRDD;
import org.apache.spark.api.java.JavaSparkContext;
import org.apache.spark.api.java.function.Function;

import org.apache.spark.mllib.classification.LogisticRegressionWithSGD;
import org.apache.spark.mllib.classification.LogisticRegressionModel;
import org.apache.spark.mllib.linalg.Vectors;
import org.apache.spark.mllib.regression.LabeledPoint;

/**
 * Logistic regression based classification using ML Lib.
 */
public final class JavaLR {

  static class ParsePoint implements Function<String, LabeledPoint> {
    private static final Pattern COMMA = Pattern.compile(",");
    private static final Pattern SPACE = Pattern.compile(" ");

    @Override
    public LabeledPoint call(String line) {
      String[] parts = COMMA.split(line);
      double y = Double.parseDouble(parts[0]);
      String[] tok = SPACE.split(parts[1]);
      double[] x = new double[tok.length];
      for (int i = 0; i < tok.length; ++i) {
        x[i] = Double.parseDouble(tok[i]);
      }
      return new LabeledPoint(y, Vectors.dense(x));
    }
  }

  public static void main(String[] args) {
    if (args.length != 3) {
      args = new String[3];
      //For remote cluster set remote host_name:port instead of localhost:9000
      args[0] = "hdfs://localhost:9000/Data/Spark/MLLib/LR-Data/Random.data";
      args[1] = "5";
      args[2] = "10";
    }
    SparkConf sparkConf = new SparkConf().setAppName("JavaLR");
    //To run this jar with 'spark-submit' set master to 'yarn-cluster'
    sparkConf.setMaster("local");
    JavaSparkContext sc = new JavaSparkContext(sparkConf);
    JavaRDD<String> lines = sc.textFile(args[0]);
    JavaRDD<LabeledPoint> points = lines.map(new ParsePoint()).cache();
    double stepSize = Double.parseDouble(args[1]);
    int iterations = Integer.parseInt(args[2]);

    // Another way to configure LogisticRegression
    //
    // LogisticRegressionWithSGD lr = new LogisticRegressionWithSGD();
    // lr.optimizer().setNumIterations(iterations)
    //               .setStepSize(stepSize)
    //               .setMiniBatchFraction(1.0);
    // lr.setIntercept(true);
    // LogisticRegressionModel model = lr.train(points.rdd());

    LogisticRegressionModel model = LogisticRegressionWithSGD.train(points.rdd(),
      iterations, stepSize);

    System.out.print("Final w: " + model.weights());

    sc.stop();
  }
}
