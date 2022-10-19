dotnet publish .\src\lexicon.sln --output .\asm\app

echo "deploying db(s).."

.\asm\app\Lexicon.Cli.exe createDb --sectionName "SQLite-cz"
.\asm\app\Lexicon.Cli.exe import --sectionName "SQLite-cz" --dataFile ".\asm\app\data-slav_gods.csv"
.\asm\app\Lexicon.Cli.exe import --sectionName "SQLite-cz" --dataFile ".\asm\app\data-czech_verbs.csv"

.\asm\app\Lexicon.Cli.exe createDb --sectionName "SQLite-en"
.\asm\app\Lexicon.Cli.exe import --sectionName "SQLite-en" --dataFile ".\asm\app\data-english_names.csv"

.\asm\app\Lexicon.Cli.exe createDb --sectionName "SQLite-gr"
.\asm\app\Lexicon.Cli.exe import --sectionName "SQLite-gr" --dataFile ".\asm\app\data-greek_gods.csv"