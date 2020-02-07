# DeptAssign

## Development

I used the Secret Manager tool to store sensitive data during the development. To run the application successfully you should use user-secrets as showed below:

`dotnet user-secrets init` 

A secrets.json file will be created outside the working tree. Make sure you set the same keys name (although you can use your API keys as a value)

`dotnet user-secrets set "Movies:TmdbApiKey" "24ef18a9a2a6be1292a50289a8b49004"`
`dotnet user-secrets set "Movies:YtApiKey" "AIzaSyCi3fLy92sRIlSNoh3lteI0IALE0dqOG2k"`

Run the server to expose the API on https://localhost:5001:

```
cd DepTrailersApp/ 
donet clean && dotnetbuild && dotnet run
```

Run the client server on http://localhost:4200:

```
cd ClientApp/
npm install
ng serve
```

## Documentation and Decisions

### Decoupled backend and frontend

### Use of Angular 8 (version 8.3.24)

### Use of Youtube API
Fazer o pedido apenas quando queremos um filme em especifico
### Use of TmDB API

### Cors Policy
Alternatevely, and probably a bit more safe solution I could explicitly specify origin url 
```
 services.AddCors(options =>
    {
        options.AddPolicy(MyAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("http://example.com",
                                "http://www.contoso.com")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
    });

```
### Smart Search (Autocomplete)

### Bonus Task (Contact Form)

### Extra features (Sorting results)
 TODO
 
### Versioning API

