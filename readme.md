<div align="center" style="color:magenta">
  <font size=4> Demo Project </font>
</div>  

# Lexicon
![Build workflow](https://github.com/jirikostiha/lexicon/actions/workflows/build.yml/badge.svg)
![GitHub repo size](https://img.shields.io/github/repo-size/jirikostiha/lexicon)  

It's a demonstration project. It shows the possibility of implementing the service with a database.  
It provides words for other services, for example Markov word generator.  

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


## [Conventions](./doc/conventions.md)