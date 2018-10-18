import java.util.Random
import scala.math.exp
import breeze.linalg.{Vector, DenseVector}
import org.apache.hadoop.conf.Configuration
import org.apache.spark.storage.StorageLevel
  val D = 10   // Numer of dimensions
  val rand = new Random(42)
  def showWarning() {
    System.err.println(
      """WARN: This is a naive implementation of Logistic Regression and is given as an example!
        |Please use either org.apache.spark.mllib.classification.LogisticRegressionWithSGD or
        |org.apache.spark.mllib.classification.LogisticRegressionWithLBFGS
        |for more conventional use.
      """.stripMargin)
  }
  case class DataPoint(x: breeze.linalg.Vector[Double], y: Double)
  def parsePoint(line: String): DataPoint = {
    val tok = new java.util.StringTokenizer(line, " ")
    var y = tok.nextToken.toDouble
    var x = new Array[Double](D)
    var i = 0
    while (i < D) {
      x(i) = tok.nextToken.toDouble; i += 1
    }
    DataPoint(new DenseVector(x), y)
  }
   showWarning()
   
	
    val inputPath = "/Data/Spark/MLLib/LR_Data.txt"  
    val conf = new Configuration() 
    val lines = sc.textFile(inputPath)
    val points = lines.map(parsePoint _).persist(StorageLevel.OFF_HEAP)
    val ITERATIONS = 0
    var w = DenseVector.fill(D){2 * rand.nextDouble - 1}
    println("Initial w: " + w)

    for (i <- 1 to ITERATIONS) {
      println("On iteration " + i)
      val gradient = points.map { p =>
        p.x * (1 / (1 + exp(-p.y * (w.dot(p.x)))) - 1) * p.y
      }.reduce(_ + _)
      w -= gradient
    }
    println("Final w: " + w)