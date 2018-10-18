import org.apache.sqoop.tool.ImportTool;
import com.cloudera.sqoop.SqoopOptions;


@SuppressWarnings("deprecation")
public class SqoopImportJob {

	public static void main(String[] args) throws InstantiationException, IllegalAccessException, ClassNotFoundException {
		 String driver = "com.mysql.jdbc.Driver";
		    Class.forName(driver).newInstance();
		    SqoopOptions options = new SqoopOptions();
		    options.setDriverClassName(driver);		
            options.setConnectString("connectionString");
			// Oracle does not allow the as keyword for table aliasing. For example, select * from dual as d gives ORA-00933: SQL command not properly ended 
			//To avoid above exception,please comment the setTableName query while you run oracle import and uncomment the SetSqlQuery below.
			//options.setSqlQuery("select * from <tablename> where $CONDITIONS");
			options.setTableName("tableName");
		    options.setUsername("userName");
		    options.setPassword("password");
		    options.setTargetDir("/Output/sqoopimport");
		    options.setNumMappers(1);
		    new ImportTool().run(options);
	}
}
