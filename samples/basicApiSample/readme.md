## Basic Api Sample

In ConfigureServices:
```c#
// Create mongoDb context and generate Item's repository.

services.AddMongoDataContext<MongoDataContext>(o => { o.ConnectionString = "MONGO_CONNECTION_STRING"; o.DatabaseName = "MONGO_DB_NAME"; })
	.CreateRepository<Item>();
```

In ApiController:
```c#
private Repository<Item> _itemsRepo;
public ApiController(Repository<Item> itemsRepo)
{
	_itemsRepo = itemsRepo;
}
```