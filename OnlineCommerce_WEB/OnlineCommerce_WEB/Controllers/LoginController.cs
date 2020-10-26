﻿using OnlineCommerce_WEB.Models;
using OnlineCommerce_WEB.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineCommerce_WEB.Controllers
{
    public class LoginController : Controller
    {
        private OnlineCommerceEntities db = new OnlineCommerceEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginEntity entity)
        {
            var user = (from u in db.Accounts
                            where u.Username == entity.Username
                        select u).Any();

            if (user) // Kullanıcı adı varmı kontrolü
            {
                var pass = db.Accounts.Where(p => p.Username == entity.Username).FirstOrDefault<Accounts>();

                Encryption enc = new Encryption();

                if (pass.Password == entity.Password || enc.Decrypt(pass.Password) == entity.Password) // Veri tabanındaki şifrelenmiş şifre ile aynı mı kontrolü
                {
                    Response.Write("<script>alert('Giriş Başarılı')</script>");
                    entity.IsLogin = true;

                    // Giriş yapan kullanıcnın bilgilerini alıyoruz.
                    CurrentLoginEntity.IsLogin = true;
                    CurrentLoginEntity.Username = entity.Username;
                    CurrentLoginEntity.Password = entity.Password;

                    return Redirect("/Products");// Giriş yaptıktan sonra ürünler sayfasına yönlendiriyor

                }
                else
                {
                    Response.Write("<script>alert('Şifre Hatalı')</script>");
                    entity.IsLogin = false;
                }
            }
            else
            {
                Response.Write("<script>alert('Başka bir kullanıcı adı giriniz.')</script>"); // Hata Mesajı
                entity.IsLogin = false;
            }
            return View();
        }

        public RedirectResult Logout()
        {
            CurrentLoginEntity.IsLogin = false;
            return Redirect("/Home"); // Çıkış yaptıktan sonra ana sayfaya yönlendiriyor.
        }
    }
}
