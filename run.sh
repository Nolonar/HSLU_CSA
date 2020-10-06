echo "Fetching changes"
git pull
echo "Building & running"
dotnet run -p $1/$1.csproj