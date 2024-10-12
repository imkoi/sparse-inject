namespace CleanResolver.Tests.Scopes
{
    public class FeatureRewardPopup : IFeaturePopup
    {
        private readonly RewardService _rewardService;

        public FeatureRewardPopup(RewardService rewardService)
        {
            _rewardService = rewardService;
        }
        
        public void Show()
        {
            
        }

        public void Hide()
        {
            _rewardService.AddReward();
        }
    }
}