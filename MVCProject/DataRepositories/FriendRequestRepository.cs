using Microsoft.EntityFrameworkCore;
using MVCProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCProject.DataRepositories
{
    public class FriendRequestRepository : IDataRepository<FriendRequest, string>
    {
        FacebookContext context;
        public FriendRequestRepository(FacebookContext _context)
        {
            context = _context;
        }

        public void Delete(string id)
        {
            try
            {
                var friendRequest = context.FriendRequests.Find(id);
                if (friendRequest == null)
                    return;
                context.Remove(friendRequest);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Insert(FriendRequest t)
        {
            try
            {
                context.FriendRequests.Add(t);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool IsExist(string id)
        {
            var friendRequest = context.FriendRequests.Find(id);
            if (friendRequest == null)
                return false;
            else
                return true;
        }

        public List<FriendRequest> SelectAll()
        {
            try
            {
                var friendRequests = context.FriendRequests.ToList();
                return friendRequests;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public FriendRequest SelectById(string id)
        {
            try
            {
                return context.FriendRequests.Find(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(string id, FriendRequest t)
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
