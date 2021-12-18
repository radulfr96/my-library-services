﻿using Microsoft.EntityFrameworkCore;
using MyLibrary.IDP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.IDP.DataLayer
{
    public class UserDataLayer : IUserDataLayer
    {
        private MyLibraryContext _context;

        public UserDataLayer(MyLibraryContext context)
        {
            _context = context;
        }

        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User> GetUserBySubject(string subject)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Subject == subject);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user != null)
            {
                user.UserClaims = await _context.UserClaims.Where(u => u.UserId == user.UserId).ToListAsync();
            }

            return user;
        }

        public async Task<List<UserClaim>> GetUserClaimsBySubject(string subject)
        {
            return await _context.UserClaims.Where(u => u.User.Subject == subject).ToListAsync();
        }

        public async Task<User> GetUserBySecurityCode(string securityCode)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.SecurityCode == securityCode && u.SecurityCodeExpirationDate >= DateTime.Now);
        }
    }
}