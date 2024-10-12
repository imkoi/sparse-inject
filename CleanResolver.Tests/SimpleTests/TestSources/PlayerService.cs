namespace CleanResolver.Tests.Simple
{
    public class PlayerService
    {
        private readonly PlayerController _playerController;
        private readonly IPlayerControllerProcessor[] _playerProcessors;

        public PlayerService(PlayerController playerController, IPlayerControllerProcessor[] playerProcessors)
        {
            _playerController = playerController;
            _playerProcessors = playerProcessors;
        }
        
        public void SpawnPlayer()
        {
            
        }
    }
}