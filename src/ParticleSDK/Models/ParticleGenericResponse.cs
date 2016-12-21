using Newtonsoft.Json;

namespace ParticleSDK.Models
{
    /// <summary>
    /// Helper class from general Particle Cloud request
    /// </summary>
    internal class ParticleGenericResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("ok")]
        public bool Ok { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
