using LearningPlan.DomainModel;
using LearningPlan.DomainModel.Exceptions;
using LearningPlan.ObjectServices;
using LearningPlan.Services.Model;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LearningPlan.Infrastructure;
using LearningPlan.Infrastructure.Model;

namespace LearningPlan.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserObjectService _userObjectService;
        private readonly IPasswordService _passwordService;
        private readonly IUserActivationCodeObjectService _userActivationCodeObjectService;
        private readonly IEmailSender _emailSender;

        public UserService(IUserObjectService userObjectService, 
            IPasswordService passwordService, 
            IUserActivationCodeObjectService userActivationCodeObjectService,
            IEmailSender emailSender)
        {
            _userObjectService = userObjectService;
            _passwordService = passwordService;
            _userActivationCodeObjectService = userActivationCodeObjectService;
            _emailSender = emailSender;
        }

        public async Task<AuthenticateResponseModel> AuthenticateAsync(AuthenticateRequestModel model)
        {
            var user = await _userObjectService.GetUserByUserNameAsync(model.Username);

            if (user == null || user.Password != HashPassword(model.Password, user.Salt)) return null;

            if (!user.IsApproved)
            {
                throw new DomainServicesException($"User {user.Username} is not approved.");
            }

            var token = GenerateToken(user, model.Secret);

            return await Task.FromResult(new AuthenticateResponseModel(user.Id, user.Username, token));
        }

        public async Task SignUpAsync(SignInServiceModel model)
        {
            _passwordService.ValidatePassword(model.Password);
            if ((await _userObjectService.GetUserByUserNameAsync(model.Username) != null))
            {
                throw new DomainServicesException($"User with login '{model.Username}' already exists.");
            }

            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            User user = new User
            {
                Password = HashPassword(model.Password, salt),
                Username = model.Username,
                Salt = salt,
                IsApproved = false
            };

            await _userObjectService.CreateUserAsync(user);

            UserActivationCode code = await _userActivationCodeObjectService.CreateCodeAsync(user);

            await _emailSender.SendAsync(
                new Message(new List<string> { user.Username }, "Activate your account", code.Code.ToString()));
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _userObjectService.GetByIdAsync<User>(id);
        }

        public async Task ActivateUserAsync(Guid activationCode)
        {
            UserActivationCode code = await _userActivationCodeObjectService.GetCodeAsync(activationCode);
            User user = await _userObjectService.GetByIdAsync<User>(code.UserId);

            if (user == null)
            {
                throw new DomainServicesException("User not found.");
            }

            user.IsApproved = true;
            await _userObjectService.UpdateAsync(user);
            await _userActivationCodeObjectService.DeleteAsync(code);
        }

        private string GenerateToken(User user, string secret)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string HashPassword(string password, byte[] salt)
        {
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }
    }
}