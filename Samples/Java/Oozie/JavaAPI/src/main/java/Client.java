import java.util.Properties;

import org.apache.oozie.client.OozieClient;
import org.apache.oozie.client.WorkflowJob;

public class Client {

	public static void main(String[] args) {
		// TODO Auto-generated method stub
            
            if(args.length==1 && args[0].equals("help")){
            System.out.println("Syntax: runJar <HostName> <Authentication Type");
            System.out.println("-------");
            System.out.println("Authentication Type");
            System.out.println("--------------");
            System.out.println("Kerberos");
            System.out.println("Simple");
            System.exit(0);
            }
           String mode= "";
            if(args.length>1)
            {
             mode=args[1];
            }
            else
            {
                mode ="simple";
            }
            String oozieClient= mode.equalsIgnoreCase("kerberos") ? ("https://"+args[0]+":11443/oozie") : ("http://"+args[0]+":11000/oozie");
		OozieClient wc = new OozieClient(oozieClient);

	    Properties conf = wc.createConfiguration();

	    conf.setProperty(OozieClient.APP_PATH, "hdfs://"+args[0]+":9000/Data/Oozie/Apps/Map-Reduce-Workflow.xml");
	    conf.setProperty("jobTracker", args[0]+":8032");
	    conf.setProperty("nameNode", "hdfs://"+args[0]+":9000");
	    conf.setProperty("queueName", "default");
	    conf.setProperty("examplesRoot", "examples");
	    conf.setProperty(OozieClient.LIBPATH, "true");
	    conf.setProperty("outputDir","OozieOutput");
	    conf.setProperty(OozieClient.USER_NAME, "SYSTEM");
	    try {
	        String jobId = wc.run(conf);
	        System.out.println("Workflow job submitted");

	        while (wc.getJobInfo(jobId).getStatus() == WorkflowJob.Status.RUNNING) {
	            System.out.println("Workflow job running ...");
	            Thread.sleep(10 * 1000);
	        }
	        System.out.println("Workflow job completed ...");
	        System.out.println(wc.getJobInfo(jobId));
	    } catch (Exception r) {
	        System.out.println("Errors");
	    }
	}

}
