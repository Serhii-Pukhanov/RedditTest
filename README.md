1. Run API and get the base API Url.
2. Fill appconfig for the AuthTokenRetriever app and run it to get token.
3. Fill appconfig for the RedditTestConsumerWorkerService app and run it to start updates monitoting. You can use a few token for different users.
4. Use swagger in api to get statistics:
GET /api/Subreddits/GetStats
GET /api/Subreddits/{subredditName}/GetMostVoted
