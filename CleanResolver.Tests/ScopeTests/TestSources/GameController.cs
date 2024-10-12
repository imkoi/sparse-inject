namespace CleanResolver.Tests.Scopes
{
    public class GameController
    {
        private readonly FeatureScope _featureScope;

        public GameController(FeatureScope featureScope)
        {
            _featureScope = featureScope;
        }

        public void Execute()
        {
            _featureScope.Execute();
            _featureScope.Dispose();
        }
    }
}
