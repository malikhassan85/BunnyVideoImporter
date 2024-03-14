using Bunny_Video_Importer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Security.Cryptography.Xml;

namespace Bunny_Video_Importer.Services
{
    public class BunnyVideoTransferService : IVideoTransferService
    {
        private string bunnyApiBaseUrl = "https://video.bunnycdn.com/";
        public async Task<TransferVideoResult> TransferVideo(VideoTransfer videoTransferInfo)
        {
            var transferResult = new TransferVideoResult();

            try
            {
                var createVideoOptions = new RestClientOptions($"{bunnyApiBaseUrl}library/{videoTransferInfo.DestinationVideoLibraryId}/videos/");
                var createVideoClient = new RestClient(createVideoOptions);
                var createVideorequest = new RestRequest("");
                createVideorequest.AddHeader("AccessKey", videoTransferInfo.DestinationVideoLibraryAPIKey);
                createVideorequest.AddHeader("accept", "application/json");
                createVideorequest.AddHeader("content-type", "application/json");
                createVideorequest.AddBody(new
                {
                    title = videoTransferInfo.VideoTitle,
                    collectionId = videoTransferInfo.DestinationVideoLibraryCollectionId
                });

                var createVideoResponse = await createVideoClient.PostAsync(createVideorequest);


                if (createVideoResponse.IsSuccessStatusCode)
                {
                    var createVideoStringContent = createVideoResponse.Content;

                    if (createVideoStringContent is not null)
                    {
                        dynamic createVideoContent = JObject.Parse(createVideoStringContent);
                        var fetchVideoOptions = new RestClientOptions($"{bunnyApiBaseUrl}library/{videoTransferInfo.DestinationVideoLibraryId}/videos/{createVideoContent.guid}/fetch");
                        var fetchVideoClient = new RestClient(fetchVideoOptions);
                        var fetchVideorequest = new RestRequest("");
                        fetchVideorequest.AddHeader("AccessKey", videoTransferInfo.DestinationVideoLibraryAPIKey);
                        fetchVideorequest.AddHeader("accept", "application/json");
                        fetchVideorequest.AddHeader("content-type", "application/json");
                        fetchVideorequest.AddBody(new
                        {
                            Url = videoTransferInfo.VideSourceUrl
                        });

                        var fetchVideoResponse = await fetchVideoClient.PostAsync(fetchVideorequest);

                        if (fetchVideoResponse.IsSuccessStatusCode)
                        {
                            transferResult.StatusCode = (int)fetchVideoResponse.StatusCode;
                            transferResult.TransferResult = $"Video {createVideoContent.guid} was created successfully, and now the transfer is in progress";
                        }
                        else
                        {
                            transferResult.StatusCode = (int)fetchVideoResponse.StatusCode;
                            transferResult.TransferResult = $"Video {createVideoContent.guid} was created successfully, but the transfer failed";
                        }
                    }

                }
                else
                {
                    transferResult.StatusCode = (int)createVideoResponse.StatusCode;
                    transferResult.TransferResult = createVideoResponse.Content;
                }
            }
            catch(Exception ex)
            {
                transferResult.StatusCode = 500;
                transferResult.TransferResult = ex.Message;
            }

            return transferResult;
        }
    }
}
