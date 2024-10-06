namespace CleanResolver.Tests.TestSources
{
    public class WeaponController : ITickable
    {
        private readonly IWeaponControllerProcessor[] _processors;

        public WeaponController(IWeaponControllerProcessor[] processors)
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