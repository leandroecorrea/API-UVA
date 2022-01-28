using UVAGraphs.Api.Model;
using UVAGraphs.Api.Repositories;
using UVAGraphs.Api.Utils;
using UVAGraphs.Api.Enums;

namespace UVAGraphs.Api.Services;
public class UpdateServices : IHostedService, IDisposable
{    
    private int executionCount = 0;
    private readonly ILogger<UpdateServices> _logger;
    private Timer _timer;
    private readonly IUpdatable _updatable;
    private IUpdateStrategy _updateStrategy;

    public UpdateServices(ILogger<UpdateServices> logger, IUpdatable updatable)
    {
        _logger = logger;
        _updatable = updatable;
        _updateStrategy = new SimpleUpdateStrategy();
    }    

    public void Update(object state)
    {        
        var count = Interlocked.Increment(ref executionCount);
        _logger.LogInformation(
            "Timed Hosted Service is working. Count: {Count}", count);
            
        
        //UpdateStrategyLogic
        var list = new List<UVA>();
        var latestRecord = _updatable.LatestRecord();
        if(_updateStrategy.Execute(latestRecord, ref list))
        {      
            // Lo dejo comentado porque todavía no quiero llenar la base de datos
            // list.ForEach(u => _updatable.InsertUVA(u));
            // _updatable.Save();
            _logger.LogInformation("Está funcionando la lógica del update");
            _logger.LogWarning($"{list.Count}");                   
        }
    }    

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Servicios de actualización ha iniciado");
        _timer = new Timer(Update, null, TimeSpan.Zero, TimeSpan.FromDays(_updateStrategy.DaysForUpdate));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}