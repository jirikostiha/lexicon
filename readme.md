# Lexicon

![GitHub repo size](https://img.shields.io/github/repo-size/jirikostiha/lexicon)
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/jirikostiha/lexicon)  
![Build workflow](https://github.com/jirikostiha/lexicon/actions/workflows/build.yml/badge.svg)
![Code Analysis](https://github.com/jirikostiha/lexicon/actions/workflows/analyse-code.yml/badge.svg)
![Code Lint](https://github.com/jirikostiha/lexicon/actions/workflows/lint-code.yml/badge.svg?colorB=orange)
![Documentation Lint](https://github.com/jirikostiha/lexicon/actions/workflows/lint-docs.yml/badge.svg?label=Docs+Lint&colorB=orange)
![Configuration Lint](https://github.com/jirikostiha/lexicon/actions/workflows/lint-config.yml/badge.svg?label=Config+Lint&colorB=orange)

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
