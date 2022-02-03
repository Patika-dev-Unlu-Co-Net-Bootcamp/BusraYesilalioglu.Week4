﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UnluCo.Week1.HomeWork.Models;

namespace UnluCo.Week1.HomeWork
{
    public class TokenGenerator
    {
      
        private readonly IConfiguration _configuration;
        public TokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        
        }

        public Token CreateToken(Person person)
        {

            DateTime exp = DateTime.Now.AddMinutes(30);
            Token token = new Token();
            token.Expiration = exp;

          
           

            SymmetricSecurityKey securityKey =
                new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken securityToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: exp, notBefore: DateTime.Now,
                signingCredentials: signingCredentials,
                claims: new Claim[] {
                     new Claim("PersonId",person.Id.ToString())});



            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string accessToken = tokenHandler.WriteToken(securityToken);
            token.AccessToken = accessToken;
            token.RefreshToken = Guid.NewGuid().ToString();

            return token;
        }
    }
}