# Conventions

## Versioning

Using [semantic versioning](https://semver.org/).  


## Folder structure

* src - source code  
    * code - production code only
    * quality - everything related to quality (tests, benchmarks, testable types, ..)
* doc - documentation
* asm - is generated, contains assemblies
    * app - published application

## Naming

**project folders**
* a folder starting with a capital letter is a namespace maker  
* a folder starting with a small letter is not a namespace maker and its purpose is only to keep related files together  

**projects**  
* Projects containing production code use the clasical dot notation `[Product].[Project]`.  
* Projects containing non-production code use notation `[Product].[Project]__[Test]`  
