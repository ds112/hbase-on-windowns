import org.apache.sqoop.tool.ExportTool;
import com.cloudera.sqoop.SqoopOptions;


@SuppressWarnings("deprecation")
public class SqoopExportJob {

	public static void main(String[] args) throws InstantiationException, IllegalAccessException, ClassNotFoundException {
		 String driver = "com.mysql.jdbc.Driver";
		    Class.forName(driver).newInstance();
		    SqoopOptions options = new SqoopOptions();
		    options.setDriverClassName(driver);		
		    options.setConnectString("connectionString");
		    options.setTableName("tableName");
		    options.setUsername("userName");
		    options.setPassword("password");
		    options.setNumMappers(1);
		    options.setExportDir("/user/SYSTEM/book");
		    new ExportTool().run(options);

	}

}
