namespace Gestion.Common.Domain.Auth
{
    public class Audience
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Base64Secret { get; set; }
        public bool Active { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int RefreshTokenLifeTime { get; set; }
        public string AllowedOrigin { get; set; }
    }
}
