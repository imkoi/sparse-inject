namespace CleanResolver.Tests.Scopes
{
    public class GameController
    {
        private readonly LevelScope _levelScope;

        public GameController(LevelScope levelScope)
        {
            _levelScope = levelScope;
        }

        public void Execute()
        {
            _levelScope.Execute();
            _levelScope.Dispose();
        }
    }
}
