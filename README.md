# DeptAssign

## Development

I used the Secret Manager tool to store sensitive data during the development. To run the application successfully you should use user-secrets running the command (in the project directory):

```
cd DepTrailersApp/ 

dotnet user-secrets init 
```
A secrets.json file will be created outside the working tree. Make sure you set the same Key names (although, use your API keys as a value)
```
dotnet user-secrets set "Movies:TmdbApiKey" "{YOUR_API_KEY}"

dotnet user-secrets set "Movies:YtApiKey" "{YOUR_API_KEY}"

dotnet user-secrets set "Movies:TmdbApiUrl" "{YOUR_API_URL}"
```

Run the server to expose the API on https://localhost:5001:

``` 
dotnet clean && dotnet build && dotnet run
```

Run the client server on http://localhost:4200:

```
cd ClientApp/
npm install
ng serve
```

## Documentation and Decisions

### Decoupled backend and frontend
Since it was written in the assignment description that "optionally create a web page" I decided to develop and run separately the frontend and backend. This decision has its advantages and disavantages but what I wanted to focus here it is the decoupling of both sides and the fact that the middleware API can be used by another client.

### Use of Angular 8 (version 8.3.24)
I used this framework to challenge myself a bit during this assignment, I had small experience with Angularv4 (4 years ago) and I have never used it again. 
TypeScript is a language that I also wanted to learn, so why not!

### Use of Youtube API
Youtube API has some limited requests per day, however using Vimeo API is not free.
To reduce the data traffic and consequently website latency I only request Youtube API when the user tries to access a specific movie page.

### Use of TmDB API
I used this API before and I knew I could get from it all the data I needed for the assignment, without any costs or limits associated.
(I could get also the videos associated with each movie from this API, avoiding the use of Youtube API and incresing website speed. However, I used to fulfill one of the assignments requirement).

### Cors Policy
The solution adopted is only valid for development environment
Alternatively, and probably a bit more safe solution would be explicitly specify origin url of the client which performs the requests as follows:

```
 services.AddCors(options =>
    {
        options.AddPolicy(MyAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("http://example.com")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
    });

```
Or even more ideal configuration would be the use of policy same-origin.

### Versioning API
For a matter of scalability and future use the current API endpoints were registered in version 1.0

#### Endpoints
List movies matching with the string send as a parameter in the query
```
GET /api/v1.0/movies/find?q={query} 
````
List most popular movies
```
GET /api/v1.0/movies/popular
```
Get specific movie details by Id
```
GET api/v1.0/movies/{id}
```
