import requests, json, sys

def PythonRestAPI():
    print "\nSend Http POST request"
    host=sys.argv[1]

    url = "http://"+host+":11000//oozie/v1/jobs?offset=1&len=50&timezone=GMT"
    headers = {'content-type': 'application/json'}
    payload = {'user.name': 'SYSTEM', 'oozie.wf.application.path': '${nameNode}/Data/Oozie/Apps/Java-Mainworkflow.xml', 'queueName': 'default', 'nameNode': 'hdfs://localhost:9000', 'jobTracker': 'localhost:8032', 'examplesRoot': 'examples', 'oozie.use.system.libpath': 'true'}
    r = requests.get(url);
    print r
    print r.text
    print "Oozie job jubmitted"

    if r.status_code == 200 or r.status_code == 201:
        print "Success!"
    else:
        print "Operation Failed!"

if "__main__":
    PythonRestAPI()