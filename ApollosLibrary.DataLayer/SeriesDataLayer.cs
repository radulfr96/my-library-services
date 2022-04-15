﻿using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.DataLayer
{
    public class SeriesDataLayer : ISeriesDataLayer
    {
        private readonly ApollosLibraryContext _context;

        public SeriesDataLayer(ApollosLibraryContext context)
        {
            _context = context;
        }

        public async Task AddSeries(Series series)
        {
            await _context.Series.AddAsync(series);
        }

        public async Task DeleteSeries(int id)
        {
            _context.Series.Remove(await _context.Series.FirstOrDefaultAsync(g => g.SeriesId == id));
        }

        public async Task<Series> GetSeries(int id)
        {
            return await _context.Series
                .Include(s => s.Books)
                .FirstOrDefaultAsync(a => a.SeriesId == id);
        }

        public async Task<List<Series>> GetMultiSeries()
        {
            return await _context.Series.ToListAsync();
        }
    }
}
