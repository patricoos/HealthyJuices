﻿using System;
using HealthyJuices.Shared.Enums;

namespace HealthyJuices.Shared.Dto
{
    public record UserDto(long Id, string Email, UserRole Roles, long? CompanyId, DateTime DateCreated, DateTime DateModified, bool IsRemoved);
}