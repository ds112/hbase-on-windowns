package com.syncfusion;

import java.sql.SQLException;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.Statement;
import java.sql.DriverManager;

public final class Recommend {
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
	    String tableName1 = "recommend_Ratings1";
	    String tableName2 = "recommend_Ratings2";
	    String sql = "";
	    ResultSet res;
	    
	    stmt.execute("create External table if not exists " + tableName1 + "(criticid string,movieid string,rating double) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/Ratings/'");
	    stmt.execute("create External table if not exists " + tableName2 + "(criticid string,movieid string,rating double) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/Ratings/'");
		
	    // select * query
	    sql = "select recommend_Ratings1.movieid as movieid1,recommend_Ratings2.movieid as movieid2,corr(recommend_Ratings1.rating,recommend_Ratings2.rating) as correlation from recommend_Ratings1 join recommend_Ratings2 ON (recommend_Ratings1.criticid = recommend_Ratings2.criticid) where recommend_Ratings1.movieid < recommend_Ratings2.movieid GROUP by recommend_Ratings1.movieid,recommend_Ratings2.movieid";
	    System.out.println("Running: " + sql);
	    res = stmt.executeQuery(sql);
	    while (res.next()) {
	      System.out.println(String.valueOf(res.getString(1)) + "\t" + res.getString(2)+ "\t" + res.getString(3));
	    }	 
	  }
}
			