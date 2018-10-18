-- Load input file as chararray.

lines = LOAD '/Data/ShowcaseSamples/KeywordDensity/Input/Input1/KenInTheJungle.txt' AS (line:chararray);

-- Split the words from the input file.

words = FOREACH lines GENERATE FLATTEN(TOKENIZE(line,' ')) as word;

-- Remove the space before and after of words and convert the text into lower case.
-- Replace the special characters.

trim_words = foreach words generate REPLACE(LOWER(TRIM(word)),'\\.',' ') as word;

-- Extract the English words by removing other language words and special characters. 

regex = foreach trim_words generate REGEX_EXTRACT(word,'(^[A-Za-z]*)',0) as word;

-- group by words.

grouped = GROUP regex BY word;

-- Calculate the count.

wordcount = FOREACH grouped GENERATE group as word, COUNT(regex) as count;

--wordcount1 = foreach wordcount generate word.wrd as word,count as count;

-- Project the needed columns.

convert2tuple = FOREACH wordcount GENERATE FLATTEN(TOTUPLE($0,$1));

required_fields = foreach convert2tuple generate $0 as word,$1 as count;

-- Filter the words by using word size greater then 2, 'For eg: i,a,is,to these words are removed'.

filter_word = filter required_fields by (int)SIZE(word)>2;

-- Sort the count of words from larger number.

final_count = order filter_word by count desc;

-- Write the final_count to HDFS.
-- Please use below command for storing Result
--store final_count into '/ShowcaseSamples/KeywordDensity/PigOutput';

dump final_count;