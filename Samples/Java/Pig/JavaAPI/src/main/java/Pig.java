import java.io.IOException;
import org.apache.pig.PigServer;

public class Pig {
    public static void main(String[] args) {
        try {
            PigServer pigServer = new PigServer("mapreduce");
            pigServer.registerScript(args[0]);
        } catch (Exception e) {
            System.out.println(e);
        }
    }
}