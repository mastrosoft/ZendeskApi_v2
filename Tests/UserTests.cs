﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using ZenDeskApi_v2;
using ZenDeskApi_v2.Models.Constants;
using ZenDeskApi_v2.Models.Shared;
using ZenDeskApi_v2.Models.Tickets;
using ZenDeskApi_v2.Models.Users;


namespace Tests
{
    [TestFixture]
    public class UserTests
    {        
        ZenDeskApi api = new ZenDeskApi(Settings.Site, Settings.Email, Settings.Password);

        [Test]
        public void CanGetUsers()
        {
            var res = api.Users.GetAllUsers();
            Assert.True(res.Count > 0);
        }

        [Test]
        public void CanGetUser()
        {
            var res = api.Users.GetUser(Settings.UserId);
            Assert.True(res.User.Id == Settings.UserId);
        }

        [Test]
        public void CanGetUsersInGroup()
        {
            var res = api.Users.GetUsersInGroup(Settings.GroupId);
            Assert.True(res.Count > 0);
        }

        [Test]
        public void CanGetUsersInOrg()
        {
            var res = api.Users.GetUsersInOrganization(Settings.OrganizationId);
            Assert.True(res.Count > 0);
        }

        [Test]
        public void CanCreateUpdateSuspendAndDeleteUser()
        {
            var user = new User()
                           {
                              Name = "test user3",
                              Email = "test3@test.com",
                           };

            var res1 = api.Users.CreateUser(user);
            Assert.IsTrue(res1.User.Id > 0);

            res1.User.Phone = "555-555-5555";
            var res2 = api.Users.UpdateUser(res1.User);
            Assert.AreEqual(res1.User.Phone, res2.User.Phone);

            var res3 = api.Users.SuspendUser(res2.User.Id.Value);
            Assert.IsTrue(res3.User.Suspended);

            var res4 = api.Users.DeleteUser(res3.User.Id.Value);
            Assert.True(res4);
        }

        //Bulk Create users is hard to test because of we don't know how long the job will take to complete. Test should pass if you run individually but might cause problems in parallel.
        //[Test]
        //public void CanBulkCreateUsers()
        //{            
        //    var users = new List<User>()
        //    {
        //        new User()
        //            {
        //                Name = "test user7",
        //                Email = "test7@test.com",
        //            },
        //        new User()
        //            {
        //                Name = "test user8",
        //                Email = "test8@test.com",
        //            },
        //    };

        //    var res1 = api.Users.BulkCreateUsers(users);
        //    Assert.IsNotEmpty(res1.JobStatus.Id);            

        //    Thread.Sleep(2000);

        //    var user1 = api.Users.SearchByEmail(users[0].Email);
        //    var user2 = api.Users.SearchByEmail(users[1].Email);
        //    Assert.True(api.Users.DeleteUser(user1.Users[0].Id.Value));
        //    Assert.True(api.Users.DeleteUser(user2.Users[0].Id.Value));
        //}

        [Test]
        public void CanFindUser()
        {            
            var res1 = api.Users.SearchByEmail(Settings.Email);
            Assert.True(res1.Users.Count > 0);            
        }

        [Test]
        public void CanGetCurrentUser()
        {
            var res1 = api.Users.GetCurrentUser();
            Assert.True(res1.User.Id > 0);
        } 
    }
}