## Lexicon
Is a demonstration project. It shows the possibility of implementing the service with a database.  
It provides words for other services such as Markov's word generator.  

### Usage
1. To run a server first deploy a database.  
```powershell
.\publishAndDeploy.ps1 
```

2. Run server.
```powershell
.\run.ps1  
```

3. Then in browser call service to obtain a data.
```http
https://localhost:5001/api/words?page=0&pageSize=100
```


### Special conventions
  
