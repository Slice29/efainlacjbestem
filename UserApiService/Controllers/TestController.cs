using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using UserApiService.Attributes;

namespace UserApiService.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/test")]
    [ServiceFilter(typeof(JwtAuthorizeAttribute))]
    [EnableCors]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("validatetoken")]
        public async Task<IActionResult> GetToken([FromQuery] string code)
        {

            return Ok("SE POATE DA!!!");
        }


        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("startvpn")]
        public IActionResult StartVpnConnection()
        {
            try
            {
                string command = @"c:\Program Files\WireGuard\wireguard.exe";
                string arguments = @"/installtunnelservice D:\bestem\wg0.conf";

                var processStartInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processStartInfo))
                {
                    string result = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    return Ok("VPN connection initiated. Result: " + result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error starting VPN connection: " + ex.Message);
            }
        }

        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("stopvpn")]
        public IActionResult StopVpnConnection()
        {
            try
            {
                string command = @"c:\Program Files\WireGuard\wireguard.exe";
                string arguments = @"/uninstalltunnelservice wg0";

                var processStartInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processStartInfo))
                {
                    string result = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    return Ok("VPN connection terminated. Result: " + result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error terminating VPN connection: " + ex.Message);
            }

        }

    }

}