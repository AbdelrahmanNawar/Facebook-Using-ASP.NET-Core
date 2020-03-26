using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVCProject.DataRepositories
{
    public class UserRepository : IDataRepository<User, string>
    {
        FacebookContext context;
        public UserRepository([FromServices] FacebookContext _context)
        {
            context = _context;
        }
        public void Delete(string id)
        {
            try
            {
                var user = context.Users.Find(id);
                if (user == null)
                    return;
                user.IsDeleted = true;
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Insert(User t)
        {
            try
            {
                context.Users.Add(t);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool IsExist(string id)
        {
            var user = context.Users.Find(id);
            if (user == null)
                return false;
            else
                return true;
        }

        public List<User> SelectAll()
        {
            try
            {
                var users = context.Users.ToList();
                return users;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<User> SelectAllWithPosts()
        {
            try
            {
                var users = context.Users.ToList();
                return users;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public User SelectById(string id)
        {
            try
            {
                return context.Users.Include(uPost => uPost.Posts).FirstOrDefault(u => u.Id == id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(string id, User t)
        {
            try
            {
                context.Entry(t).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
