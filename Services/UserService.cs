using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
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