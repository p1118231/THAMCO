
using THAMCOMVC.Models;
using THAMCOMVC.Data;


namespace THAMCOMVC.Interfaces;


public interface IAccountRepository
{
    
    Task<User?> GetUserByIdAsync(int? id);
    Task AddUserAsync(User user);

    Task<bool> UpdateUser(User user);

    bool UserExists(int id);
 

    Task SaveChangesAsync();

    Task<User?> GetUserByAuth0IdAsync(string? id);
}
