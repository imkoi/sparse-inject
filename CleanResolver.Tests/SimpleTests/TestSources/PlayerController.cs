namespace CleanResolver.Tests.Simple
{
    public class PlayerController
    {
        private readonly IPlayerControllerProcessor[] _processors;

        public PlayerController(IPlayerControllerProcessor[] processors)
        {
            _processors = processors;
        }
    
        public void Tick(float deltaTime)
        {
            foreach (var processor in _processors)
            {
                processor.Process(deltaTime);
            }
        }
    }
}