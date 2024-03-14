using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Bunny_Video_Importer.Models
{
    public class VideoTransfer
    {
        [Required]
        public string VideoTitle { get; set; }

        [Required]
        public string VideSourceUrl { get; set; }

        public int? VideThumbnailTimeInSeconds { get; set; }

        public string? DestinationVideoLibraryCollectionId { get; set; }
        
        [Required]
        [BindProperty(SupportsGet = true)]
        public int? DestinationVideoLibraryId { get; set; }

        [Required]
        public string DestinationVideoLibraryAPIKey { get; set; }

    }
}
