﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLibrary.Common.Responses;
using MyLibrary.Contracts.UnitOfWork;
using MyLibrary.Data.Model;
using MyLibrary.DataLayer;
using MyLibrary.DataLayer.Contracts;
using MyLibrary.Services;
using MyLibrary.UnitOfWork;
using NLog;

namespace MyLibrary.WebApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private MyLibraryContext _context;

        public UserController(MyLibraryContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult GetUsers()
        {
            try
            {
                IUserUnitOfWork userUnitOfWork = new UserUnitOfWork(_context);

                var service = new UserService(userUnitOfWork);
                var response = service.GetUsers();

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return Ok(response);
                    case HttpStatusCode.BadRequest:
                        return BadRequest(BuildBadRequestMessage(response));
                    case HttpStatusCode.NotFound:
                        return NotFound();
                    case HttpStatusCode.InternalServerError:
                        return StatusCode(StatusCodes.Status500InternalServerError);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to retreive users.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}