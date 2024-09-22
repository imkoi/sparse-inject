namespace CleanResolver.Tests.TestSources;

public class PlayerController : ITickable
{
    private readonly IPlayerControllerProcessor[] _processors;
    private readonly IPlayerModel _playerModel;
    private readonly IPlayerView _playerView;

    public PlayerController(IPlayerControllerProcessor[] processors, IPlayerModel playerModel, IPlayerView playerView)
    {
        _processors = processors;
        _playerModel = playerModel;
        _playerView = playerView;
    }
    
    public void Tick(float deltaTime)
    {
        foreach (var processor in _processors)
        {
            processor.Process(deltaTime);
        }
    }

    public override string ToString()
    {
        return $"{_playerModel}, {_playerView}";
    }
}