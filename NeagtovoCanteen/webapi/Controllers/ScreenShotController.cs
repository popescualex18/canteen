using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using PuppeteerSharp.Input;
using PuppeteerSharp.Media;

namespace SCNeagtovo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenshotController : ControllerBase
    {
        private readonly string _chromeExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";

        [HttpGet]
        public async Task<IActionResult> GetScreenshot()
        {
            var url = @"http://86.123.235.136:81/daily-menu-overview?selectedDate=01%2F03%2F2024";
        
            var launchOptions = new LaunchOptions
            {
                ExecutablePath = _chromeExecutablePath,
                Headless = true,
                DefaultViewport = new ViewPortOptions
                {
                    Width= 1510,
                    Height=980,
                }
                
            };

            using var browser = await Puppeteer.LaunchAsync(launchOptions);
            using var page = await browser.NewPageAsync();
            await page.GoToAsync(url);
            var selector = ".content-container.mb-3"; // Hardcoded selector
            await page.WaitForSelectorAsync(selector);
            var element = await page.QuerySelectorAsync(selector);
            if (element == null)
            {
                return NotFound("Element not found.");
            }

            var boundingBox = await element.BoundingBoxAsync();
            if (boundingBox == null)
            {
                return NotFound("Element bounding box not found.");
            }

            var screenshotOptions = new ScreenshotOptions
            {
                Type = ScreenshotType.Jpeg,
                Quality = 80,
                Clip = new Clip
                {
                    X = boundingBox.X,
                    Y = boundingBox.Y,
                    Width = boundingBox.Width,
                    Height = boundingBox.Height
                }
            };

            var screenshotBytes = await page.ScreenshotDataAsync(screenshotOptions);

            return File(screenshotBytes, "image/jpeg", "screenshot.jpg");
        }

        [HttpGet("print-order/{id}")]
        public async Task<IActionResult> PrintOrder(string id)
        {
            var url = @$"http://86.123.235.136:81/invoice/{id}";

            var launchOptions = new LaunchOptions
            {
                ExecutablePath = _chromeExecutablePath,
                Headless = false,
                DefaultViewport = new ViewPortOptions
                {
                    Width = 406,
                    Height = 980,
                }

            };

            using var browser = await Puppeteer.LaunchAsync(launchOptions);
            using var page = await browser.NewPageAsync();
            await page.GoToAsync(url);
            var selector = ".invoice"; // Hardcoded selector
            await page.WaitForSelectorAsync(selector);
            var element = await page.QuerySelectorAsync(selector);
            if (element == null)
            {
                return NotFound("Element not found.");
            }

            var boundingBox = await element.BoundingBoxAsync();
            if (boundingBox == null)
            {
                return NotFound("Element bounding box not found.");
            }

            var screenshotOptions = new ScreenshotOptions
            {
                Type = ScreenshotType.Jpeg,
                Quality = 80,
                Clip = new Clip
                {
                    X = boundingBox.X,
                    Y = boundingBox.Y,
                    Width = 410,
                    Height = boundingBox.Height
                }
            };

            var screenshotBytes = await page.ScreenshotDataAsync(screenshotOptions);

            return File(screenshotBytes, "image/jpeg", "screenshot.jpg");
        }

    }
}
