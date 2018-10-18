package com.syncfusion;

import java.sql.SQLException;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.Statement;
import java.sql.DriverManager;

public final class FindDistinct {
    private static String driverName = "org.apache.hive.jdbc.HiveDriver";
	 
	  /**
	   * @param args
	   * @throws SQLException
	   */
	  public static void main(String[] args) throws SQLException {
	      try {
	      Class.forName(driverName);
	    } catch (ClassNotFoundException e) {
	      // TODO Auto-generated catch block
	      e.printStackTrace();
	      System.exit(1);
	    }
            
            //replace "hive" here with the name of the user the queries should run as
	     String mode = "" , username="" , password="";
          if(args.length>1)
          {
              mode= args[1];
              username=args[2];
              password=args[3];
          }
          else
          {
              mode= "";
              username="";
              password="";
              
          }
            Connection con;
            if(mode.equalsIgnoreCase("kerberos")&& !username.isEmpty() && !password.isEmpty())
                {
	    con =DriverManager.getConnection("jdbc:hive2://"+args[0]+":10000/default;",username,password);
            }
            else
            {
            con=DriverManager.getConnection("jdbc:hive2://"+args[0]+":10000/default;auth=noSasl");
            }
	    Statement stmt = con.createStatement();
	    String tableName = "Customers";
	    String sql = "";
	    ResultSet res;
	    
	    stmt.execute("drop table if exists " + tableName);
	    stmt.execute("CREATE EXTERNAL TABLE IF NOT EXISTS " + tableName + " (CUSTOMERID string,COMPANYNAME string,CONTACTNAME string,CONTACTTIT string,ADDRESS string,CITY string,REGION string,POSTALCODE string,COUNTRY string,PHONE string,FAX string) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/Customers'");
	    
	    // select Distinct query
	    sql = "select DISTINCT(COMPANYNAME) from " + tableName;
	    System.out.println("Running: " + sql);
	    res = stmt.executeQuery(sql);
	    while (res.next()) {
	      System.out.println(String.valueOf(res.getString(1)));
	    }
	  }
}
			