using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SampleController : ControllerBase
{
    private readonly ISampleService _sampleService;

    public SampleController(ISampleService sampleService)
    {
        _sampleService = sampleService;
    }

    [HttpPost]
    [Route("add-entity")]
    public async Task<IActionResult> AddEntity(string name)
    {
        await _sampleService.AddEntityAsync(name);
        return Ok();
    }

    [HttpPost]
    [Route("parallel-test")]
    public async Task<IActionResult> ParallelTest()
    {
        var tasks = new List<Task>();

        for (int i = 0; i < 10; i++)
        {
            var name = $"Entity-{i}";
            tasks.Add(_sampleService.AddEntityAsync(name));
        }

        await Task.WhenAll(tasks);

        return Ok(await _sampleService.GetAllEntitiesAsync());
    }
}