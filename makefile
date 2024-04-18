SERVER_IMAGE = ranceserver


all :: server client

server ::
	docker build ./ -t $(SERVER_IMAGE) --no-cache
	docker run $(SERVER_IMAGE) -d -p 11000:11000 -v .\db\:\db\

client ::
	dotnet restore "./app/Rance Connect/"
	dotnet run --project "./app/Rance Connect/Rance App.csproj"
