namespace Linkslap.WP.Communication.Models
{
    using Newtonsoft.Json;

    public class RegisterModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("confirmPassword")]
        public string ConfirmPassword { get; set; }
    }
}
