using System.Data;

internal class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnection _dbConnection;
    private IDbTransaction _dbTransaction;
    private ISampleRepository _sampleRepository;

    public UnitOfWork(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
        _dbConnection.Open();
        _dbTransaction = _dbConnection.BeginTransaction();
    }

    public ISampleRepository SampleRepository
    {
        get
        {
            if (_sampleRepository == null)
            {
                _sampleRepository = new SampleRepository(_dbConnection, _dbTransaction);
            }
            return _sampleRepository;
        }
    }

    public async Task CommitAsync()
    {
        try
        {
            _dbTransaction.Commit();
        }
        catch
        {
            _dbTransaction.Rollback();
            throw;
        }
        finally
        {
            _dbTransaction.Dispose();
            _dbConnection.Close();
        }
    }

    public void Dispose()
    {
        _dbTransaction.Dispose();
        _dbConnection.Dispose();
    }
}