﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyLibrary.Common.Requests;
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
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private readonly MyLibraryContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(MyLibraryContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("")]
        public IActionResult GetUsers()
        {
            try
            {
                IUserDataLayer userDataLayer = new UserDataLayer(_context);
                IRoleDataLayer roleDataLayer = new RoleDataLayer(_context);
                IUserUnitOfWork userUnitOfWork = new UserUnitOfWork(userDataLayer, roleDataLayer);

                var service = new UserService(userUnitOfWork, _configuration);
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

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                IUserDataLayer userDataLayer = new UserDataLayer(_context);
                IRoleDataLayer roleDataLayer = new RoleDataLayer(_context);
                IUserUnitOfWork userUnitOfWork = new UserUnitOfWork(userDataLayer, roleDataLayer);

                var service = new UserService(userUnitOfWork, _configuration);
                var response = service.Login(request);

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
                s_logger.Error(ex, "Unable to login user.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("userinfo")]
        public IActionResult GetUserInfo()
        {
            var response = new GetUserInfoResponse();

            try
            {
                var user = _httpContextAccessor.HttpContext.User;
                response.Username = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
                response.Roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
                response.StatusCode = HttpStatusCode.OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Unable to login user.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}