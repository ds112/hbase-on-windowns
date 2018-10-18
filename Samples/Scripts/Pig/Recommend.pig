-- Load the data from HDFS
-- This code assumes that you have uploaded Ratings.csv to a folder named recommend on HDFS
-- We have to load the same data twice in order to do a self join. 
Ratings1 = load '/Data/Ratings/Ratings.csv' using PigStorage(',') as (critic:chararray, movie:chararray, rating:double);
Ratings2 = load '/Data/Ratings/Ratings.csv' using PigStorage(',') as (critic:chararray, movie:chararray, rating:double);

-- We do a self join by the name of the critic
combined = JOIN Ratings1 BY critic, Ratings2 BY critic;

-- This removes combinations of movies that are identical and provides all unique pairs
filtered = FILTER combined BY Ratings1::movie < Ratings2::movie;

-- We project the results of the join, properly naming each field
movie_pairs = FOREACH filtered GENERATE 		Ratings1::critic AS critic1,
						Ratings1::movie AS movie1,
						Ratings1::rating AS rating1,
						Ratings2::critic AS critic2, 
						Ratings2::movie AS movie2,
						Ratings2::rating AS rating2;

-- DUMP to verify results
dump movie_pairs;

-- We then group by the pair of movie names
grouped_Ratings = group movie_pairs by (movie1, movie2);

-- Pig offers built-in support for calculating correlations. We make use of the pairs of Ratings that have been gathered during the grouping
correlations = foreach grouped_Ratings generate group.movie1 as movie1,group.movie2 as movie2, 
	FLATTEN(COR(movie_pairs.rating1, movie_pairs.rating2)) as (var1, var2, correlation);

results = foreach correlations generate movie1, movie2, correlation;

-- dump final results for review
dump results;