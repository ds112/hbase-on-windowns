package com.syncfusion;


import java.io.IOException;
import java.util.ArrayList;
import java.util.StringTokenizer;
import java.util.Map.Entry;

import org.apache.hadoop.conf.Configuration;
import org.apache.hadoop.conf.Configured;
import org.apache.hadoop.io.Text;
import org.apache.hadoop.mapreduce.Job;
import org.apache.hadoop.mapreduce.lib.aggregate.ValueAggregatorBaseDescriptor;
import org.apache.hadoop.mapreduce.lib.aggregate.ValueAggregatorJob;
import org.apache.hadoop.mapreduce.lib.input.FileInputFormat;
import org.apache.hadoop.util.Tool;
import org.apache.hadoop.util.ToolRunner;

/**
 * This is an example Aggregated Hadoop Map/Reduce application. It reads the
 * text input files, breaks each line into words and counts them. The output is
 * a locally sorted list of words and the count of how often they occurred.
 * 
 * To run: bin/hadoop jar hadoop-*-examples.jar aggregatewordcount 
 * <i>in-dir</i> <i>out-dir</i> <i>numOfReducers</i> textinputformat
 * 
 */
public class AggregateWordCount extends Configured implements Tool {

  public class WordCountPlugInClass extends
      ValueAggregatorBaseDescriptor {
    @Override
    public ArrayList<Entry<Text, Text>> generateKeyValPairs(Object key,
                                                            Object val) {
      String countType = LONG_VALUE_SUM;
      ArrayList<Entry<Text, Text>> retv = new ArrayList<Entry<Text, Text>>();
      String line = val.toString();
      StringTokenizer itr = new StringTokenizer(line);
      while (itr.hasMoreTokens()) {
        Entry<Text, Text> e = generateEntry(countType, itr.nextToken(), ONE);
        if (e != null) {
          retv.add(e);
        }
      }
      return retv;
    }

  }

  /**
   * The main driver for word count map/reduce program. Invoke this method to
   * submit the map/reduce job.
 * @throws Exception 
   */
  @SuppressWarnings("unchecked")
  public static void main(String[] args) 
    throws Exception  {	
      
	       String[] arguments=new String[4];
	        arguments[0] = "hdfs://"+args[0]+":9000/Data/WarPeace.txt";
	        arguments[1] = "hdfs://"+args[0]+":9000/AWC";
	        arguments[2] = "2";
	        arguments[3] = "textinputformat";
	        int res = ToolRunner.run(new Configuration(), new AggregateWordCount(), arguments);
		    System.exit(res);
   
  }

@Override
public int run(String[] arg0) throws Exception {
	// TODO Auto-generated method stub
	Configuration conf=getConf();
	Job aggregateJob=ValueAggregatorJob.createValueAggregatorJob(arg0
	        , new Class[] {WordCountPlugInClass.class});
	aggregateJob.setJobName("AggregateWordCount");
	FileInputFormat.setInputPaths(aggregateJob,arg0[0]);
	aggregateJob.setJarByClass(AggregateWordCount.class);
	 int ret = aggregateJob.waitForCompletion(true) ? 0 : 1;
	   
	return ret;
}



}
