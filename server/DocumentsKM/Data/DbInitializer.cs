using DocumentsKM.Data;
using DocumentsKM.Models;
using System.Collections.Generic;
using System.Linq;
 
namespace DocumentsKM
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationContext ctx)
        {
            ctx.Database.EnsureCreated();

            List<User> users = new List<User>
            {
                new User
                {
                    Login = "1",
                    Password = BCrypt.Net.BCrypt.HashPassword("1"),
                    EmployeeId = 11,
                },
                new User
                {
                    Login = "2",
                    Password = BCrypt.Net.BCrypt.HashPassword("2"),
                    EmployeeId = 27,
                },
            };
            if (!ctx.Users.Any())
            {
                ctx.Users.AddRange(users);
                ctx.SaveChanges();
                ctx.Users.AttachRange(users);
            }

            List<DefaultValues> defaultValues = new List<DefaultValues>
            {
                new DefaultValues
                {
                    User = users[0],
                    DepartmentId = 1,
                    CreatorId = users[0].EmployeeId,
                },
                new DefaultValues
                {
                    User = users[1],
                    DepartmentId = 4,
                    CreatorId = users[1].EmployeeId,
                },
            };
            if (!ctx.DefaultValues.Any())
            {
                ctx.DefaultValues.AddRange(defaultValues);
                ctx.SaveChanges();
            }
        }
    }
}
