using Microsoft.EntityFrameworkCore;
using MVCProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCProject.DataRepositories
{
    public class LikeRepository : IDataRepository<Like, int>
    {
        FacebookContext context;
        public LikeRepository(FacebookContext _context)
        {
            context = _context;
        }

        public void Delete(int id)
        {
            //try
            //{
            //    var like = context.Likes.Find(id);
            //    if (like == null)
            //        return;
            //    like.IsLiked = false;
            //    context.SaveChanges();
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}
        }

        public void Insert(Like t)
        {
            try
            {
                context.Likes.Add(t);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Like> SelectAll()
        {
            try
            {
                var likes = context.Likes.ToList();
                return likes;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Like SelectById(int id)
        {
            try
            {
                return context.Likes.FirstOrDefault(like => like.PostId == id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(int id, Like t)
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

        public bool IsExist(int id)
        {
            try
            {
                var like = context.Likes.SingleOrDefault<Like>(l => l.PostId == id);
                if (like == null)
                    return false;
                else
                    return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

