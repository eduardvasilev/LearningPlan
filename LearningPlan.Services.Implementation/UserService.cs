﻿using LearningPlan.DataAccess;
using LearningPlan.DomainModel;
using LearningPlan.Services.Model;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LearningPlan.DomainModel.Exceptions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace LearningPlan.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IReadRepository<User> _userReadRepository;
        private readonly IWriteRepository<User> _userWriteRepository;

        public UserService(IReadRepository<User> userReadRepository, IWriteRepository<User> userWriteRepository)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
        }

        public async Task<AuthenticateResponseModel> AuthenticateAsync(AuthenticateRequestModel model)
        {
            var user = _userReadRepository.GetAll().SingleOrDefault(x => x.Username == model.Username);

            if (user == null || user.Password != HashPassword(model.Password, user.Salt)) return null;
            
            var token = GenerateToken(user, model.Secret);

            return await Task.FromResult(new AuthenticateResponseModel(user.Id, user.Username, token));
        }

        public async Task SignInAsync(SignInServiceModel model)
        {
            //consider to rid of AsEnumerable()
            if (_userReadRepository.GetAll().Where(x => x.Username == model.Username).AsEnumerable().Any())
            {
                throw new DomainServicesException($"User with login '{model.Username}' is already exists.");
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
                Salt = salt
            };
            await _userWriteRepository.CreateAsync(user);

            await _userWriteRepository.SaveChangesAsync();
        }

        public async Task<User> GetByIdAsync(string id)
        {
           return await Task.FromResult(_userReadRepository.GetAll().SingleOrDefault(user => user.Id == id));
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