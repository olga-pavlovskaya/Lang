﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class UserManager
    {
        public static List<User> GetAllUsers()
        {
            DataClassesDataContext db = new DataClassesDataContext();
            return db.Users.ToList();
        }

        public static User GetUser(int userid)
        {
            DataClassesDataContext db = new DataClassesDataContext();
            return db.Users.FirstOrDefault(u => u.id == userid);
        }

        public static User GetUser(string username, string pass)
        {
            DataClassesDataContext db = new DataClassesDataContext();
            return db.Users.FirstOrDefault(u => u.name == username && u.password == pass);
        }

        public static User GetUser()
        {
            DataClassesDataContext db = new DataClassesDataContext();
            return db.Users.FirstOrDefault();
        }

        public static void DeleteUser(int userid)
        {
            DataClassesDataContext db = new DataClassesDataContext();

            db.Users.Where(u => u.id == userid)
                .ToList()
                .ForEach(u => db.Users.DeleteOnSubmit(u));
            db.SubmitChanges();
        }

        private static int GetNextId()
        {
            DataClassesDataContext db = new DataClassesDataContext();
            int i = 0;
            db.Users.ToList().ForEach(u => { if (u.id > i) i = u.id; });
            return i + 1;
        }

        public static void AddUser(User user)
        {
            DataClassesDataContext db = new DataClassesDataContext();

            user.id = GetNextId();
            db.Users.InsertOnSubmit(user);
            db.SubmitChanges();
        }

        public static void UpdateUser(User user)
        {
            DataClassesDataContext db = new DataClassesDataContext();

            db.Users.Where(u => u.id == user.id)
                .ToList()
                .ForEach(u =>
                {
                    u.name = user.name;
                    u.password = user.password;
                    u.role = user.role;
                });
            db.SubmitChanges();
        }
    }

    public class Role
    {
        public const int USER = 0;
        public const int ADMIN = 1;
    }
}
