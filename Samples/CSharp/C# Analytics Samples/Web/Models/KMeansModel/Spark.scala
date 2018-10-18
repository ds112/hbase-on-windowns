import org.apache.spark.mllib.clustering.{KMeans, KMeansModel}
import org.apache.spark.mllib.linalg.Vectors

// Load and parse the data
val data = sc.textFile("data/mllib/kmeans_data.txt")
val parsedData = data.map(s => Vectors.dense(s.split(' ').map(_.toDouble))).cache()

// Cluster the data into two classes using KMeans
val numClusters = 2
val numIterations = 20
val clusters = KMeans.train(parsedData, numClusters, numIterations)

// Prediction and Input Label
val predictionAndLabel = parsedData.map(p => (clusters.predict(p)))

// Save Prediction
predictionAndLabel.saveAsTextFile("data/mllib/kmeans_data")

// Evaluate clustering by computing Within Set Sum of Squared Errors
val WSSSE = clusters.computeCost(parsedData)
println("Within Set Sum of Squared Errors = " + WSSSE)

// Save and load model
clusters.save(sc, "myModelPath")
val sameModel = KMeansModel.load(sc, "myModelPath")

// Generate PMML
clusters.toPMML("data/mllib/kmeans_data.pmml")