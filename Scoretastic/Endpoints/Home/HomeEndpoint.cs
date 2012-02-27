namespace Scoretastic.Endpoints.Home
{
    public class HomeEndpoint
    {
        public HomeViewModel Get(HomeRequestModel request)
        {
            return new HomeViewModel() {Note = "Hmmmm"};
        }
    }

    public class HomeRequestModel
    {
    }

    public class HomeViewModel
    {
        public string Note { get; set; }
    }
}