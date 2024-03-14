using Bunny_Video_Importer.Models;
using Bunny_Video_Importer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;

namespace Bunny_Video_Importer.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty(SupportsGet = true)]

        public VideoTransfer? VideoTransfer { get; set; }

        private IVideoTransferService _videoTransferService;
        public IndexModel(ILogger<IndexModel> logger, IVideoTransferService videoTransferService)
        {
            _logger = logger;
            _videoTransferService = videoTransferService;
        }

        public void OnGet()
        {
            if (Request.Query["message"] != Microsoft.Extensions.Primitives.StringValues.Empty)
            {
                ViewData["message"] = Request.Query["message"];
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var transferResult = await _videoTransferService.TransferVideo(VideoTransfer);

            ViewData.Add("result", transferResult);

            if (transferResult.StatusCode == 200)
            {
                return RedirectToPage("index", new
                {
                    DestinationVideoLibraryId = VideoTransfer.DestinationVideoLibraryId,
                    DestinationVideoLibraryAPIKey = VideoTransfer.DestinationVideoLibraryAPIKey,
                    message = transferResult.TransferResult
                });
            }
            else
            {
                return Page();
            }
        }
    }
}