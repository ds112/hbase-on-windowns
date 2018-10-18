-- Create table and Load file Ratings.csv twice since we need to do a self-join to obtain unique pairs of movies.
-- This script assumes you have uploaded the Ratings.csv file into a folder named movielens on HDFS.
create External table if not exists recommend_Ratings1(criticid string,movieid string,rating double)
ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' 
LOCATION '/Data/Ratings/';

create External table if not exists recommend_Ratings2(criticid string,movieid string,rating double)
ROW FORMAT DELIMITED FIELDS TERMINATED BY ',' 
LOCATION '/Data/Ratings/';

-- Join by critic name.
-- Since movies are identified by name of type string, we filter and remove cases where both movies are identical.
-- Group by movie named.
-- Calculate correlation in rating values.
select recommend_Ratings1.movieid as movieid1,recommend_Ratings2.movieid as movieid2,corr(recommend_Ratings1.rating,recommend_Ratings2.rating) as correlation from recommend_Ratings1 join recommend_Ratings2 ON (recommend_Ratings1.criticid = recommend_Ratings2.criticid) where recommend_Ratings1.movieid < recommend_Ratings2.movieid GROUP by recommend_Ratings1.movieid,recommend_Ratings2.movieid;