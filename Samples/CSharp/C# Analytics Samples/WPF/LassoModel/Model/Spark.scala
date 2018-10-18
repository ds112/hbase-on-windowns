import org.apache.spark.mllib.regression.LabeledPoint
import org.apache.spark.mllib.regression.LassoModel
import org.apache.spark.mllib.regression.LassoWithSGD
import org.apache.spark.mllib.linalg.Vectors

// Load and parse the data
val data = sc.textFile("data/mllib/ridge-data/lpsa.data")
val parsedData = data.map { line =>
  val parts = line.split(',')
  LabeledPoint(parts(0).toDouble, Vectors.dense(parts(1).split(' ').map(_.toDouble)))
}.cache()

// Building the model
val algorithm = new LassoWithSGD()
algorithm.setIntercept(true)
val model = algorithm.run(parsedData)

// Evaluate model on training examples and compute training error
val valuesAndPreds = parsedData.map { point =>
  val prediction = model.predict(point.features)
  (point.label, prediction)
}

// Save Prediction and Label
valuesAndPreds.saveAsTextFile("data/mllib/ridge-data/lpsa")

// Generate PMML
model.toPMML("data/mllib/ridge-data/lpsa.pmml")

val MSE = valuesAndPreds.map{case(v, p) => math.pow((v - p), 2)}.mean()
println("training Mean Squared Error = " + MSE)

// Save and load model
model.save(sc, "myModelPath")
val sameModel = LassoModel.load(sc, "myModelPath")

