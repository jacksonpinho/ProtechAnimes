﻿namespace Animes.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> Authenticate(string username, string password);

    }

}
