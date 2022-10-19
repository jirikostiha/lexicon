# Lexicon
![Build workflow](https://github.com/jirikostiha/lexicon/actions/workflows/build.yml/badge.svg)  
Is a demonstration project. It shows the possibility of implementing the service with a database.  
It provides words for other services such as Markov's word generator.  

## Usage
1. To start the server, first deploy the database.  
```powershell
.\publishAndDeploy.ps1 
```

2. Start the server.
```powershell
.\run.ps1  
```

3. Then call the service in the browser to retrieve the data.
```
https://localhost:5001/api/words?page=0&pageSize=100
```


## Conventions
see [conventions](/doc/conventions.md)
  
