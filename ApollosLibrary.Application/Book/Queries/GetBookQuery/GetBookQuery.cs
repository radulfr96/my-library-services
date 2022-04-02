﻿using MediatR;
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Book.Queries.GetBookQuery
{
    public class GetBookQuery : IRequest<GetBookQueryDto>
    {
        public int BookId { get; set; }
    }

    public class GetBookQueryHandler : IRequestHandler<GetBookQuery, GetBookQueryDto>
    {
        public IBookUnitOfWork _bookUnitOfWork { get; set; }

        public GetBookQueryHandler(IBookUnitOfWork bookUnitOfWork)
        {
            _bookUnitOfWork = bookUnitOfWork;
        }

        public async Task<GetBookQueryDto> Handle(GetBookQuery query, CancellationToken cancellationToken)
        {
            var response = new GetBookQueryDto();

            var book = await _bookUnitOfWork.BookDataLayer.GetBook(query.BookId);

            if (book == null)
            {
                throw new BookNotFoundException($"Unable to get find book with id {query.BookId}");
            }

            response.BookId = book.BookId;
            response.CoverImage = book.CoverImage;
            response.Edition = book.Edition;
            response.eISBN = book.EIsbn;
            response.FictionTypeId = book.FictionTypeId;
            response.FormTypeId = book.FormTypeId;
            response.ISBN = book.Isbn;
            response.NumberInSeries = book.NumberInSeries;
            response.PublicationFormatId = book.PublicationFormatId;
            response.PublisherId = book.PublisherId;
            response.SeriesId = book.SeriesId;
            response.Subtitle = book.Subtitle;
            response.Title = book.Title;
            response.Authors = book.Authors.Select(a => a.AuthorId).ToList();
            response.Genres = book.Genres.Select(g => g.GenreId).ToList();
            return response;
        }
    }
}
