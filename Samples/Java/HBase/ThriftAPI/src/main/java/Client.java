import java.util.ArrayList;
import java.util.List;
import java.nio.ByteBuffer;
import java.nio.charset.StandardCharsets;

import org.apache.thrift.TException;
import org.apache.thrift.protocol.TBinaryProtocol;
import org.apache.thrift.protocol.TProtocol;
import org.apache.thrift.transport.TSocket;
import org.apache.thrift.transport.TTransportException;
import org.apache.hadoop.hbase.thrift.generated.*;

public class Client {
public static void main(String[] args) throws TTransportException,TException {
		   final TSocket tSocket = new TSocket(args[0], Integer.parseInt(args[1]));
	       final TProtocol protocol = new TBinaryProtocol(tSocket);
	       Hbase.Client client = new Hbase.Client(protocol);
	       tSocket.open();
	       
	       ByteBuffer tableName = ByteBuffer.wrap("AdventureWorks_Person_Contact".getBytes());	
	       List<ByteBuffer> tableNames = client.getTableNames();
	       boolean isTableExists = false;
	       for (ByteBuffer table:tableNames)
	       {
	    	   String tab = StandardCharsets.US_ASCII.decode(table).toString();
	    	   if(tableName.equals(tab))
	    	   {
	    		   isTableExists = true;
	    	   }
	       }
	       
	       if(isTableExists != true)
	       {
	       ColumnDescriptor columnDescriptor = new ColumnDescriptor();	
	       List<ColumnDescriptor> columnFamilies = new ArrayList<ColumnDescriptor>();	          
	       columnDescriptor.name = ByteBuffer.wrap("Info:".getBytes());
	       columnFamilies.add(columnDescriptor);
	       columnDescriptor = new ColumnDescriptor();	 	       
	       columnDescriptor.name = ByteBuffer.wrap("contact:".getBytes());
	       columnFamilies.add(columnDescriptor);
	       columnDescriptor = new ColumnDescriptor();	 	      
	       columnDescriptor.name = ByteBuffer.wrap("others:".getBytes());
	       columnFamilies.add(columnDescriptor);	       
	       client.createTable(tableName,columnFamilies);	       
	      
	       List<BatchMutation> batchmutations = new ArrayList<BatchMutation>();
	       BatchMutation batchmutation = new BatchMutation();
	       List<Mutation> mutations = new ArrayList<Mutation>();
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:FULLNAME".getBytes()),ByteBuffer.wrap("Gustavo Achong".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:AGE".getBytes()),ByteBuffer.wrap("38".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:EMAILID".getBytes()),ByteBuffer.wrap("gustavo0@adventure-works.com".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:PHONE".getBytes()),ByteBuffer.wrap("398-555-0132".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:MODIFIEDDATE".getBytes()),ByteBuffer.wrap("5/16/2005 4:33:33 PM".getBytes()),false));
	       batchmutation.mutations = mutations;
	       batchmutation.row = ByteBuffer.wrap("1".getBytes());
	       batchmutations.add(batchmutation);
	       
	       batchmutation = new BatchMutation();
	       mutations = new ArrayList<Mutation>();
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:FULLNAME".getBytes()),ByteBuffer.wrap("Catherine Abel".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:AGE".getBytes()),ByteBuffer.wrap("36".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:EMAILID".getBytes()),ByteBuffer.wrap("catherine0@adventure-works.com".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:PHONE".getBytes()),ByteBuffer.wrap("747-555-0171".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:MODIFIEDDATE".getBytes()),ByteBuffer.wrap("5/16/2005 4:33:33 PM".getBytes()),false));
	       batchmutation.mutations = mutations;
	       batchmutation.row = ByteBuffer.wrap("2".getBytes());
	       batchmutations.add(batchmutation);
	       
	       batchmutation = new BatchMutation();
	       mutations = new ArrayList<Mutation>();
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:FULLNAME".getBytes()),ByteBuffer.wrap("Kim Abercrombie".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:AGE".getBytes()),ByteBuffer.wrap("38".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:EMAILID".getBytes()),ByteBuffer.wrap("kim2@adventure-works.com".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:PHONE".getBytes()),ByteBuffer.wrap("334-555-0137".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:MODIFIEDDATE".getBytes()),ByteBuffer.wrap("5/16/2005 4:33:33 PM".getBytes()),false));
	       batchmutation.mutations = mutations;
	       batchmutation.row = ByteBuffer.wrap("3".getBytes());
	       batchmutations.add(batchmutation);
	       
	       batchmutation = new BatchMutation();
	       mutations = new ArrayList<Mutation>();
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:FULLNAME".getBytes()),ByteBuffer.wrap("Humberto Acevedo".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:AGE".getBytes()),ByteBuffer.wrap("31".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:EMAILID".getBytes()),ByteBuffer.wrap("humberto0@adventure-works.com".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:PHONE".getBytes()),ByteBuffer.wrap("599-555-0127".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:MODIFIEDDATE".getBytes()),ByteBuffer.wrap("5/16/2005 4:33:33 PM".getBytes()),false));
	       batchmutation.mutations = mutations;
	       batchmutation.row = ByteBuffer.wrap("4".getBytes());
	       batchmutations.add(batchmutation);
	       
	       batchmutation = new BatchMutation();
	       mutations = new ArrayList<Mutation>();
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:FULLNAME".getBytes()),ByteBuffer.wrap("Pilar Ackerman".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:AGE".getBytes()),ByteBuffer.wrap("33".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:EMAILID".getBytes()),ByteBuffer.wrap("pilar1@adventure-works.com".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:PHONE".getBytes()),ByteBuffer.wrap("1 (11) 500 555-0132".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:MODIFIEDDATE".getBytes()),ByteBuffer.wrap("5/16/2005 4:33:33 PM".getBytes()),false));
	       batchmutation.mutations = mutations;
	       batchmutation.row = ByteBuffer.wrap("5".getBytes());
	       batchmutations.add(batchmutation);
	       
	       batchmutation = new BatchMutation();
	       mutations = new ArrayList<Mutation>();
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:FULLNAME".getBytes()),ByteBuffer.wrap("Frances Adams".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:AGE".getBytes()),ByteBuffer.wrap("35".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:EMAILID".getBytes()),ByteBuffer.wrap("frances0@adventure-works.com".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:PHONE".getBytes()),ByteBuffer.wrap("991-555-0183".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:MODIFIEDDATE".getBytes()),ByteBuffer.wrap("5/16/2005 4:33:33 PM".getBytes()),false));
	       batchmutation.mutations = mutations;
	       batchmutation.row = ByteBuffer.wrap("6".getBytes());
	       batchmutations.add(batchmutation);
	       
	       batchmutation = new BatchMutation();
	       mutations = new ArrayList<Mutation>();
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:FULLNAME".getBytes()),ByteBuffer.wrap("Margaret Smith".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:AGE".getBytes()),ByteBuffer.wrap("23".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:EMAILID".getBytes()),ByteBuffer.wrap("margaret0@adventure-works.com".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:PHONE".getBytes()),ByteBuffer.wrap("959-555-0151".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:MODIFIEDDATE".getBytes()),ByteBuffer.wrap("5/16/2005 4:33:33 PM".getBytes()),false));
	       batchmutation.mutations = mutations;
	       batchmutation.row = ByteBuffer.wrap("7".getBytes());
	       batchmutations.add(batchmutation);
	       
	       batchmutation = new BatchMutation();
	       mutations = new ArrayList<Mutation>();
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:FULLNAME".getBytes()),ByteBuffer.wrap("Carla Adams".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:AGE".getBytes()),ByteBuffer.wrap("24".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:EMAILID".getBytes()),ByteBuffer.wrap("carla0@adventure-works.com".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:PHONE".getBytes()),ByteBuffer.wrap("107-555-0138".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:MODIFIEDDATE".getBytes()),ByteBuffer.wrap("5/16/2005 4:33:33 PM".getBytes()),false));
	       batchmutation.mutations = mutations;
	       batchmutation.row = ByteBuffer.wrap("8".getBytes());
	       batchmutations.add(batchmutation);
	       
	       batchmutation = new BatchMutation();
	       mutations = new ArrayList<Mutation>();
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:FULLNAME".getBytes()),ByteBuffer.wrap("Jay Adams".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:AGE".getBytes()),ByteBuffer.wrap("40".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:EMAILID".getBytes()),ByteBuffer.wrap("jay1@adventure-works.com".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:PHONE".getBytes()),ByteBuffer.wrap("158-555-0142".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:MODIFIEDDATE".getBytes()),ByteBuffer.wrap("5/16/2005 4:33:33 PM".getBytes()),false));
	       batchmutation.mutations = mutations;
	       batchmutation.row = ByteBuffer.wrap("9".getBytes());
	       batchmutations.add(batchmutation);
	       
	       batchmutation = new BatchMutation();
	       mutations = new ArrayList<Mutation>();
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:FULLNAME".getBytes()),ByteBuffer.wrap("Ronald Adina".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:AGE".getBytes()),ByteBuffer.wrap("41".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:EMAILID".getBytes()),ByteBuffer.wrap("ronald0@adventure-works.com".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:PHONE".getBytes()),ByteBuffer.wrap("453-555-0165".getBytes()),false));
	       mutations.add(new Mutation(false,ByteBuffer.wrap("Info:MODIFIEDDATE".getBytes()),ByteBuffer.wrap("5/16/2005 4:33:33 PM".getBytes()),false));
	       batchmutation.mutations = mutations;
	       batchmutation.row = ByteBuffer.wrap("10".getBytes());
	       batchmutations.add(batchmutation);
	       
	       client.mutateRows(tableName, batchmutations, null);
	       }
	       TScan scan = new TScan();
	       int scannerId = client.scannerOpenWithScan(tableName, scan, null);
	       List<TRowResult> rowResult= client.scannerGet(scannerId);	       
	       if (rowResult != null)
	       {
	           while (!rowResult.isEmpty())
	           {
	               for(TRowResult row :rowResult)
	               {
	            	   System.out.println();	            	  
	                   System.out.print(StandardCharsets.US_ASCII.decode(row.row).toString());	   
	                   for(TCell value : row.getColumns().values())
	                   {
	                	   System.out.print("\t");
	                	   System.out.print(StandardCharsets.US_ASCII.decode(value.value).toString());
	                   }
	               }    
	               rowResult = client.scannerGet(scannerId);
	           }
	       }
	       client.scannerClose(scannerId);     
	       tSocket.close();
	    }
}
