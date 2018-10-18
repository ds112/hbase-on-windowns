import org.apache.spark.mllib.linalg.distributed.RowMatrix
import org.apache.spark.mllib.linalg.Vectors
	
    val rows = sc.textFile("/Data/Spark/MLLib/Sample_TallSkinny_Data.txt").map { line =>
      val values = line.split(' ').map(_.toDouble)
      Vectors.dense(values)
    }
    val mat = new RowMatrix(rows)

    // Compute principal components.
    val pc = mat.computePrincipalComponents(mat.numCols().toInt)

    println("Principal components are:\n" + pc)