using System;
using Microsoft.EntityFrameworkCore;
using odec.Server.Model.Category;
using odec.Server.Model.User;
using odec.Server.Model.Work.Models;

namespace Work.DAL.Tests
{
    public static class WorkTestHelper
    {
        internal static void PopulateDefaultDataWorkCtx(DbContext db)
        {
            try
            {
                //db.Set<Role>().Add(new Role
                //{
                //    Id = 1,
                //    Name = "Crafter"

                //});
                //db.Set<Role>().Add(new Role
                //{
                //    Id = 2,
                //    Name = "User",
                //});
                //db.Set<UserRole>().Add(new UserRole {RoleId = 1,UserId = 1});
               
                db.Set<User>().Add(new User
                {
                    Id = 1,
                    UserName = "Andrew",

                });
                db.Set<User>().Add(new User
                {
                    Id = 2,
                    UserName = "Alex",
                });
                db.Set<WorkType>().Add(new WorkType
                {
                    Code = "FIX",
                    Name = "Fix Price",
                    DateCreated = DateTime.Now,
                    IsActive = true,
                    SortOrder = 0
                });
                db.Set<Category>().Add(new Category
                {
                    Code = "FIX",
                    Name = "Blacksmith",
                    DateCreated = DateTime.Now,
                    IsActive = true,
                    SortOrder = 0
                });
                db.Set<Category>().Add(new Category
                {
                    Code = "FIX2",
                    Name = "Carpenter",
                    DateCreated = DateTime.Now,
                    IsActive = true,
                    SortOrder = 0
                });

                db.SaveChanges();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        internal static void PopulateDefaultDataPortfolioCtx(DbContext db)
        {
            try
            {
                //db.Set<Role>().Add(new Role
                //{
                //    Id = 1,
                //    Name = "Crafter"

                //});
                //db.Set<Role>().Add(new Role
                //{
                //    Id = 2,
                //    Name = "User",
                //});
                //db.Set<UserRole>().Add(new UserRole { RoleId = 1, UserId = 1 });

                db.Set<User>().Add(new User
                {
                    Id = 1,
                    UserName = "Andrew",

                });
                db.Set<User>().Add(new User
                {
                    Id = 2,
                    UserName = "Alex",
                });
               

                db.SaveChanges();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
