# Basic api with [Orleans](https://dotnet.github.io/orleans/)

Implementation based on [this](https://samueleresca.net/developing-apis-using-actor-model-in-asp-net) tutorial from @samueleresca

## Running with docker-compose

### Building image
From root directory (where solution file -  _CartApi.lsn_ is stored) run _docker-compose_ command:
``` bash
sudo docker-compose up -d
```
### Stop everything
To start container which was stopped, type _Docker start_ command:
``` bash
sudo docker-compose down -v
```
## Building individual project docker images

### Building api image
From root directory (where solution file -  _CartApi.lsn_ is stored) run _docker build_ command:
``` bash
sudo docker build -f src/Cart/Dockerfile -t cartapi .
```

### Building Silo image
From root directory (where solution file -  _CartApi.lsn_ is stored) run _docker build_ command:
``` bash
sudo docker build -f src/Silo/Dockerfile -t cartsilo .
```
