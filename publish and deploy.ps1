dotnet publish .\src\lexicon.sln --output .\asm\app
cd .\asm\app
echo "deploying db.."
.\Lexicon.Cli.exe deploy -df "data.csv"
