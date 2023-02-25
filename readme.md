# Lexicon

![Build workflow](https://github.com/jirikostiha/lexicon/actions/workflows/build.yml/badge.svg)
![GitHub repo size](https://img.shields.io/github/repo-size/jirikostiha/lexicon)  

<div align="center" style="color:magenta">
  <font size=4> Demo Project </font>
</div>  

Lexicon is a service that provides words for other services/consumers,
such as a Markov word generator. It demonstrates the possibilities of implementing
the service with a different data sources.  

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

   ```html
   localhost:5000/api/words?page=0&pageSize=100
   ```

## [Conventions](./doc/conventions.md)
