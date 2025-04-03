using Ixnas.AltchaNet;
using Microsoft.AspNetCore.Mvc;

namespace AltchaTest.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AltchaController(AltchaService service) : ControllerBase
    {
        private readonly AltchaService _service = service;

        public class AltchaValidationRequest
        {
            public required string Key { get; set; }
        }

        [HttpGet("/challengeSelfHosted")]
        public AltchaChallenge ChallengeSelfHosted()
        {
            return _service.Generate();
        }

        [HttpPost("/verifySelfHosted")]
        public async Task<AltchaValidationResult> VerifySelfHosted([FromBody] AltchaValidationRequest altcha, CancellationToken cancellationToken)
        {
            return await _service.Validate(altcha.Key, cancellationToken);
        }
    }
}
