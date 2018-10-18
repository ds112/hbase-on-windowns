import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;

public class OozieRestGradle {

   
    public static void main(String[] args) throws Exception {
          String properties="<configuration>\n" +
"<property>\n" +
"        <name>user.name</name>\n" +
"        <value>SYSTEM</value>\n" +
"</property>\n" +
"<property>\n" +
"        <name>oozie.wf.application.path</name>\n" +
"        <value>${nameNode}/Data/Oozie/Apps/Java-Mainworkflow.xml</value>\n" +
"</property>\n" +
"<property>\n" +
"        <name>queueName</name>\n" +
"        <value>default</value>\n" +
"</property>\n" +
"<property>\n" +
"        <name>nameNode</name>\n" +
"        <value>hdfs://"+args[0]+":9000</value>\n" +
"</property>   \n" +
"<property>\n" +
"        <name>jobTracker</name>\n" +
"        <value>"+args[0]+":8032</value>\n" +
"</property>   \n" +
"<property>\n" +
"        <name>examplesRoot</name>\n" +
"        <value>examples</value>\n" +
"</property>\n" +
"<property>\n" +
"        <name>oozie.use.system.libpath</name>\n" +
"        <value>true</value>\n" +
"</property>\n" +
"</configuration>";
        String url= "";
        String jobDetail="";
        String mode="";
        System.out.println(args.length+"value:"+args[0]);
        // TODO code application logic here
        if(args.length==1 && args[0].equals("help")){
            System.out.println("Syntax: runJar <HostName> <Authentication Type");
            System.out.println("-------");
            System.out.println("Authentication Type");
            System.out.println("--------------");
            System.out.println("Kerberos");
            System.out.println("Simple");
            System.exit(0);
            }
        
       
            if(args.length>1)
            {
             mode=args[1];
            }
            else
            {
                mode ="simple";
            }
        try{
            url = mode.equalsIgnoreCase("kerberos") ?  ("https://"+args[0]+":11443/oozie/v1/jobs?action=start") : ("http://"+args[0]+":11000/oozie/v1/jobs?action=start");
            System.out.println(url);
            
            String jobid=OozieRestGradle.submitOozieJob(url, properties);
            jobDetail= mode.equalsIgnoreCase("kerberos") ? ("https://"+args[0]+":11443/oozie/v1/job/"+jobid+"?show=info") : ("http://"+args[0]+":11000/oozie/v1/job/"+jobid+"?show=info") ; 
            System.out.println(jobDetail);
        OozieRestGradle.getJobInfo(jobDetail, "GET");
        }
        catch(Exception ex)
        {
            System.err.println("ERROR:"+ex.getMessage());
        }
    }
    public static String submitOozieJob(String url,String properties) throws Exception
    {
	URL obj = new URL(url);
	HttpURLConnection con = (HttpURLConnection) obj.openConnection();

	//add reuqest header
	con.setRequestMethod("POST");
	con.setUseCaches(false);
	con.setDoOutput(true);
	con.setRequestProperty("Content-Type", "application/xml");
        //con.setRequestMethod(properties);
	
        OutputStream os = con.getOutputStream();
        os.write(properties.getBytes());
	os.flush();
        if (con.getResponseCode() != HttpURLConnection.HTTP_CREATED) {
			throw new RuntimeException("Failed : HTTP error code : "
				+ con.getResponseCode());
        }

	BufferedReader br = new BufferedReader(new InputStreamReader(
				(con.getInputStream())));

	String output;
	System.out.println("Output from Server .... \n");
	while ((output = br.readLine()) != null) {
		System.out.println(output);
                con.disconnect();
                return output=output.split(":")[1].substring(1, output.split(":")[1].length()-2);
	}
        return "error";
    }
    
    public static void getJobInfo(String url, String requestType) throws Exception
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
