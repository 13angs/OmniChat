using OmniChat.Models;

namespace OmniChat.Handlers
{
    public static class UserHandler
    {
        public static bool HandleGetMyProfile(UserRequest request)
        {
            if (!string.IsNullOrEmpty(request.Token))
            {
                return true;
            }

            throw new BadHttpRequestException("token can not be null");
        }
        public static bool HandleGetUserProfile(UserRequest request)
        {
            if (!string.IsNullOrEmpty(request.UserId))
            {
                return true;
            }

            throw new BadHttpRequestException("user_id can not be null");
        }
        public static bool HandleGetUnfollowedUsersInProvider(UserRequest request) => (
                    request.By == RequestParam.friend &&
                    RequiredPropsHandler.HandleRequiredProps("provider_id", request.ProviderId) &&
                    RequiredPropsHandler.HandleRequiredProps("user_id", request.UserId) &&
                    request.CurrentStatus == RelationshipStatus.unfollow);
    }
}