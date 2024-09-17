public interface IUnitOfWork
{
    Task CommitAsync();
    ISampleRepository SampleRepository { get; }
}