using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoardGameClub;
using BoardGameClub.Controllers;
using BoardGameClub.Models;

namespace login.Tests
{
    [TestClass()]
    public class AuthenticationTest
    {
        AccountController controller = new AccountController();
        LoginViewModel loginView = new LoginViewModel();
        [TestMethod]
        public void LoginTestWithoutModel()
        {
            var results = controller.Login("Home");

            
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void LoginTestWithNullModel()
        {
            var model = new LoginViewModel();
            model.Email = "petetjensen@gmail.com";
            model.Password = "poop";
            var results = controller.Login(model,"Home"); 
            
            Assert.IsNotNull(results);
        }
    }
}


