package com.syncfusion;

import java.sql.SQLException;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.Statement;
import java.sql.DriverManager;

public final class CreateTable {
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
	    //To connect with remote cluster give machine IP instead of 'localhost'
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
	    String tableName = "AdventureWorks_Person_Contact";
	    String sql = "";
	    ResultSet res;
	    stmt.execute("create external table IF NOT EXISTS AdventureWorks_Person_Contact(contactid int,fullname string,age int,emailaddress string,phoneno string,modifieddate string) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/AdventureWorks'");
	    
	    System.out.println("Table '" + tableName + "' created successfully\n\n");
	    // select * query
	    sql = "select * from " + tableName;
	    System.out.println("Running: " + sql);
	    res = stmt.executeQuery(sql);
	    while (res.next()) {
	      System.out.println(String.valueOf(res.getInt(1)) + "\t" + res.getString(2) + "\t" + res.getString(3) + "\t" + res.getString(4));
	    }
	 
	    // regular hive query
	    sql = "select count(1) from " + tableName;
	    System.out.println("Running: " + sql);
	    res = stmt.executeQuery(sql);
	    while (res.next()) {
	      System.out.println(res.getString(1));
	    }
	  }
}
			