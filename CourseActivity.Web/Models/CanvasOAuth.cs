namespace CourseActivity.Web.Models
{
    public class CanvasOAuth
    {
        public string BaseUrl { get; set; }
        public string AuthorizationEndpoint { get; set; }
        public string TokenEndpoint { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}