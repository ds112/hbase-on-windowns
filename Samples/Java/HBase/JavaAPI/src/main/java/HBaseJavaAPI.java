import java.io.IOException;
import java.util.logging.Level;
import java.util.logging.Logger;
import org.apache.hadoop.conf.Configuration;
import org.apache.hadoop.hbase.*;
import org.apache.hadoop.hbase.client.*;
import org.apache.hadoop.hbase.util.Bytes;

public class HBaseJavaAPI {

	public static void main(String[] args) {
		if(args.length==1 && args[0].equals("help")){
			System.out.println("Syntax: JAR <Sample Name>");
			System.out.println("=======\n");
			System.out.println("Sample Name");
			System.out.println("------------");
			System.out.println("NewTable");
			System.out.println("AlterTable");
			System.out.println("DeleteTable");
			System.out.println("InsertValue");
			System.out.println("GetSingleRow");
			System.out.println("ScanTable");
			System.exit(0);
		}
		else if(args.length!=1){
			System.err.println("Syntax incorrect. Run 'help' to get list of operation");
			System.exit(0);
		}
		try {
			Configuration con = HBaseConfiguration.create();
			HBaseAdmin admin = new HBaseAdmin(con);
			executeHBase(args[0].toString(), admin, con);
		} catch (Exception ex) {
			Logger.getLogger(HBaseJavaAPI.class.getName()).log(Level.SEVERE, null, ex);
		}
	}

	private static void executeHBase(String arg, HBaseAdmin admin,Configuration config) throws IOException {
		if (arg.equalsIgnoreCase("NewTable")) {
			HTableDescriptor table = new HTableDescriptor(TableName.valueOf("Customers"));
			table.addFamily(new HColumnDescriptor("ColumnFamily1"));
			if(admin.tableExists("Customers")){
				admin.disableTable("Customers");
				admin.deleteTable("Customers");
			}
			admin.createTable(table);
			System.out.println("Table Created! : Customers");
		}
		else if (arg.equalsIgnoreCase("AlterTable")) {
			HTableDescriptor table = new HTableDescriptor(TableName.valueOf("SampleTable"));
			table.addFamily(new HColumnDescriptor("ColumnFamily1"));
			System.out.println("Table " + table.getTableName() + " exist: " +
					admin.tableExists(table.getTableName()));
			System.out.println("Creating " + table.getTableName() + " table...");
			admin.createTable(table);
			System.out.println("Table " + table.getTableName() + " exist: " +
					admin.tableExists(table.getTableName()));
			System.out.println(admin.getTableDescriptor(table.getTableName()));
			table.addFamily(new HColumnDescriptor("ColumnFamily2"));
			admin.disableTable(table.getTableName());
			admin.deleteTable(table.getTableName());
		}
		else if (arg.equalsIgnoreCase("DeleteTable")) {
			if(!admin.tableExists("Customers")) {
				HTableDescriptor table = new HTableDescriptor(TableName.valueOf("Customers"));
				table.addFamily(new HColumnDescriptor("Info"));
				admin.createTable(table);
				System.out.println("Table Created!");
			}
			admin.disableTable("Customers");
			admin.deleteTable("Customers");
		}
		else if (arg.equalsIgnoreCase("InsertValue")) {
			PopulateTable(admin, config);
			admin.disableTable("Customers");
			admin.deleteTable("Customers");
		}
		else if(arg.equalsIgnoreCase("GetSingleRow")) {
			HTable table = PopulateTable(admin, config);
			Get get = new Get(Bytes.toBytes("ALFKI"));
			Result rs = table.get(get);
			for(KeyValue kv : rs.raw()) {
				System.out.print(new String(kv.getRow()) + " ");
				System.out.print(new String(kv.getFamily()) + ":");
				System.out.print(new String(kv.getQualifier()) + " ");
				System.out.print(kv.getTimestamp() + " ");
				System.out.println(new String(kv.getValue()));
			}
			admin.disableTable("Customers");
			admin.deleteTable("Customers");
		}
		else if(arg.equalsIgnoreCase("ScanTable")) {
			HTable table = new HTable(config, "Customers");
			PopulateTable(admin, config);
			Scan s = new Scan();
			ResultScanner ss = table.getScanner(s);
			for(Result r:ss) {
				for (KeyValue kv : r.raw()) {
					System.out.print(new String(kv.getRow()) + " ");
					System.out.print(new String(kv.getFamily()) + ":");
					System.out.print(new String(kv.getQualifier()) + " ");
					System.out.print(kv.getTimestamp() + " ");
					System.out.println(new String(kv.getValue()));
				}
			}
		}
		else{
			System.err.println("Syntax incorrect. Run 'help' to get list of operation");
			System.exit(0);
		}
	}

