## Basic Api Sample

In ConfigureServices:
```c#
// Create mongoDb context and generate Item's repository.

services.AddMongoDataContext<MongoDataContext>(o => { o.ConnectionString = "mongodb://raa:raa@ds133044.mlab.com:33044/asptest"; o.DatabaseName = "asptest"; })
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