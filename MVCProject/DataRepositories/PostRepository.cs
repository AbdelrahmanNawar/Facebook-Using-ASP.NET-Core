using Microsoft.EntityFrameworkCore;
using MVCProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCProject.DataRepositories
{
    public class PostRepository : IDataRepository<Post, int>
    {
        FacebookContext context;
        public PostRepository(FacebookContext _context)
        {
            context = _context;
        }

        public void Delete(int id)
        {
            try
            {
                var post = context.Posts.Find(id);
                if (post == null)
                    return;
                var postLikes = context.Likes.Where(l => l.PostId == id);
                foreach (var like in postLikes)
                {
                    like.IsLiked = false;
                    context.Entry(like).State = EntityState.Modified;
                }
                var postComments = context.Comments.Where(c => c.PostId == id);
                foreach (var comment in postComments)
                {
                    comment.IsDeleted = true;
                    context.Entry(comment).State = EntityState.Modified;
                }
                post.IsDeleted = true;
                context.Entry(post).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Delete(Post t)
        {
            throw new NotImplementedException();
        }

        public void Insert(Post t)
        {
            try
            {
                context.Posts.Add(t);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void DeleteById(int id)
        {
            try
            {
                var post = context.Posts.Find(id);
                if (post == null)
                    return;
                var postLikes = context.Likes.Where(l => l.PostId == id);
                foreach (var like in postLikes)
                {
                    like.IsLiked = false;
                    context.Entry(like).State = EntityState.Modified;
                }
                var postComments = context.Comments.Where(c => c.PostId == id);
                foreach (var comment in postComments)
                {
                    comment.IsDeleted = true;
                    context.Entry(comment).State = EntityState.Modified;
                }
                post.IsDeleted = true;
                context.Entry(post).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool IsExist(int id)
        {
            var post = context.Posts.FirstOrDefault(p => p.PostId == id);
            if (post == null)
                return false;
            else
                return true;
        }

        public List<Post> SelectAll()
        {
            try
            {
                var posts = context.Posts.Include(p => p.Comments).Include(l => l.Likes).OrderBy(p => p.PostDateTime).ToList();
                return posts;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Post SelectById(int id)
        {
            try
            {
                return context.Posts.Find(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Post> SelectByUserId(string id)
        {
            try
            {
                return context.Posts.Where(p=>p.UserId == id).Include(u => u.Comments).Include(u => u.Likes).OrderBy(p=>p.PostDateTime).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(int id, Post t)
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
