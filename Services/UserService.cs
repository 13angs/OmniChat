using OmniChat.DTOs;
using OmniChat.Handlers;
using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IUserFriendRepository _userFriendRepo;
        private readonly IJwtService _jwtService;

        public UserService(IUserRepository userRepo, IJwtService jwtService, IUserFriendRepository userFriendRepo)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
            _userFriendRepo = userFriendRepo;
        }

        public async Task<UserResponse> GetMyProfileAsync(UserRequest request)
        {
            if (UserHandler.HandleGetMyProfile(request))
            {
                JwtPayloadData payloadData = _jwtService.DecodeJwtPayloadData(request.Token!);
                User user = await _userRepo.FindByIdAsync(payloadData.UserId!) ??
                    throw new BadHttpRequestException("User not found");

                return new UserResponse
                {
                    User = user
                };
            }

            throw new NotImplementedException("Action is not implemented");
        }

        public async Task<UserResponse> GetUserProfileAsync(UserRequest request)
        {
            if (UserHandler.HandleGetUserProfile(request))
            {
                User user = await _userRepo.FindByIdAsync(request.UserId!) ??
                    throw new BadHttpRequestException("User not found");

                return new UserResponse
                {
                    User = user
                };
            }

            throw new NotImplementedException("Action is not implemented");
        }

        public async Task<UserResponse> GetUsersAsync(UserRequest request)
        {
            if (!Enum.IsDefined(typeof(RequestParam), request.By))
            {
                throw new ArgumentNullException($"{request.By} not found");
            }

            UserResponse userResponse = new();

            if (request.By == RequestParam.provider && RequiredPropsHandler.HandleRequiredProps("provider_id", request.ProviderId))
            {
                userResponse.Users = await _userRepo.FindUsersByProviderId(request);
                return userResponse;
            }
            // getting unfollowed users in the same provider
            else if (UserHandler.HandleGetUnfollowedUsersInProvider(request))
            {
                IEnumerable<UserFriend> userFriends = _userFriendRepo
                    .FindUserFriendsByFriend(request.ProviderId!, request.UserId!, RelationshipStatus.follow);

                IEnumerable<string> userIds = Array.Empty<string>();

                if (userFriends.Any())
                {
                    userIds = userFriends.Select(x => x.RelatedUser.UserId).ToArray()!;
                }

                var allUserIds = userIds.Append(request.UserId);

                IEnumerable<UserDto> users = _userRepo
                    .FindUsersByFriend(request.ProviderId!, allUserIds!);
                userResponse.Users = users.ToList();
                return userResponse;
            }

            throw new NotImplementedException($"By={request.By} is not implemented");
        }
    }
}