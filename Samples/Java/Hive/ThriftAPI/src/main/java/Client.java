import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import javax.security.sasl.SaslException;
import org.apache.hive.service.auth.PlainSaslHelper;
import org.apache.hive.service.cli.thrift.TCLIService;
import org.apache.hive.service.cli.thrift.TColumn;
import org.apache.hive.service.cli.thrift.TExecuteStatementReq;
import org.apache.hive.service.cli.thrift.TExecuteStatementResp;
import org.apache.hive.service.cli.thrift.TFetchOrientation;
import org.apache.hive.service.cli.thrift.TFetchResultsReq;
import org.apache.hive.service.cli.thrift.TFetchResultsResp;
import org.apache.hive.service.cli.thrift.TOpenSessionReq;
import org.apache.hive.service.cli.thrift.TOpenSessionResp;
import org.apache.hive.service.cli.thrift.TOperationHandle;
import org.apache.hive.service.cli.thrift.TRowSet;
import org.apache.hive.service.cli.thrift.TSessionHandle;
import org.apache.thrift.TException;
import org.apache.thrift.protocol.TBinaryProtocol;
import org.apache.thrift.protocol.TProtocol;
import org.apache.thrift.transport.TSocket;
import org.apache.thrift.transport.TTransport;

public class Client {

    public static void main(String[] args) throws TException,SaslException  {
        TTransport socket= new TSocket(args[0], Integer.parseInt(args[1]));
		if(args.length==4)
		{
			socket = PlainSaslHelper.getPlainTransport(args[2], args[3],socket);
		}
        TProtocol protocol = new TBinaryProtocol(socket);					   
        TCLIService.Client client = new TCLIService.Client(protocol);
         socket.open();
        TOpenSessionReq openReq = new TOpenSessionReq();
        TOpenSessionResp openResp = client.OpenSession(openReq);
        TSessionHandle m_sessHandle = openResp.getSessionHandle();


        TExecuteStatementReq execReq = new TExecuteStatementReq();
        execReq.setSessionHandle(m_sessHandle);
        execReq.setStatement("create external table IF NOT EXISTS AdventureWorks_Person_Contact(contactid int,fullname string,age int,emailaddress string,phoneno string,modifieddate string) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/AdventureWorks'");
        TExecuteStatementResp execResp = client.ExecuteStatement(execReq);
        execReq.setStatement("select * from AdventureWorks_Person_Contact");
        execResp = client.ExecuteStatement(execReq);
        TOperationHandle stmtHandle = execResp.getOperationHandle();
        TFetchResultsReq fetchReq = new TFetchResultsReq();
        fetchReq.setOperationHandle(stmtHandle);
        fetchReq.setOrientation(TFetchOrientation.FETCH_NEXT);
        fetchReq.setMaxRows(100);
        TFetchResultsResp resultsResp = client.FetchResults(fetchReq);

        TRowSet resultsSet = resultsResp.getResults();
        List<List> tableResult = new ArrayList<List>();
        if (resultsSet.getColumns() != null && resultsSet.getColumns().size() != 0)
        {
            List<TColumn> resultColumns = resultsSet.getColumns();
            for(int i=0;i<resultColumns.size();i++ )
            {
                TColumn resultRow = resultColumns.get(i);
                List result = new ArrayList();
                if(resultRow.isSetBinaryVal() == true)
                {
                    result = resultRow.getBinaryVal().getValues();
                }
                else if(resultRow.isSetBoolVal() == true)
                {
                    result =resultRow.getBoolVal().getValues();
                }
                else if(resultRow.isSetByteVal() == true)
                {
                    result =resultRow.getByteVal().getValues();
                }
                else if(resultRow.isSetDoubleVal() == true)
                {
                    result=resultRow.getDoubleVal().getValues();
                }
                else if(resultRow.isSetI16Val() == true)
                {
                    result =resultRow.getI16Val().getValues();
                }
                else if(resultRow.isSetI32Val() == true)
                {
                    result =resultRow.getI32Val().getValues();
                }
                else if(resultRow.isSetI64Val() == true)
                {
                    result =resultRow.getI64Val().getValues();
                }
                else if(resultRow.isSetStringVal()==true)
                {
                    result = resultRow.getStringVal().getValues();
                }
                tableResult.add(result);
            }
        }
        for(int i=0;i<tableResult.get(0).size();i++)
        {
            for(int j=0;j<tableResult.size();j++)
            {
                System.out.print(tableResult.get(j).get(i).toString() + "\t");
            }
            System.out.println();
        }
    }
}