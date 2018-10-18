-- Load  raw_data input file.

Raw_data1 = load '/Data/ShowcaseSamples/WeatherAnalysis/Input/Input1/RawData' as (STN:chararray,WBAN:chararray,YEARMODA:chararray,TEMPERATURE:chararray,temperature_count:chararray,DEWP:chararray,dewp_count:chararray,SLP:chararray,slp_count:chararray,STP:chararray,stp_count:chararray,
VISIB:chararray,visib_count:chararray,WDSP:chararray,wdsp_count:chararray,MXSPD:chararray,GUST:chararray,MAX:chararray,MIN:chararray,PRCP:chararray,SNDP:chararray,FRSHTT:chararray);

Raw_data2 = filter Raw_data1 by NOT (temperature_count matches '(.*)COUNT(.*)');

-- Get the rain and snow values in FRSHTT column by using substring.

Raw_data3 = foreach Raw_data2 generate STN,WBAN,YEARMODA,
(TEMPERATURE matches '(.*)9999.9(.*)'?TRIM(REPLACE(TEMPERATURE,'9999.9','0.00')):TEMPERATURE) as TEMPERATURE,
(DEWP matches '(.*)9999.9(.*)'?TRIM(REPLACE(DEWP,'9999.9','0.00')):DEWP) as DEWP,
SUBSTRING(FRSHTT,1,2) as RAIN,
SUBSTRING(FRSHTT,2,3) as SNOW;

-- Project the needed columns 

Raw_data4 = foreach Raw_data3 generate (long)STN,(long)WBAN,(long)YEARMODA,(double)TEMPERATURE,(double)DEWP,(double)RAIN,(double)SNOW;

-- Group the columns using STN,WBAN,YEARMODA.

grp1 = group Raw_data4 by (STN,WBAN,YEARMODA);

-- Calculate the avergae of temperature,dew,rain and snow.

grp2 = foreach grp1
{
generate FLATTEN(group),AVG(Raw_data4.TEMPERATURE),AVG(Raw_data4.DEWP),(int)AVG(Raw_data4.RAIN)*100,(int)AVG(Raw_data4.SNOW)*100;
}

-- Load the state input file.

state_info = load '/Data/ShowcaseSamples/WeatherAnalysis/Input/Input2/USStateInfo' as (stn:long,wban:long,st:chararray,ctry);

-- Join grp2 and state_info using STN and WBAN.

result1 = join grp2 by ($0,$1),state_info by ($0,$1);

-- Project the needed columns.

result2 = foreach result1 generate $9,$10,$2,$3,$4,$5,$6;

-- Group result2 by st,ctry,YEARMODA;

result3 = group result2 by ($0,$1,$2);

result4 = foreach result3
{
generate FLATTEN(group),AVG(result2.$3),AVG(result2.$4),AVG(result2.$5),AVG(result2.$6);
}

-- Filter the result by removing the null values and checking the column contains UNITED STATES.

result5 = filter result4 by (chararray)$1=='UNITED STATES' and $0 IS NOT NULL;

-- Round the average values of temperature,dew,snow and rain.

result6 = foreach result5 generate $0,$1,$2,(float)ROUND((float)$3*100f)/100f,(float)ROUND((float)$4*100f)/100f,(float)ROUND((float)$5*100f)/100f,(float)ROUND((float)$6*100f)/100f;

-- Write the result6 to HDFS.
-- Please use below command for storing Result
--store result6 into '/ShowcaseSamples/WeatherAnalysis/PigOutput';

dump result6;