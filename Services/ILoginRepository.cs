using System;
using RssMob.Models;

namespace RssMob.Services
{
    public interface ILoginRepository
    {
        Task<Employee> Login(string email, string password);
    }
}

