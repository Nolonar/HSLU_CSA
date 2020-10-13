if [ $# -eq 0 ]
then
    echo "Please specify the project to run"
    exit 1
fi

echo "Fetching changes"
git pull
echo "Building & running"
dotnet run -p $1/$1.csproj --debug