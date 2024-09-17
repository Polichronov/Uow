using Dapper;
using System.Data;

public class SampleRepository : ISampleRepository
{
    private readonly IDbConnection _dbConnection;
    private readonly IDbTransaction _dbTransaction;

    public SampleRepository(IDbConnection dbConnection, IDbTransaction dbTransaction)
    {
        _dbConnection = dbConnection;
        _dbTransaction = dbTransaction;
    }

    public async Task AddAsync(SampleEntity entity)
    {
        var sql = "INSERT INTO SampleEntities (Name) VALUES (@Name)";
        var parameters = new { Name = entity.Name };
        await _dbConnection.ExecuteAsync(sql, parameters, _dbTransaction);
    }

    public async Task<List<SampleEntity>> GetAllAsync()
    {
        var sql = "SELECT * FROM SampleEntities";
        var result = await _dbConnection.QueryAsync<SampleEntity>(sql, transaction: _dbTransaction);
        return result.ToList();
    }
}