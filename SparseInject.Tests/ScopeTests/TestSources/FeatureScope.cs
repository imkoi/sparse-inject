namespace SparseInject.Tests.Scopes
{
    public class FeatureScope : Scope
    {
        private readonly IFeaturePopup _featurePopup;

        public FeatureScope(IFeaturePopup featurePopup)
        {
            _featurePopup = featurePopup;
        }
        
        public void Execute()
        {
            _featurePopup.Show();
            _featurePopup.Hide();
        }
    }
}