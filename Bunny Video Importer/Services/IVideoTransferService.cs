using Bunny_Video_Importer.Models;

namespace Bunny_Video_Importer.Services
{
    public interface IVideoTransferService
    {
        public Task<TransferVideoResult> TransferVideo(VideoTransfer videoTransferInfo);
    }
}
