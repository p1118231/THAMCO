using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Auth0.AuthenticationApi;
using System.Net.Http.Headers;
using System.Text.Json;
using Auth0.AuthenticationApi.Models;
using THAMCOMVC.Data;
using THAMCOMVC.Models;
using THAMCOMVC.Support;
using Microsoft.AspNetCore.Authorization;
using SQLitePCL;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;
using System.ComponentModel.DataAnnotations;
using THAMCOMVC.Interfaces;


namespace THAMCOMVC.Controllers
{
    public class AccountController : Controller

    {
        private readonly IConfiguration _configuration;
        
        private readonly IAccountRepository _repository;
      
        private string? _accessToken;
        private DateTime _tokenExpiry;

        private readonly bool _isTestEnvironment;

        

        public AccountController(IAccountRepository repository,IConfiguration configuration, bool isTestEnvironment = false)
        {
            
            _configuration = configuration;
            _repository = repository;
            _isTestEnvironment = isTestEnvironment;
        }

       
        //enable auth0 login
        public IActionResult Login()
        {
        return Challenge(new AuthenticationProperties { RedirectUri = "/" }, "Auth0");
        }

        //logout logic 
        public async Task Logout()
        {
        await HttpContext.SignOutAsync("Auth0");
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        // GET: Account/Details/5
        [Authorize]
        public async Task<IActionResult> Details(string? id)
        {
            
            if (id == null)
            {
                id = Auth0UserHelper.GetAuth0UserId(User);
                
            }

            var user = await _repository.GetUserByAuth0IdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Account/Create
        public IActionResult Create()
        {
            return View();
        }

            //get access token
         private async Task<string> GetAccessTokenAsync()
        {
            // If the token is still valid, return the cached token
            if (!string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow < _tokenExpiry)
            {
                return _accessToken;
            }

            using var httpClient = new HttpClient();

            // Define the payload
            var payload = new
            {
                client_id = _configuration["Auth0:ClientId"],
                client_secret = _configuration["Auth0:ClientSecret"],
                audience = _configuration["Auth0:Audience"],
                grant_type = "client_credentials",
                scope = "read:users create:users update:users"
            };

            // Send POST request to Auth0
            var response = await httpClient.PostAsJsonAsync("https://p1118231.uk.auth0.com/oauth/token", payload);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to retrieve access token. Status: {response.StatusCode}, Content: {errorDetails}");
            }

            var content = await response.Content.ReadAsStringAsync();

            // Extract access token and expiry time from the JSON response
            var jsonDocument = JsonDocument.Parse(content);
            if (!jsonDocument.RootElement.TryGetProperty("access_token", out var accessTokenElement) ||
                !jsonDocument.RootElement.TryGetProperty("expires_in", out var expiresInElement))
            {
                throw new Exception("Access token or expiry time not found in response.");
            }

            _accessToken = accessTokenElement.GetString();
            _tokenExpiry = DateTime.UtcNow.AddSeconds(expiresInElement.GetInt32());

            return _accessToken;
        }
        // POST: Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,PaymentAddress,Password,PhoneNumber")] User user)
        {
            if (_isTestEnvironment)
            {
                await _repository.AddUserAsync(user);
                await _repository.SaveChangesAsync();
                return RedirectToAction("Login");
            }

        if (ModelState.IsValid)
        {
        
        // Get the access token using Auth0's SDK
        var accessToken = await GetAccessTokenAsync();
        


        // Send user info to Auth0 using the obtained access token
        var auth0User = new
        {   
            email = user.Email,
            password = user.Password,
            phone_number = user.PhoneNumber,
            given_name = user.FirstName,
            family_name = user.LastName,
            name= user.FirstName,
            connection = "THAMCO-DB",
            
            
        };

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await httpClient.PostAsJsonAsync("https://p1118231.uk.auth0.com/api/v2/users", auth0User);

         if (!response.IsSuccessStatusCode)
        {
            // Handle the error
            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, "Error creating user: " + error);
            return View(user);
        }
        response.EnsureSuccessStatusCode();

        var auth0ResponseContent = await response.Content.ReadAsStringAsync();
        var auth0ResponseJson = JsonDocument.Parse(auth0ResponseContent);
        var auth0UserId = auth0ResponseJson.RootElement.GetProperty("user_id").GetString();

        user.Auth0UserId = auth0UserId;
        await _repository.AddUserAsync(user);
        await _repository.SaveChangesAsync();

      return RedirectToAction("Login");
        }

        return View(user);
        }


        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _repository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        [HttpGet]
        public async Task<IActionResult> EditField(int? id, string field)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _repository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Field = field;
            ViewBag.FieldValue = field switch
            {
                "FirstName" => user.FirstName,
                "LastName" => user.LastName,
                "Email" => user.Email,
                "Password" => string.Empty, // Do not show the password
                "PhoneNumber" => user.PhoneNumber,
                "PaymentAddress" => user.PaymentAddress,
                _ => throw new Exception("Invalid field.")
            };

            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditField(int id, string field, string newValue)
        {
            Console.Write("here");
            var user = await _repository.GetUserByIdAsync(id);
            
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                // Local update
                switch (field)
                {
                    case "FirstName":
                        user.FirstName = newValue;
                        break;
                    case "LastName":
                        user.LastName = newValue;
                        break;
                    case "Email":
                       // Validate email format server-side
                        if (!new EmailAddressAttribute().IsValid(newValue))
                        {
                            ModelState.AddModelError(string.Empty, "Invalid email format.");
                            return View(user);
                        }
                        user.Email = newValue;
                        break;
                    case "Password":
                        user.Password = BCrypt.Net.BCrypt.HashPassword(newValue); // Hash locally
                        break;
                    case "PaymentAddress":
                        user.PaymentAddress = newValue;
                        break;
                    case "PhoneNumber":
                        user.PhoneNumber = newValue;
                        break;
                    default:
                        throw new Exception("Invalid field.");
                }

               
                // Update Auth0
                if (!string.IsNullOrEmpty(user.Auth0UserId))
                {
                    var accessToken = await GetAccessTokenAsync();
                    using var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    if (field == "Email")
                    {
                        var emailPayload = new { email = newValue, email_verified = false };
                        var response = await httpClient.PatchAsJsonAsync(
                            $"https://p1118231.uk.auth0.com/api/v2/users/{user.Auth0UserId}",
                            emailPayload
                        );
                        if (!response.IsSuccessStatusCode)
                        {
                            var errorDetails = await response.Content.ReadAsStringAsync();
                            throw new Exception($"Failed to update email in Auth0: {errorDetails}");
                        }
                    }
                    else if (field == "Password")
                    {
                        var passwordPayload = new { password = newValue };
                        var response = await httpClient.PatchAsJsonAsync(
                            $"https://p1118231.uk.auth0.com/api/v2/users/{user.Auth0UserId}",
                            passwordPayload
                        );
                        if (!response.IsSuccessStatusCode)
                        {
                            var errorDetails = await response.Content.ReadAsStringAsync();
                            throw new Exception($"Failed to update password in Auth0: {errorDetails}");
                        }
                    }
                    else if (field == "FirstName")
                    {
                        var namePayload = new { name = newValue };
                        var response = await httpClient.PatchAsJsonAsync(
                            $"https://p1118231.uk.auth0.com/api/v2/users/{user.Auth0UserId}",
                            namePayload
                        );
                        if (!response.IsSuccessStatusCode)
                        {
                            var errorDetails = await response.Content.ReadAsStringAsync();
                            throw new Exception($"Failed to update password in Auth0: {errorDetails}");
                        }
                    }
                    else if (field == "PhoneNumber")
                    {
                        var phone_numberPayload = new { phone_number = newValue };
                        var response = await httpClient.PatchAsJsonAsync(
                            $"https://p1118231.uk.auth0.com/api/v2/users/{user.Auth0UserId}",
                            phone_numberPayload
                        );
                        if (!response.IsSuccessStatusCode)
                        {
                            var errorDetails = await response.Content.ReadAsStringAsync();
                            throw new Exception($"Failed to update password in Auth0: {errorDetails}");
                        }
                    }
                }

                 // Save local changes
                await _repository.UpdateUser(user);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error updating {field}: {ex.Message}");
                return View(user);
            }

            return RedirectToAction(nameof(Details));
        }



        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                id = Auth0UserHelper.GetAuth0UserId(User);
            }

            var user = await _repository.GetUserByAuth0IdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           var user = await _repository.GetUserByIdAsync(id);
           /* if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            */
            return RedirectToAction(nameof(Logout));
        }

        private bool UserExists(int id)
        {
            return _repository.UserExists(id);
        }
    }
}
