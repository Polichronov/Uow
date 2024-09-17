public interface ISampleRepository
{
    Task AddAsync(SampleEntity entity);
    Task<List<SampleEntity>> GetAllAsync();
}