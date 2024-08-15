build:
	dotnet build
start:
	dotnet run --project ./src/GenocsAspire.AppHost
publish:
	dotnet publish --os linux --arch x64 -c Release --self-contained

dcu: # docker-compose up : bare infrastructure
	cd ./containers/docker-compose && docker-compose -f infrastructure-bare.yml --env-file ./.env --project-name genocs up -d
dcd: # docker-compose down : webapi + postgresql
	cd ./containers/docker-compose && docker-compose -f infrastructure-bare.yml down
