### Versioning
Using [semantic versioning](https://semver.org/).

### Folder structure  
* src - source code  
    * code - production code only
    * quality - everything related to quality (tests, benchmarks, testable types, ..)
* doc - documentation
* asm - is generated, contains assemblies
    * app - published application

### Naming
**project folders**
* folder starting with a big letter is namespace maker
* folder starting with a small letter is not namespace maker and its purpose is only to keep related files together

**projects**  
* Projects containing production code uses clasical dot notation `[Product].[Project]`.
* Projects containing non-production code uses notation `[Product].[Project]__[Test]`
