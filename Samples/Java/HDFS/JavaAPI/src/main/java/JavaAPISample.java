
import java.io.*;

import org.apache.hadoop.conf.Configuration;
import org.apache.hadoop.fs.FSDataOutputStream;
import org.apache.hadoop.fs.FileSystem;
import org.apache.hadoop.fs.Path;
import org.apache.hadoop.security.UserGroupInformation;

public class JavaAPISample {

    public static void main(String[] args) {

        String mode="",filepath="";
                
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
        
        Configuration configuration = new Configuration();
        /**
         * If you were using from Command shell in BigData studio please remove the below code 
        if(mode.equalsIgnoreCase("kerberos"))
        {
        configuration.set("hadoop.security.authentication", "Kerberos");
        UserGroupInformation.setConfiguration(configuration);
        }      
         */
        if(mode.equalsIgnoreCase("kerberos"))
        {
        configuration.set("hadoop.security.authentication", "Kerberos");
        UserGroupInformation.setConfiguration(configuration);
        }        
        configuration.set("fs.defaultFS","hdfs://"+args[0]+":9000");
        File path = new File(System.getProperty("user.dir"));
        if(path.toString()!="")
        {
            filepath = path.toString().substring(0, path.toString().lastIndexOf("Samples")+7)+ File.separator + "Samples.xml";
        }
        try{
            switch(args[1]){
                case "upload":
                    JavaAPISample.uploadFile(configuration, filepath, "/Data/Output/Sample.txt");
                    System.out.println("Upload operation completed");
                    break;
               case "delete":
                    JavaAPISample.deleteFile(configuration, "/Data/Output/Sample.txt");
                    System.out.println("Delete operation completed");
                    break;
               case "mkdir":
                    JavaAPISample.mkDir(configuration, "/Data/Output2");
                    System.out.println("Create directory operation completed");
                    break;
                case "move":
                    JavaAPISample.moveFile(configuration, "/Data/Output/Sample.txt", "/Data/Output2/Sample.txt");
                    System.out.println("Move operation completed");
                    break;
                case "setreplication":
                    JavaAPISample.setReplication(configuration, "/Data/Output/Sample.txt", (short)3);
                    System.out.println("Move operation completed");
                    break;
                default:
                    System.out.println("Given operation type not available. Run 'help' to get list of operation"+args[1]);
                    System.exit(0);
            }
        }
        catch(Exception ex){
            System.out.println("ERROR: "+ex.toString());
        }
    }
    private static void uploadFile(Configuration conf, String srcPath, String dstPath) throws IOException
    {
        Path inFile=new Path(srcPath);
        Path outFile=new Path(dstPath);
        InputStream inputStream = new BufferedInputStream(new FileInputStream(srcPath));
        FileSystem hdfs = FileSystem.get(conf);
        if (hdfs.exists(outFile))
            System.out.println("Output already exists");
        FSDataOutputStream out = hdfs.create(outFile);
        int bytesRead;
        while ((bytesRead = inputStream.read()) > 0) {
            out.write(bytesRead);
        }
        inputStream.close();
        out.close();
    }
    
    private static void deleteFile(Configuration conf, String srcPath) throws IOException
    {
        Path inFile=new Path(srcPath);
        FileSystem hdfs = FileSystem.get(conf);
        System.out.println("delete status: "+hdfs.delete(inFile, true));
    }
    
    private static void mkDir(Configuration conf, String srcPath) throws IOException
    {
        Path inFile=new Path(srcPath);
        FileSystem hdfs = FileSystem.get(conf);
        System.out.println("mkdir status: "+hdfs.mkdirs(inFile));
    }
    
    private static void moveFile(Configuration conf, String srcPath, String dstPath) throws IOException
    {
        Path inFile=new Path(srcPath);
        Path outFile=new Path(dstPath);
        FileSystem hdfs = FileSystem.get(conf);
        hdfs.rename(inFile, outFile);
    }
    
    private static void setReplication(Configuration conf, String srcPath, short replication) throws IOException
    {
        Path inFile=new Path(srcPath);
        FileSystem hdfs = FileSystem.get(conf);
        hdfs.setReplication(inFile, replication);
    }
}
