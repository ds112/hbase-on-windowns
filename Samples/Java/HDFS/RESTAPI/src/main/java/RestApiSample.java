import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.File;
import java.io.InputStreamReader;
import java.net.URL;
import java.net.HttpURLConnection;

public class RestApiSample {

    public static void main(String[] args) {
        String mode="", url = "", destinationPath="",sourcePath="";
        
        System.out.println("\nSending Http[s] request");

        if(args.length==1 && args[0].equals("help")){
            System.out.println("Syntax: runJAR <host> <Operation Type> <Authentication Type>");
            System.out.println("=======\n");
            System.out.println("Operation Type");
            System.out.println("--------------");
            System.out.println("upload");
            System.out.println("delete");
            System.out.println("mkdir");
            System.out.println("move");
            System.out.println("setreplication");
            System.out.println("Authentication Type");
            System.out.println("--------------");
            System.out.println("Kerberos");
            System.out.println("Simple");
            System.exit(0);
        }
        else if(args.length==2)
            mode="";
        else if(args.length>2)
            mode=args[2];
        
        String host=args[0]; 
        try{
            switch(args[1]){
                case "upload":
                    File path = new File(System.getProperty("user.dir"));
                    if(path.toString()!="")
                    {
                        sourcePath = path.toString().substring(0, path.toString().lastIndexOf("Samples")+7)+ File.separator + "Data\\WarPeace.txt";
                    }
                    destinationPath="/javarest/war.txt";
                    System.out.println("Uploading file: "+sourcePath);
                    if(mode.equalsIgnoreCase("kerberos"))
                    url = "https://"+host+":50470/webhdfs/v1"+destinationPath+"?op=CREATE&overwrite=true";
                    else
                    url = "http://"+host+":50070/webhdfs/v1"+destinationPath+"?op=CREATE&overwrite=true";
                    RestApiSample.sendPutRequest(url, sourcePath);
                    System.out.println("Upload operation completed");
                    break;
               case "delete":
                    destinationPath="/javarest";
                    System.out.println("Deleting file: "+destinationPath);
                    if(mode.equalsIgnoreCase("kerberos"))
                    url = "https://"+host+":50470/webhdfs/v1"+destinationPath+"?op=DELETE&recursive=true";
                    else
                    url = "http://"+host+":50070/webhdfs/v1"+destinationPath+"?op=DELETE&recursive=true";
                    RestApiSample.sendRequest(url,"DELETE");
                    System.out.println("Delete operation completed");
                    break;
               case "mkdir":
                    destinationPath="/javarest";
                    System.out.println("Create directory: "+destinationPath);
                    if(mode.equalsIgnoreCase("kerberos"))
                    url = "https://"+host+":50470/webhdfs/v1"+destinationPath+"?op=MKDIRS&permission=777";
                    else
                    url = "http://"+host+":50070/webhdfs/v1"+destinationPath+"?op=MKDIRS&permission=777";
                    RestApiSample.sendRequest(url,"PUT");
                    System.out.println("Create directory operation completed");
                    break;
                case "move":
                    sourcePath="/javarest";
                    destinationPath="/javarest2";
                    System.out.println("Move : "+destinationPath);
                    if(mode.equalsIgnoreCase("kerberos"))
                    url = "https://"+host+":50470/webhdfs/v1"+sourcePath+"?op=RENAME&destination="+destinationPath;
                    else
                    url = "http://"+host+":50070/webhdfs/v1"+sourcePath+"?op=RENAME&destination="+destinationPath;
                    RestApiSample.sendRequest(url,"PUT");
                    System.out.println("Move operation completed");
                    break;
                case "setreplication":
                    sourcePath="/javarest/war.txt";
                    System.out.println("Move : "+destinationPath);
                    if(mode.equalsIgnoreCase("kerberos"))
                    url = "https://"+host+":50470/webhdfs/v1"+sourcePath+"?op=SETREPLICATION&&replication=3";
                    else
                    url = "http://"+host+":50070/webhdfs/v1"+sourcePath+"?op=SETREPLICATION&&replication=3";
                    RestApiSample.sendRequest(url,"PUT");
                    System.out.println("Move operation completed");
                    break;
                default:
                    System.out.println("Given operation type not available. Run 'help' to get list of operation");
                    System.exit(0);
            }
        }
        catch(Exception ex)
        {
            System.out.println("Error: "+ex.getMessage());
            System.exit(0);
        }
    }

    public static void sendPutRequest(String url,String postFile) throws Exception
    {
	URL obj = new URL(url);
	HttpURLConnection con = (HttpURLConnection) obj.openConnection();

	//add reuqest header
	con.setRequestMethod("PUT");
	con.setUseCaches(false);
	con.setDoOutput(true);	 
	con.setRequestProperty("Connection", "Keep-Alive");
	con.setRequestProperty("Content-Type", "multipart/form-data");
	//String urlParameters = "sn=C02G8416DRJM&cn=&locale=&caller=&num=12345";
        File file = new File(postFile);
	byte[] bFile = new byte[(int) file.length()];
	
	// Send post request
	con.setDoOutput(true);
	DataOutputStream wr = new DataOutputStream(con.getOutputStream());
	wr.write(bFile);
	wr.flush();
	wr.close();

	int responseCode = con.getResponseCode();
	System.out.println("\nSending 'POST' request to URL : " + url);
	//System.out.println("Post parameters : " + urlParameters);
	System.out.println("Response Code : " + responseCode);

	BufferedReader in = new BufferedReader(
	new InputStreamReader(con.getInputStream()));
	String inputLine;
	StringBuffer response = new StringBuffer();

        while ((inputLine = in.readLine()) != null) {
		response.append(inputLine);
        }
	in.close();
	
	//print result
	System.out.println(response.toString());
    }
    
    public static void sendRequest(String url, String requestType) throws Exception
    {
	URL obj = new URL(url);
	HttpURLConnection con = (HttpURLConnection) obj.openConnection();

	//add reuqest header
	con.setRequestMethod(requestType);
	con.setUseCaches(false);
	con.setDoOutput(true);	 
	con.setRequestProperty("Connection", "Keep-Alive");
	con.setRequestProperty("Content-Type", "multipart/form-data");
	//String urlParameters = "sn=C02G8416DRJM&cn=&locale=&caller=&num=12345";
        
	// Send post request
	con.setDoOutput(true);
	
	int responseCode = con.getResponseCode();
	System.out.println("\nSending '"+requestType+"' request to URL : " + url);
	//System.out.println("Post parameters : " + urlParameters);
	System.out.println("Response Code : " + responseCode);

	BufferedReader in = new BufferedReader(
	new InputStreamReader(con.getInputStream()));
	String inputLine;
	StringBuffer response = new StringBuffer();

        while ((inputLine = in.readLine()) != null) {
		response.append(inputLine);
        }
	in.close();
	
	//print result
	System.out.println(response.toString());
    }
}
