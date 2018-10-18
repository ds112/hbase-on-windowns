import sys

from thrift import Thrift
from thrift.transport import TSocket
from thrift.transport import TTransport
from thrift.protocol import TBinaryProtocol
from TCLIService import TCLIService
from TCLIService.ttypes import TOpenSessionReq, TFetchResultsReq,\
  TExecuteStatementReq, TFetchOrientation
 
try:
    transport = TSocket.TSocket( sys.argv[1], int(sys.argv[2]))
    transport = TTransport.TBufferedTransport(transport)
    protocol = TBinaryProtocol.TBinaryProtocol(transport)    
    client = TCLIService.Client(protocol)
    transport.open()
    openReq = TOpenSessionReq()
    openResp = client.OpenSession(openReq)
    m_sessHandle = openResp.sessionHandle
    execReq =  TExecuteStatementReq()    
    execReq.sessionHandle = m_sessHandle
    execReq.statement = "create external table IF NOT EXISTS AdventureWorks_Person_Contact(contactid int,fullname string,age int,emailaddress string,phoneno string,modifieddate string) ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' LOCATION '/Data/AdventureWorks'"
    execResp = client.ExecuteStatement(execReq)     
    execReq.statement = 'select * from AdventureWorks_Person_Contact'
    execResp = client.ExecuteStatement(execReq)     
    stmtHandle = execResp.operationHandle
    fetchReq = TFetchResultsReq()
    fetchReq.operationHandle = stmtHandle
    fetchReq.orientation = TFetchOrientation.FETCH_NEXT
    fetchReq.maxRows = 100
    resultsResp = client.FetchResults(fetchReq)
    resultsSet = resultsResp.results
    tableResult = []
    if resultsSet.columns != None: 
      if len(resultsSet.columns) != 0:
        resultColumns = resultsSet.columns
        for i in range(0,len(resultColumns)):
            resultRow = resultColumns[i]
            if resultRow.binaryVal != None:
                result = resultRow.binaryVal.values 
            elif resultRow.boolVal != None:
                result = resultRow.boolVal.values
            elif resultRow.byteVal != None:
                result = resultRow.byteVal.values
            elif resultRow.doubleVal != None:
                result = resultRow.doubleVal.values
            elif resultRow.i16Val != None:
                result = resultRow.i16Val.values
            elif resultRow.i32Val != None:
                result = resultRow.i32Val.values
            elif resultRow.i64Val != None:
                result = resultRow.i64Val.values
            elif resultRow.stringVal != None:                
                result = resultRow.stringVal.values     
            tableResult.append(result);       
    for i in range (0,len(tableResult[0])):
        for j in range (0,len(tableResult)):          
            print '%s' % (tableResult[j][i]),   
        print '\n'
    transport.close() 
except Thrift.TException, tx:
    print '%s' % (tx.message)