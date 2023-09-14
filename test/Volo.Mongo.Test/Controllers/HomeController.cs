using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Volo.Abp.Domain.Repositories;

namespace Volo.Mongo.Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IRepository<Book, Guid> _bookRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, MyDbContext dbContext, IRepository<Book, Guid> repository)
        {
            _logger = logger;
            _bookRepository = repository;
        }

        [HttpGet(Name = "Get")]
        public async Task<IActionResult> Get()
        {
            IMongoDatabase database = await _bookRepository.GetDatabaseAsync();
            IMongoCollection<Book> books = await _bookRepository.GetCollectionAsync();
            IAggregateFluent<Book> bookAggregate = await _bookRepository.GetAggregateAsync();
            var result = await _bookRepository.GetListAsync();
            return Ok(result);
        }
    }
}