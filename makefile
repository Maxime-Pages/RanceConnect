SERVER_IMAGE = "ranceserver"


all :: server client

server ::
	docker build ./ -t $(SERVER_IMAGE) --no-cache
	docker run -d -p 11000:11000 -v .\db\:\db\ $(SERVER_IMAGE)

client ::
	dotnet restore "./app/Rance Connect/"
	dotnet run --project "./app/Rance Connect/Rance App.csproj"
