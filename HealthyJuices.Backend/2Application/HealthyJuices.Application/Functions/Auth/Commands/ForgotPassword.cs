﻿using System;
using System.Threading;
using System.Threading.Tasks;
using HealthyJuices.Application.Providers;
using HealthyJuices.Common.Contracts;
using HealthyJuices.Common.Exceptions;
using HealthyJuices.Domain.Models.Users.DataAccess;
using MediatR;

namespace HealthyJuices.Application.Functions.Auth.Commands
{
    public static class ForgotPassword
    {
        // Command 
        public record Command(string Email) : IRequest { }

        // Handler
        public class Handler : IRequestHandler<Command>
        {
            private readonly IUserRepository _userRepository;
            private readonly ITimeProvider _timeProvider;
            private readonly EmailProvider _emailProvider;

            public Handler(IUserRepository repository, ITimeProvider timeProvider, EmailProvider emailProvider)
            {
                this._userRepository = repository;
                _timeProvider = timeProvider;
                _emailProvider = emailProvider;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.Query()
                    .ByEmail(request.Email)
                    .IsNotRemoved()
                    .FirstOrDefaultAsync();

                if (user == null)
                   throw new BadRequestException($"User with email '{request.Email}' not found");

                user.SetPermissionsToken(_timeProvider, Guid.NewGuid().ToString(), _timeProvider.UtcNow.AddDays(1));

                // TODO: get current url

                await _emailProvider.SendForgotPasswordEmail(user.Email, "http://localhost:4200/auth/reset-password", user.PermissionsToken.Token);
                await _userRepository.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}