using OmniChat.Handlers;
using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtService _jwtService;

        public UserService(IUserRepository userRepo, IJwtService jwtService)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
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

            UserResponse userResponse = new UserResponse();

            if (request.By == RequestParam.provider && !string.IsNullOrEmpty(request.ProviderId))
            {
                userResponse.Users = await _userRepo.FindUsersByProviderId(request);
                return userResponse;
            }

            throw new NotImplementedException($"By={request.By} is not implemented");
        }
    }
}