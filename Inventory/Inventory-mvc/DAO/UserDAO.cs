using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.DAO
{
    public class UserDAO : IUserDAO
    {
        List<User> IUserDAO.GetAllUser()
        {
            using (StationeryModel entity = new StationeryModel())
            {
                return (from u in entity.User select u).ToList<User>();
            }
        }

        User IUserDAO.FindByUserID(string userID)
        {
            string userid = userID.ToUpper().Trim();
            using (StationeryModel entity = new StationeryModel())
            {
                return (from user in entity.User where user.userID == userid select user).FirstOrDefault();
            }
        }

        bool IUserDAO.AddNewUser(User user)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                entity.User.Add(user);
                int rowAffected = entity.SaveChanges();

                if (rowAffected == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        int IUserDAO.UpdateUserInfo(User user)
        {
            using (StationeryModel entity = new StationeryModel())
            {
                try
                {
                    User u = (from a in entity.User where a.userID == user.userID select a).LastOrDefault();
                    u = user;
                    int rowAffected = entity.SaveChanges();
                    return rowAffected;
                }
                catch (Exception)
                {
                    throw new Exception("incorrect userid");
                }
            }
        }

        //bool IUserDAO.DeleteUser(string userID)
        //{
        //    using (StationeryModel entity = new StationeryModel())
        //    {

        //    }
        //}

    }
}