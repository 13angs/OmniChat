using OmniChat.Handlers;
using OmniChat.Models;
using OmniChat.Repositories;

namespace OmniChat.Services
{
    public class ChannelService
    {
        private readonly ChannelRepository _channelRepo;
        public ChannelService(ChannelRepository channelRepo)
        {
            _channelRepo = channelRepo;
        }
        public async Task<OkResponse<string>> CreateChannelAsync(ChannelRequest request, string baseUrl)
        {
            ChannelHandler.HandleCreateChannel(request);

            Channel channel = await _channelRepo
                .FindByClientIdAsync(request);

            if (channel != null)
            {
                throw new BadHttpRequestException($"Channel with client_id={request.ChannelInfo.ClientId} exists");
            }

            LineBotInfo lineChannelInfo = await LineApiRefService.GetLineBotInfo(request.ChannelInfo.AccessToken);

            channel = new Channel()
            {
                ProviderId = request.ProviderId,
                Platform = request.Platform,
                ChannelType = request.ChannelType,
                Description = request.Description,
                CreatedBy = request.CreatedBy,
                ChannelInfo = request.ChannelInfo
            };

            channel.ChannelInfo.Line = new LineChannelInfo
            {
                UserId = lineChannelInfo.UserId!,
                BasicId = lineChannelInfo.BasicId!,
                DisplayName = lineChannelInfo.DisplayName!,
                PictureUrl = lineChannelInfo.PictureUrl!,
                ChatMode = lineChannelInfo.ChatMode!,
                MarkAsReadMode = lineChannelInfo.MarkAsReadMode!,
            };

            await _channelRepo.InsertOneAsync(channel);

            // Get the base URL without the path and query string
            var fullUrl = $"{baseUrl}//message/provider/{channel.ProviderId}/channel/{channel.Id}/platform/line";

            return new OkResponse<string>
            {
                Message = fullUrl
            };
        }

        public OkResponse<ChannelResponse> GetChannels(ChannelRequest request)
        {
            ChannelHandler.HandleGetChannelByLine(request);

            IEnumerable<Channel> channels = _channelRepo.FindChannels(request);

            return new OkResponse<ChannelResponse>
            {
                Data = new ChannelResponse
                {
                    Channels = channels.ToList()
                }
            };
        }
    }
}