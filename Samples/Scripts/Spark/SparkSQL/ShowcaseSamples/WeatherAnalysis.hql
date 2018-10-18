-- Create table and set the path where the input file is located.
-- Fields are splited by tab separator.

Create External table if not exists weatherinput(STN string,WBAN string,YEARMODA string,TEMPERATURE string,temperature_count string,DEWP string,dewp_count string,SLP string,slp_count string,STP string,stp_count string,
VISIB string,visib_count string,WDSP string,wdsp_count string,MXSPD string,GUST string,MAX string,MIN string,PRCP string,SNDP string,FRSHTT string)
ROW FORMAT DELIMITED FIELDS TERMINATED BY '\t'
LOCATION '/Data/ShowcaseSamples/WeatherAnalysis/Input/Input1';

-- Create table and set the path where the input file is located.
-- Fields are splited by tab separator.

Create External table if not exists stateinput(stns string,wbans string,st string,ctry string)
ROW FORMAT DELIMITED FIELDS TERMINATED BY '\t'
LOCATION '/Data/ShowcaseSamples/WeatherAnalysis/Input/Input2';

-- Get the rain and snow values in FRSHTT column by using substring.
-- Calculate the avergae of temperature,dew,rain and snow.
-- Project the needed columns and group the columns using STN,WBAN,YEARMODA,FRSHTT.
-- Join the weatherinput table with stateinput table using STN and WBAN
-- Filter the result by removing the null values and checking st column contains UNITED STATES.
-- Group the table weatherreport by st,ctry,YEARMODA;
-- Round the average values of temperature,dew,snow and rain.
-- Project the needed columns and create the weatherreport table.

--Please use below query to store Results.
--INSERT OVERWRITE DIRECTORY '/ShowcaseSamples/WeatherAnalysis/HiveOutput' select stateinput.st as state,stateinput.ctry as country,w.YEARMODA as year,round(avg(w.temperature),2),round(avg(w.dewp),2),round((avg(w.rain)*100),2),round((avg(w.snow)*100),2) from (select STN,WBAN,YEARMODA,avg(TEMPERATURE) as temperature,avg(DEWP) as dewp,substr(FRSHTT,3,1) AS snow,substr(FRSHTT,2,1) AS rain from weatherinput GROUP by STN,WBAN,YEARMODA,FRSHTT) w join stateinput ON (w.STN=stateinput.stns AND w.WBAN=stateinput.wbans) where stateinput.ctry =='UNITED STATES' and stateinput.st IS NOT NULL GROUP BY stateinput.st,stateinput.ctry,w.YEARMODA;

select stateinput.st as state,stateinput.ctry as country,w.YEARMODA as year,round(avg(w.temperature),2),round(avg(w.dewp),2),round((avg(w.rain)*100),2),round((avg(w.snow)*100),2) from (select STN,WBAN,YEARMODA,avg(TEMPERATURE) as temperature,avg(DEWP) as dewp,substr(FRSHTT,3,1) AS snow,substr(FRSHTT,2,1) AS rain from weatherinput GROUP by STN,WBAN,YEARMODA,FRSHTT) w join stateinput ON (w.STN=stateinput.stns AND w.WBAN=stateinput.wbans) where stateinput.ctry =='UNITED STATES' and stateinput.st IS NOT NULL GROUP BY stateinput.st,stateinput.ctry,w.YEARMODA;