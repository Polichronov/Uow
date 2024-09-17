public interface ISampleService
{
    Task AddEntityAsync(string name);
    Task<List<SampleEntity>> GetAllEntitiesAsync();
}