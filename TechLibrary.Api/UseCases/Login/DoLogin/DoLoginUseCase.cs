using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Api.Infrastructure.Security.Cripotagraphy;
using TechLibrary.Api.Infrastructure.Security.Tokens.Access;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Login.DoLogin;

public class DoLoginUseCase
{
    public ResponseRegisterUserJson Execute(RequestLoginJson request)
    {
        var dbContext = new TechLibraryDbContext();
        var user = dbContext.Users.FirstOrDefault(user => user.Email.Equals(request.Email));
        if (user is null)
        {
            throw new InvalidLoginException();
        }

        var cryptograph = new BCryptAlgorithm();
        var passwordIsValid = cryptograph.VerifyPassword(request.Password, user);
        if (passwordIsValid == false)
        {
            throw new InvalidLoginException();
        }

        var tokenGenerator = new JwtTokenGenerator();

        return new ResponseRegisterUserJson
        {
            Name = user.Name,
            AccessToken = tokenGenerator.Generate(user)
        };
    }
}
