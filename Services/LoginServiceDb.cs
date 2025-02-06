using Microsoft.Extensions.Logging;
using DbRepos;
using Models.DTO;
using DbContext;

namespace Services;

public class LoginServiceDb : ILoginService
{
    private readonly LoginDbRepos _repo;
    private readonly ILogger<LoginServiceDb> _logger;

    private readonly JWTService _jtwService;

    public LoginServiceDb(ILogger<LoginServiceDb> logger, LoginDbRepos repo, JWTService jtwService)
    {
        _repo = repo;
        _logger = logger;
        _jtwService = jtwService;
    }

    public async Task<ResponseItemDto<LoginUserSessionDto>> LoginUserAsync(LoginCredentialsDto usrCreds)
    {
        try
        {
            var _usrSession = await _repo.LoginUserAsync(usrCreds);

            _usrSession.Item.JwtToken = _jtwService.CreateJwtUserToken(_usrSession.Item);

            return _usrSession;
        }
        catch
        {
            //if there was an error during login, simply pass it on.
            throw;
        }
    }
}

