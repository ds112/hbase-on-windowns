import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.nio.charset.StandardCharsets;
import java.util.ArrayList;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.xml.bind.DatatypeConverter;
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;

public class HBaseRestAPI {

	static int port=10005;
	
	 // For Cluster aasign host="<ipaddress/hostname>";
	static String  host ="localhost";
	
    public static void main(String[] args) {
        if (args.length == 1 && args[0].equals("help")) {
            System.out.println("Syntax: JAR <Sample Name>");
            System.out.println("=======\n");
            System.out.println("Sample Name");
            System.out.println("------------");
            System.out.println("NewTable");
            System.out.println("DeleteTable");
            System.out.println("InsertValue");
            System.out.println("GetSingleRow");
            System.out.println("ScanTable");
            System.exit(0);
        } else if (args.length != 1) {
            System.err.println("Syntax incorrect. Run 'help' to get list of operation");
            System.exit(0);
        }
        try {
            executeHBase(args[0].toString());
        } catch (Exception ex) {
            Logger.getLogger(HBaseRestAPI.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    public static void executeHBase(String arg) throws IOException, ParseException {

        if (arg.equalsIgnoreCase("NewTable")) {
            if (IsTableExists("http://"+host+":"+port+"/Customers/exists")) {
                Delete("http://"+host+":"+port+"/Customers/schema");
            }
            String createQuery = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            createQuery += "<TableSchema name=\"Customers\">";
            createQuery += "<ColumnSchema name=\"Info\"/>";
            createQuery += "</TableSchema>";
            Post("http://"+host+":"+port+"/Customers/schema", createQuery);
            System.out.println("Table Created! : Customers");
        } else if (arg.equalsIgnoreCase("DeleteTable")) {
            if (!IsTableExists("http://"+host+":"+port+"/Customers/exists")) {
                String createQuery = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                createQuery += "<TableSchema name=\"Customers\">";
                createQuery += "<ColumnSchema name=\"Info\"/>";
                createQuery += "</TableSchema>";
                Post("http://"+host+":"+port+"/Customers/schema", createQuery);
            }
            Delete("http://"+host+":"+port+"/Customers/schema");
            System.out.println("Table Deleted! : Customers");
        } else if (arg.equalsIgnoreCase("InsertValue")) {
            PopulateTable();
            System.out.println("Value Inserted!");
        } else if (arg.equalsIgnoreCase("GetSingleRow")) {
            PopulateTable();
            String output = Get("http://"+host+":"+port+"/Customers/ALFKI");
            List<Values> list = new ArrayList<Values>();

            if (!output.isEmpty()) {
                JSONParser parser = new JSONParser();
                JSONObject obj = (JSONObject) parser.parse(output);
                JSONArray array = (JSONArray) obj.get("Row");
                obj = (JSONObject) array.get(0);
                JSONArray cell = (JSONArray) obj.get("Cell");
                String key = obj.get("key").toString();

                for (int i = 0; i < cell.size(); i++) {
                    obj = (JSONObject) cell.get(i);
                    String column = obj.get("column").toString();
                    String dollar = obj.get("$").toString();
                    Values value = new Values(Decode(key), Decode(column), Decode(dollar));
                    list.add(value);
                }
            }
            String result = "";
            for (Values val : list) {
                result += val.key + " " + val.column + " " + val.dollar + "\n";
            }
            System.out.println(result);
        } else if (arg.equalsIgnoreCase("ScanTable")) {
            PopulateTable();
            String scannerQuery = "<Scanner batch=\"1\">";
            scannerQuery += "</Scanner>";
            String scannerResponse = Put("http://"+host+":"+port+ "/Customers/scanner", scannerQuery, true);
            String output = Get(scannerResponse);
            List<Values> list = new ArrayList<Values>();

            while (!output.isEmpty() && !output.equals("false")) {
                JSONParser parser = new JSONParser();
                JSONObject obj = (JSONObject) parser.parse(output);
                JSONArray array = (JSONArray) obj.get("Row");
                obj = (JSONObject) array.get(0);
                JSONArray cell = (JSONArray) obj.get("Cell");
                String key = obj.get("key").toString();
                obj = (JSONObject) cell.get(0);
                String column = obj.get("column").toString();
                String dollar = obj.get("$").toString();
                Values value = new Values(Decode(key), Decode(column), Decode(dollar));
                list.add(value);
                output = Get(scannerResponse);
            }
            String result = "";
            for (Values val : list) {
                result += val.key + " " + val.column + " " + val.dollar + "\n";
            }
            System.out.println(result);
        }
    }

    private static void PopulateTable() throws IOException, ParseException {
        executeHBase("NewTable");
        String insertQuery = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
        insertQuery += "<CellSet>";
        insertQuery += "<Row key=\"" + Encode("ALFKI") + "\">";
        insertQuery += "<Cell column=\"" + Encode("Info:companyName") + "\">" + Encode("Alfred Futterkiste") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:contactName") + "\">" + Encode("Maria Anders") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:contactTIT") + "\">" + Encode("Sales Representative") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:address") + "\">" + Encode("Obere Str 57") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:city") + "\">" + Encode("Berlin") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:region") + "\">" + Encode("") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:postalCode") + "\">" + Encode("12209") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:country") + "\">" + Encode("Germany") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:phone") + "\">" + Encode("030-0074321") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:fax") + "\">" + Encode("030-0076545") + "</Cell>";
        insertQuery += "</Row>";
        insertQuery += "<Row key=\"" + Encode("DRACD") + "\">";
        insertQuery += "<Cell column=\"" + Encode("Info:companyName") + "\">" + Encode("Drachenblut Delikatessen") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:contactName") + "\">" + Encode("Sven Ottlieb") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:contactTIT") + "\">" + Encode("Order Administrator") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:address") + "\">" + Encode("Walserweg 21") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:city") + "\">" + Encode("Aachen") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:region") + "\">" + Encode("") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:postalCode") + "\">" + Encode("52066") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:country") + "\">" + Encode("Germany") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:phone") + "\">" + Encode("0241-039123") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:fax") + "\">" + Encode("0241-059428") + "</Cell>";
        insertQuery += "</Row>";
        insertQuery += "<Row key=\"" + Encode("BONAP") + "\">";
        insertQuery += "<Cell column=\"" + Encode("Info:companyName") + "\">" + Encode("Bon app") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:contactName") + "\">" + Encode("Laurence Lebihan") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:contactTIT") + "\">" + Encode("Owner") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:address") + "\">" + Encode("12") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:city") + "\">" + Encode("rue des Bouchers") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:region") + "\">" + Encode("Marseille") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:postalCode") + "\">" + Encode("13008") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:country") + "\">" + Encode("France") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:phone") + "\">" + Encode("91.24.45.40") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:fax") + "\">" + Encode("") + "</Cell>";
        insertQuery += "</Row>";
        insertQuery += "<Row key=\"" + Encode("BOTTM") + "\">";
        insertQuery += "<Cell column=\"" + Encode("Info:companyName") + "\">" + Encode("Bottom-Dollar Markets") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:contactName") + "\">" + Encode("Elizabeth Lincoln") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:contactTIT") + "\">" + Encode("Accounting Manager") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:address") + "\">" + Encode("23 Tsawassen Blvd") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:city") + "\">" + Encode("Tsawassen") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:region") + "\">" + Encode("BC") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:postalCode") + "\">" + Encode("T2F 8M4") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:country") + "\">" + Encode("Canada") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:phone") + "\">" + Encode("(604) 555-4729") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:fax") + "\">" + Encode("(604) 555-3745") + "</Cell>";
        insertQuery += "</Row>";
        insertQuery += "<Row key=\"" + Encode("FRANR") + "\">";
        insertQuery += "<Cell column=\"" + Encode("Info:companyName") + "\">" + Encode("France restauration") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:contactName") + "\">" + Encode("Carine Schmitt") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:contactTIT") + "\">" + Encode("Marketing Manager") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:address") + "\">" + Encode("54\\t rue Royale") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:city") + "\">" + Encode("Nantes") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:region") + "\">" + Encode("") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:postalCode") + "\">" + Encode("8010") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:country") + "\">" + Encode("Austria") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:phone") + "\">" + Encode("7675-3425") + "</Cell>";
        insertQuery += "<Cell column=\"" + Encode("Info:fax") + "\">" + Encode("7675-3426") + "</Cell>";
        insertQuery += "</Row>";
        insertQuery += "</CellSet>";
        Put("http://"+host+":"+port+"/Customers/Row1", insertQuery, false);
    }

	private static boolean IsTableExists(String url) {
        try {
            String isTableExists = Get(url);
            if (isTableExists.equals(("false"))) {
                return false;
            } else {
                return true;
            }
        } catch (Exception ex) {
            return false;
        }
    }
	
    private static String Get(String url) throws IOException {
        String outputResponse = "";
        URL obj = new URL(url);
        HttpURLConnection con = (HttpURLConnection) obj.openConnection();
        con.setRequestMethod("GET");
        con.setRequestProperty("Accept", "application/json");
        int responseCode = con.getResponseCode();
        if (responseCode == HttpURLConnection.HTTP_OK) { // success
            BufferedReader in = new BufferedReader(new InputStreamReader(
                    con.getInputStream()));
            String inputLine;
            StringBuffer response = new StringBuffer();

            while ((inputLine = in.readLine()) != null) {
                response.append(inputLine + "\n");
            }
            in.close();
            outputResponse = response.toString();
        } else {
            return "false";
        }
        return outputResponse;
    }

    private static String Delete(String url) throws IOException {
        String outputResponse = "";
        URL obj = new URL(url);
        HttpURLConnection con = (HttpURLConnection) obj.openConnection();
        con.setRequestMethod("DELETE");
        int responseCode = con.getResponseCode();
        if (responseCode == HttpURLConnection.HTTP_OK) { // success
            BufferedReader in = new BufferedReader(new InputStreamReader(
                    con.getInputStream()));
            String inputLine;
            StringBuffer response = new StringBuffer();

            while ((inputLine = in.readLine()) != null) {
                response.append(inputLine + "\n");
            }
            in.close();
            outputResponse = response.toString();
        }
        return outputResponse;

    }

    private static String Post(String url, String responseMessage) throws IOException {
        String outputResponse = "";
        URL obj = new URL(url);
        HttpURLConnection con = (HttpURLConnection) obj.openConnection();
        con.setRequestMethod("POST");
        con.setDoOutput(true);
        con.setRequestProperty("Content-Type", "text/xml; charset=UTF-8");
        con.setRequestProperty("Content-Length", String.valueOf(responseMessage.length()));
        OutputStream os = con.getOutputStream();
        os.write(responseMessage.getBytes());
        int responseCode = con.getResponseCode();
        
        if (responseCode == HttpURLConnection.HTTP_OK) { //success
            BufferedReader in = new BufferedReader(new InputStreamReader(
                    con.getInputStream()));
            String inputLine;
            StringBuffer response = new StringBuffer();

            while ((inputLine = in.readLine()) != null) {
                response.append(inputLine);
            }
            in.close();

            outputResponse = response.toString();
        }
        os.flush();
        os.close();
        return outputResponse;
    }

    private static String Put(String url, String responseMessage, boolean isScanner) throws IOException {
        String outputResponse = "";
        URL obj = new URL(url);
        HttpURLConnection con = (HttpURLConnection) obj.openConnection();
        con.setRequestMethod("PUT");
        con.setDoOutput(true);
        con.setRequestProperty("Content-Type", "text/xml; charset=UTF-8");
        con.setRequestProperty("Content-Length", String.valueOf(responseMessage.length()));
        OutputStream os = con.getOutputStream();
        os.write(responseMessage.getBytes());
        int responseCode = con.getResponseCode();
        if (isScanner) {
            os.flush();
            os.close();
            return con.getHeaderField("Location");
        }

        if (responseCode == HttpURLConnection.HTTP_OK) { //success
            BufferedReader in = new BufferedReader(new InputStreamReader(
                    con.getInputStream()));
            String inputLine;
            StringBuffer response = new StringBuffer();
            while ((inputLine = in.readLine()) != null) {
                response.append(inputLine);
            }
            in.close();
            outputResponse = response.toString();
        }
        os.flush();
        os.close();
        return outputResponse;
    }

    private static String Encode(String value) {

        return DatatypeConverter.printBase64Binary(value.getBytes(StandardCharsets.UTF_8));

    }

    private static String Decode(String value) {
        byte[] decodedValue = DatatypeConverter.parseBase64Binary(value);
        return new String(decodedValue, StandardCharsets.UTF_8);
    }

    private static class Values {

        String key;
        String column;
        String dollar;

        public Values(String key, String column, String dollar) {
            this.key = key;
            this.column = column;
            this.dollar = dollar;
        }
    }
}
