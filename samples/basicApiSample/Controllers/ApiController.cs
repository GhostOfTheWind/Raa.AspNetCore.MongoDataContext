using basicApiSample.Entities;
using Raa.AspNetCore.MongoDataContext.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace basicApiSample.Controllers
{
    [Route("[controller]")]
    public class ApiController : Controller
    {

        private Repository<Item> _itemsRepo;
        public ApiController(Repository<Item> itemsRepo)
        {
            _itemsRepo = itemsRepo;

        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_itemsRepo.List);
        }

        // GET api/id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var item = await _itemsRepo.FindByIdAsync(id);
            return item != null ? Ok(item) : NotFound() as IActionResult;
        }

        // POST api/
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]string value)
        {
            if (value == null)
            {
                return BadRequest();
            }

            var item = await _itemsRepo.InsertAsync(new Item { Name = value });
            return Ok(item);
        }


        // DELETE api/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var item = await _itemsRepo.FindByIdAsync(id);

            await _itemsRepo.DeleteAsync(item);

            return item != null ? Ok() : NotFound() as IActionResult;
        }
    }
}
