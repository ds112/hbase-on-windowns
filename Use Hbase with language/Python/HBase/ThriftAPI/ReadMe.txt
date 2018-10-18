#Steps to run HBase Gradle samples through Python Thrift API

*Gradle in command line
Open BigData command shell, type 
To build : gradle -p %SYNCBDP_HOME%\Samples\Python\HBase\ThriftAPI build
To run : gradle -p %SYNCBDP_HOME%\Samples\Python\HBase\ThriftAPI runPy -Parguments="ipaddress portNo"

* Gradle in Netbeans
Open the gradle project in Netbeans IDE.
To build : Right click on the project and click 'build'.
To run : Right click on the project click Custom Tasks-> Custom Tasks
             Under 'Tasks' type 'runPy'
             Under 'Arguments' type '-Parguments="ipaddress portNo"
             Click Execute to run the sample.
Ex: runPy -Parguments="192.168.10.12 10003"

* Gradle in Eclipse
File-> Import-> Import Gradle project.
Choose the Gradle project and click 'Build Model'. Click Finish.
To build : Right click the project, click run As-> Gradle Build.  
                Under Gradle Tasks type 'build' (or) 'jar', Apply and then click run.
To run :  Right click the project, click run As-> Gradle Build.
              Under Gradle Tasks type 'runPy'
              Under 'Arguments' tab, in Program arguments select 'Use' and type '-Parguments="ipaddress portNo"

* Gradle in IntelliJ
Open the gradle project in IntelliJ IDE.
Click View-> Tool Windows-> Gradle to open gradle projects. 
Click on the icon 'Execute Gradle Task'.
Under 'Gradle project' choose the Gradle project for execution.
Under 'Command line',
        To build sample : build  (or) jar (this generates an executable jar file under build/libs folder)
        To run : runPy -Parguments="ipaddress portNo"
Click OK to run the sample.

(Note: To run the hbase samples locally type runPy -Parguments="localhost 10003")