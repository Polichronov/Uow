public class SampleService : ISampleService
{
    private readonly IUnitOfWork _unitOfWork;

    public SampleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddEntityAsync(string name)
    {
        var entity = new SampleEntity { Name = name };
        await _unitOfWork.SampleRepository.AddAsync(entity);
        await _unitOfWork.CommitAsync();
    }

    public async Task<List<SampleEntity>> GetAllEntitiesAsync()
    {
        return await _unitOfWork.SampleRepository.GetAllAsync();
    }
}