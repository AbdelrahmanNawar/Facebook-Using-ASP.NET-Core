using Microsoft.EntityFrameworkCore;
using MVCProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVCProject.DataRepositories
{
    public class CommentRepository : IDataRepository<Comment, int>
    {
        FacebookContext context;
        public CommentRepository(FacebookContext _context)
        {
            context = _context;
        }

        public void Delete(int id)
        {
            try
            {
                var comment = context.Comments.FirstOrDefault(c => c.CommentId == id);
                if (comment == null)
                    return;
                comment.IsDeleted = true;
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Insert(Comment t)
        {
            try
            {
                context.Comments.Add(t);
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
                var comment = context.Comments.FirstOrDefault<Comment>(c => c.CommentId == id);
                if (comment == null)
                    return false;
                else
                    return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Comment> SelectAll()
        {
            try
            {
                var comments= context.Comments.ToList();
                return comments;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Comment SelectById(int id)
        {
            try
            {
                return context.Comments.FirstOrDefault(comment => comment.CommentId == id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(int id, Comment t)
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
