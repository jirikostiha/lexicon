dotnet publish .\src\lexicon.sln --output .\asm\app

echo "deploying db(s).."
try {
  push-location .\asm\app\
  
  .\Lexicon.Cli.exe createDb --sectionName "SQLite-cz"
  .\Lexicon.Cli.exe import --sectionName "SQLite-cz" --dataFile ".\data-slav_gods.csv"
  .\Lexicon.Cli.exe import --sectionName "SQLite-cz" --dataFile ".\data-czech_verbs.csv"

  .\Lexicon.Cli.exe createDb --sectionName "SQLite-en"
  .\Lexicon.Cli.exe import --sectionName "SQLite-en" --dataFile ".\data-english_names.csv"

  .\Lexicon.Cli.exe createDb --sectionName "SQLite-gr"
  .\Lexicon.Cli.exe import --sectionName "SQLite-gr" --dataFile ".\data-greek_gods.csv"
}
finally {
  pop-location
}