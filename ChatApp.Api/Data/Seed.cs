﻿using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ChatApp.Api.Data;

public static class Seed
{
    public static async void SeedData(this IApplicationBuilder builder)
    {
        Console.WriteLine("------> Seeding Datas");
        using var serviceScope = builder.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
        var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();
        var mapper = serviceScope.ServiceProvider.GetService<IMapper>();

        var registeredUsers = new RegisterDto[]
        {
            new RegisterDto()
            {
                UserName = "djoufson",
                Email = "djoufson@example.com",
                Password = "String 1",
                PhoneNumber = "699254549"
            },
            new RegisterDto()
            {
                UserName = "johndoe",
                Email = "doe@example.com",
                Password = "String 2",
                PhoneNumber = "699254549"
            },
            new RegisterDto()
            {
                UserName = "janesmith",
                Email = "jane@example.com",
                Password = "String 3",
                PhoneNumber = "699254549"
            },
        };

        var users = mapper?.Map<AppUser[]>(registeredUsers);
        for (int i = 0; i < users?.Length; i++)
        {
            var existingUser = await userManager?.FindByEmailAsync(users[i].Email)!;
            if (existingUser is not null)
                continue;

            await userManager?.CreateAsync(users[i], registeredUsers[i].Password)!;
        }
    }

    public static void ResetData(this IApplicationBuilder builder)
    {
        Console.WriteLine("------> Reseting Database");
        using var serviceScope = builder.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

        for (int i = 0; i < context?.Users.Count(); i++)
        {
            var user = context.Users.First();
            context.Users.Remove(user);
            context?.SaveChanges();
        }

        context?.SaveChanges();
    }
}