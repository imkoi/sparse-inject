namespace CleanResolver.Tests.Scopes
{
    public class LevelScope : Scope
    {
        private readonly LevelEndPopup _levelEndPopup;

        public LevelScope(LevelEndPopup levelEndPopup)
        {
            _levelEndPopup = levelEndPopup;
        }
        
        public void Execute()
        {
            _levelEndPopup.Show();
            _levelEndPopup.Hide();
        }
    }
}