﻿using ApollosLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.DataLayer.Contracts
{
    /// <summary>
    /// Used to handle the storage of series
    /// </summary>
    public interface ISeriesDataLayer
    {
        /// <summary>
        /// Used to the the series received
        /// </summary>
        /// <param name="series">The series to be added</param>
        Task AddSeries(Series series);

        /// <summary>
        /// Used to get a series by its id
        /// </summary>
        /// <param name="id">The id of the series to be found</param>
        /// <returns>The series with the id received</returns>
        Task<Series> GetSeries(int id);

        /// <summary>
        /// Used to get all series
        /// </summary>
        /// <returns>The list of series</returns>
        Task<List<Series>> GetMultiSeries();

        /// <summary>
        /// Used to remove an series from the database
        /// </summary>
        /// <param name="id">The id of the series to be deleted</param>
        Task DeleteSeries(Series series);
    }
}
