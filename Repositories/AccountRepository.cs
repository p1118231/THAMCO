using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using THAMCOMVC.Controllers;
using THAMCOMVC.Data;
using THAMCOMVC.Interfaces;
using THAMCOMVC.Models;

namespace THAMCOMVC.Repositories;


public class AccountRepository : IAccountRepository
{
    private readonly AccountContext _context;

    public AccountRepository(AccountContext context)
    {
        _context = context;
    }


    public async Task<User?> GetUserByIdAsync(int? id)
    {
        return await _context.User.FindAsync(id);
    }

    public async Task AddUserAsync(User user)
    {
        await _context.User.AddAsync(user);
    }

    public async Task<User?> GetUserByAuth0IdAsync(string? id)
    {
        return await _context.User.FirstOrDefaultAsync(m => m.Auth0UserId == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

   public async Task<bool> UpdateUser(User user)
    {
        var existingUser = await _context.User.FindAsync(user.Id);
        if (existingUser == null)
        {
            return false; // User not found
        }

        // Update the fields
        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Email = user.Email;
        existingUser.PaymentAddress = user.PaymentAddress;
        existingUser.Password = user.Password;
        existingUser.PhoneNumber = user.PhoneNumber;
        existingUser.Auth0UserId = user.Auth0UserId;

        // Save changes
        _context.User.Update(existingUser);
        await _context.SaveChangesAsync();
        return true;
    }

    public bool UserExists(int id)
    {
        return _context.User.Any(e => e.Id == id);
    }


    
}
