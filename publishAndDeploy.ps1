dotnet publish .\src\lexicon.sln --output .\asm\app
echo "deploying db.."
.\asm\app\Lexicon.Cli.exe deploy -df ".\asm\app\data.csv"
