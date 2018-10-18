import requests
import sys
import os

def PythonRestAPI():
    print "\nSend Http POST request"
    if len(sys.argv)==3 and sys.argv[1]=="runPy" and sys.argv[2]=="help":
        print "Syntax: JAR <host> <Operation Type>"
        print "=======\n"
        print "Operation Type"
        print "--------------"
        print "upload"
        print "delete"
        print "mkdir"
        print "move"
        print "setreplication"
        exit(0);
    elif len(sys.argv)!=3:
        print "Syntax incorrect. Run 'runPy help' to get list of operation"
        exit(0);

    host=sys.argv[1]
    if sys.argv[2] == 'upload':
        sourcePath = os.path.abspath(os.path.join(os.getcwd(),'..\\..\\..\\..\\..\\..\\Data/WarPeace.txt'))
        file = open(sourcePath, 'r')
        destinationPath="/python_rest/war.txt"
        print "Uploading file: "+sourcePath
        url = "http://"+host+":50070/webhdfs/v1"+destinationPath+"?op=CREATE&overwrite=true"
        r = requests.put(url, file.read())
        file.close()
        print r
        print "Upload operation completed"
    elif sys.argv[2] == 'delete':
        destinationPath="/python_rest/war.txt"
        url = "http://"+host+":50070/webhdfs/v1"+destinationPath+"?op=DELETE&recursive=true"
        r = requests.delete(url)
        print r
        print "Delete operation completed"
    elif sys.argv[2] == 'mkdir':
        destinationPath="/python_rest2"
        print "Create directory: "+destinationPath
        url = "http://"+host+":50070/webhdfs/v1"+destinationPath+"?op=MKDIRS&permission=777"
        r = requests.put(url,"PUT")
        print r
        print "Create directory operation completed"
    elif sys.argv[2] == 'move':
        sourcePath="/python_rest1"
        destinationPath="/python_rest2"
        print "Move : "+destinationPath
        url = "http://"+host+":50070/webhdfs/v1"+sourcePath+"?op=RENAME&destination="+destinationPath
        r = requests.put(url,"PUT")
        print r
        print "Move operation completed"
    elif sys.argv[2] == 'setreplication':
        sourcePath="/python_rest1/war.txt"
        url = "http://"+host+":50070/webhdfs/v1"+sourcePath+"?op=SETREPLICATION&&replication=3"
        r = requests.put(url,"PUT");
        print r
        print "Replication operation completed"
    else:
        print "Given operation type not available. Run 'runPy help' to get list of operation"

    if r.status_code == 200 or r.status_code == 201:
        print "Success!"
    else:
        print "Operation Failed!"

if "__main__":
    PythonRestAPI()
