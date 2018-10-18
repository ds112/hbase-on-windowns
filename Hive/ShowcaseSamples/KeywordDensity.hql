-- Create table and set the path where the input file is located.

CREATE EXTERNAL TABLE if not exists inputdata1 (keywords STRING)
LOCATION '/Data/ShowcaseSamples/KeywordDensity/Input/Input1';

-- Split the words from the input file using ' '(space).
-- Remove the space before and after of words and convert the text into lower case.
-- Replace the special characters.
-- Extract the English words by removing other language words.
-- Calculate the count.
-- Join the inputdata1 and inputdata2 table by LEFT outer.
-- Remove the NULL values.
-- Filter the words by using word size greater then 2, 'For eg: i,a,is,to these words are removed'.
-- Sort the count of words from larger number.
-- project the needed columns.

--Please use below query to store Result.
--INSERT OVERWRITE DIRECTORY '/ShowcaseSamples/KeywordDensity/HiveOutput' SELECT w1.result, count(w1.result) AS count FROM
--(select regexp_extract(regexp_replace(lower(trim(w.word)),'\\.',''),'(^[A-Za-z]*)',0) as result FROM (SELECT explode(split(keywords, ' ')) AS word FROM inputdata1) w ) w1 where (length(w1.result))>2 == true
--GROUP BY w1.result 
--ORDER BY count DESC;

SELECT w1.result, count(w1.result) AS count FROM
(select regexp_extract(regexp_replace(lower(trim(w.word)),'\\.',''),'(^[A-Za-z]*)',0) as result FROM (SELECT explode(split(keywords, ' ')) AS word FROM inputdata1) w ) w1 where (length(w1.result))>2 == true
GROUP BY w1.result 
ORDER BY count DESC;