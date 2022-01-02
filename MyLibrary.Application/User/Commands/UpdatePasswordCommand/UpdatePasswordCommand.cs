﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using MyLibrary.Application.Common.Enums;
using MyLibrary.Application.Common.Exceptions;
using MyLibrary.Application.Interfaces;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.User.Commands.UpdatePasswordCommand
{
    public class UpdatePasswordCommand : IRequest<UpdatePasswordCommandDto>
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, UpdatePasswordCommandDto>
    {

        public IUserUnitOfWork _userUnitOfWork;
        public IUserService _userService;
        public IPasswordHasher<Persistence.Model.User> _passwordHasher;
        public IDateTimeService _dateTimeService;

        public UpdatePasswordCommandHandler(
            IUserUnitOfWork userUnitOfWork
            , IUserService userService
            , IPasswordHasher<Persistence.Model.User> passwordHasher
            , IDateTimeService dateTimeService
            )
        {
            _userUnitOfWork = userUnitOfWork;
            _userService = userService;
            _passwordHasher = passwordHasher;
            _dateTimeService = dateTimeService;
        }

        public async Task<UpdatePasswordCommandDto> Handle(UpdatePasswordCommand command, CancellationToken cancellationToken)
        {
            var response = new UpdatePasswordCommandDto();

            var userId = _userService.GetUserId();

            var user = await _userUnitOfWork.UserDataLayer.GetUser(userId);

            if (user == null)
                throw new UserNotFoundException("Unable to find user");

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, command.CurrentPassword);

            if (result == PasswordVerificationResult.Failed)
                throw new PasswordInvalidException(ErrorCodeEnum.PasswordInvalid, "The password you entered was not correct");

            var newHash = _passwordHasher.HashPassword(user, command.NewPassword);

            user.Password = newHash;
            user.ModifiedBy = userId;
            user.ModifiedDate = _dateTimeService.Now;
            await _userUnitOfWork.Save();

            return response;
        }
    }
}