	private static HTable PopulateTable(HBaseAdmin admin,Configuration config) throws IOException{
		HTableDescriptor tableDescriptor = new HTableDescriptor(TableName.valueOf("Customers"));
		tableDescriptor.addFamily(new HColumnDescriptor("Info"));
		admin.createTable(tableDescriptor);
		System.out.println("Table Created!");
		HTable table = new HTable(config, "Customers");
		Put p = new Put(Bytes.toBytes("ALFKI"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("companyName"),Bytes.toBytes("Alfred Futterkiste"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("contactName"),Bytes.toBytes("Maria Anders"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("contactTIT"),Bytes.toBytes("Sales Representative"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("address"),Bytes.toBytes("Obere Str 57"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("city"),Bytes.toBytes("Berlin"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("region"),Bytes.toBytes(""));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("postalCode"),Bytes.toBytes("12209"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("country"),Bytes.toBytes("Germany"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("phone"),Bytes.toBytes("030-0074321"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("fax"),Bytes.toBytes("030-0076545"));
		table.put(p);
		p = new Put(Bytes.toBytes("DRACD"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("companyName"),Bytes.toBytes("Drachenblut Delikatessen"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("contactName"),Bytes.toBytes("Sven Ottlieb"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("contactTIT"),Bytes.toBytes("Order Administrator"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("address"),Bytes.toBytes("Walserweg 21"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("city"),Bytes.toBytes("Aachen"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("region"),Bytes.toBytes(""));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("postalCode"),Bytes.toBytes("52066"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("country"),Bytes.toBytes("Germany"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("phone"),Bytes.toBytes("0241-039123"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("fax"),Bytes.toBytes("0241-059428"));
		table.put(p);
		p = new Put(Bytes.toBytes("BONAP"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("companyName"),Bytes.toBytes("Bon app"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("contactName"),Bytes.toBytes("Laurence Lebihan"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("contactTIT"),Bytes.toBytes("Owner"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("address"),Bytes.toBytes("12"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("city"),Bytes.toBytes("rue des Bouchers"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("region"),Bytes.toBytes("Marseille"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("postalCode"),Bytes.toBytes("13008"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("country"),Bytes.toBytes("France"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("phone"),Bytes.toBytes("91.24.45.40"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("fax"),Bytes.toBytes(""));
		table.put(p);
		p = new Put(Bytes.toBytes("BOTTM"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("companyName"),Bytes.toBytes("Bottom-Dollar Markets"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("contactName"),Bytes.toBytes("Elizabeth Lincoln"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("contactTIT"),Bytes.toBytes("Accounting Manager"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("address"),Bytes.toBytes("23 Tsawassen Blvd"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("city"),Bytes.toBytes("Tsawassen"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("region"),Bytes.toBytes("BC"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("postalCode"),Bytes.toBytes("T2F 8M4"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("country"),Bytes.toBytes("Canada"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("phone"),Bytes.toBytes("(604) 555-4729"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("fax"),Bytes.toBytes("(604) 555-3745"));
		table.put(p);
		p = new Put(Bytes.toBytes("FRANR"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("companyName"),Bytes.toBytes("France restauration"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("contactName"),Bytes.toBytes("Carine Schmitt"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("contactTIT"),Bytes.toBytes("Marketing Manager"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("address"),Bytes.toBytes("54\t rue Royale"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("city"),Bytes.toBytes("Nantes"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("region"),Bytes.toBytes(""));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("postalCode"),Bytes.toBytes("8010"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("country"),Bytes.toBytes("Austria"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("phone"),Bytes.toBytes("7675-3425"));
		p.add(Bytes.toBytes("Info"),Bytes.toBytes("fax"),Bytes.toBytes("7675-3426"));
		table.put(p);
		Scan s = new Scan();
		s.addColumn(Bytes.toBytes("Info"), Bytes.toBytes("companyName"));
		s.addColumn(Bytes.toBytes("Info"), Bytes.toBytes("contactName"));
		s.addColumn(Bytes.toBytes("Info"), Bytes.toBytes("contactTIT"));
		s.addColumn(Bytes.toBytes("Info"), Bytes.toBytes("address"));
		s.addColumn(Bytes.toBytes("Info"), Bytes.toBytes("city"));
		s.addColumn(Bytes.toBytes("Info"), Bytes.toBytes("region"));
		s.addColumn(Bytes.toBytes("Info"), Bytes.toBytes("postalCode"));
		s.addColumn(Bytes.toBytes("Info"), Bytes.toBytes("country"));
		s.addColumn(Bytes.toBytes("Info"), Bytes.toBytes("phone"));
		s.addColumn(Bytes.toBytes("Info"), Bytes.toBytes("fax"));

		ResultScanner scanner = table.getScanner(s);
		try {
			for (Result rr = scanner.next(); rr != null; rr = scanner.next()) {
				System.out.println("Found row: " + rr);
			}
		} finally {
			scanner.close();
			table.close();
		}

		return table;
	}
}

