import sys

from thrift import Thrift
from thrift.transport import TSocket
from thrift.transport import TTransport
from thrift.protocol import TBinaryProtocol
from hbase import Hbase

try:
    transport = TSocket.TSocket(sys.argv[1], int(sys.argv[2]))
    transport = TTransport.TBufferedTransport(transport)
    protocol = TBinaryProtocol.TBinaryProtocol(transport)    
    client = Hbase.Client(protocol)
    transport.open()    
    
    tableName = 'AdventureWorks_Person_Contact'
    tableExists = False  
    tableNames = client.getTableNames();
    for table in tableNames:
        if table == tableName:
             tableExists = True
    if tableExists != True:
         columnFamilies = []
         columnFamilies.append(Hbase.ColumnDescriptor(name='info'))
         columnFamilies.append(Hbase.ColumnDescriptor(name='contact'))
         columnFamilies.append(Hbase.ColumnDescriptor(name='others'))
         client.createTable(tableName,columnFamilies)     
         mutationsbatch = []
         mutations = [Hbase.Mutation(column='info:FULLNAME', value='Gustavo Achong'),Hbase.Mutation(column='info:AGE', value='38'),Hbase.Mutation(column='contact:EMAILID', value='gustavo0@adventure-works.com'),Hbase.Mutation(column='contact:PHONE', value='398-555-0132'),Hbase.Mutation(column='others:MODIFIEDDATE', value='5/16/2005 4:33:33 PM')] 
         mutationsbatch.append(Hbase.BatchMutation(row='1',mutations=mutations)) 
         mutations = [Hbase.Mutation(column='info:FULLNAME', value='Catherine Abel'),Hbase.Mutation(column='info:AGE', value='36'),Hbase.Mutation(column='contact:EMAILID', value='catherine0@adventure-works.com'),Hbase.Mutation(column='contact:PHONE', value='747-555-0171'),Hbase.Mutation(column='others:MODIFIEDDATE', value='5/16/2005 4:33:33 PM')] 
         mutationsbatch.append(Hbase.BatchMutation(row='2',mutations=mutations))
         mutations = [Hbase.Mutation(column='info:FULLNAME', value='Kim Abercrombie'),Hbase.Mutation(column='info:AGE', value='38'),Hbase.Mutation(column='contact:EMAILID', value='kim2@adventure-works.com'),Hbase.Mutation(column='contact:PHONE', value='334-555-0137'),Hbase.Mutation(column='others:MODIFIEDDATE', value='5/16/2005 4:33:33 PM')] 
         mutationsbatch.append(Hbase.BatchMutation(row='3',mutations=mutations))
         mutations = [Hbase.Mutation(column='info:FULLNAME', value='Humberto Acevedo'),Hbase.Mutation(column='info:AGE', value='31'),Hbase.Mutation(column='contact:EMAILID', value='humberto0@adventure-works.com'),Hbase.Mutation(column='contact:PHONE', value='599-555-0127'),Hbase.Mutation(column='others:MODIFIEDDATE', value='5/16/2005 4:33:33 PM')] 
         mutationsbatch.append(Hbase.BatchMutation(row='4',mutations=mutations))
         mutations = [Hbase.Mutation(column='info:FULLNAME', value='Pilar Ackerman'),Hbase.Mutation(column='info:AGE', value='33'),Hbase.Mutation(column='contact:EMAILID', value='pilar1@adventure-works.com'),Hbase.Mutation(column='contact:PHONE', value='1 (11) 500 555-0132'),Hbase.Mutation(column='others:MODIFIEDDATE', value='5/16/2005 4:33:33 PM')] 
         mutationsbatch.append(Hbase.BatchMutation(row='5',mutations=mutations))
         mutations = [Hbase.Mutation(column='info:FULLNAME', value='Frances Adams'),Hbase.Mutation(column='info:AGE', value='35'),Hbase.Mutation(column='contact:EMAILID', value='frances0@adventure-works.com'),Hbase.Mutation(column='contact:PHONE', value='991-555-0183'),Hbase.Mutation(column='others:MODIFIEDDATE', value='5/16/2005 4:33:33 PM')] 
         mutationsbatch.append(Hbase.BatchMutation(row='6',mutations=mutations))
         mutations = [Hbase.Mutation(column='info:FULLNAME', value='Margaret Smith'),Hbase.Mutation(column='info:AGE', value='23'),Hbase.Mutation(column='contact:EMAILID', value='margaret0@adventure-works.com'),Hbase.Mutation(column='contact:PHONE', value='959-555-0151'),Hbase.Mutation(column='others:MODIFIEDDATE', value='5/16/2005 4:33:33 PM')] 
         mutationsbatch.append(Hbase.BatchMutation(row='7',mutations=mutations))
         mutations = [Hbase.Mutation(column='info:FULLNAME', value='Carla Adams'),Hbase.Mutation(column='info:AGE', value='24'),Hbase.Mutation(column='contact:EMAILID', value='carla0@adventure-works.com'),Hbase.Mutation(column='contact:PHONE', value='107-555-0138'),Hbase.Mutation(column='others:MODIFIEDDATE', value='5/16/2005 4:33:33 PM')] 
         mutationsbatch.append(Hbase.BatchMutation(row='8',mutations=mutations))
         mutations = [Hbase.Mutation(column='info:FULLNAME', value='Jay Adams'),Hbase.Mutation(column='info:AGE', value='40'),Hbase.Mutation(column='contact:EMAILID', value='jay1@adventure-works.com'),Hbase.Mutation(column='contact:PHONE', value='158-555-0142'),Hbase.Mutation(column='others:MODIFIEDDATE', value='5/16/2005 4:33:33 PM')] 
         mutationsbatch.append(Hbase.BatchMutation(row='9',mutations=mutations))
         mutations = [Hbase.Mutation(column='info:FULLNAME', value='Ronald Adina'),Hbase.Mutation(column='info:AGE', value='41'),Hbase.Mutation(column='contact:EMAILID', value='ronald0@adventure-works.com'),Hbase.Mutation(column='contact:PHONE', value='453-555-0165'),Hbase.Mutation(column='others:MODIFIEDDATE', value='5/16/2005 4:33:33 PM')] 
         mutationsbatch.append(Hbase.BatchMutation(row='10',mutations=mutations))    
         client.mutateRows(tableName, mutationsbatch,None)
    
    scan = Hbase.TScan(startRow=None,stopRow=None)       
    scannerId = client.scannerOpenWithScan(tableName, scan,None)
    scanValues = client.scannerGet(scannerId)
    if len(scanValues) == 1:
        while len(scanValues) == 1:
            for row in scanValues:
                print '\n'
                print '%s' % (row.row),
                column = row.columns
                for values in column:
                    print '%s' % (row.columns.get(values).value),
                scanValues = client.scannerGet(scannerId)
    client.scannerClose(scannerId)      
    
    transport.close()
except Thrift.TException, tx:
    print '%s' % (tx.message)