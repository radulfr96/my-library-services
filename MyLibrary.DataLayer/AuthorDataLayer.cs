﻿using Microsoft.EntityFrameworkCore;
using MyLibrary.DataLayer.Contracts;
using MyLibrary.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.DataLayer
{
    public class AuthorDataLayer : IAuthorDataLayer
    {
        private MyLibraryContext _context;

        public AuthorDataLayer(MyLibraryContext context)
        {
            _context = context;
        }

        public async Task AddAuthor(Author author)
        {
            await _context.Authors.AddAsync(author);
        }

        public async Task DeleteAuthor(Author author)
        {
            await Task.FromResult(_context.Authors.Remove(author));
        }

        public async Task<Author> GetAuthor(int id)
        {
            return await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == id);
        }

        public async Task<List<Author>> GetAuthors()
        {
            return await _context.Authors.Include("Country").ToListAsync();
        }
    }
}
