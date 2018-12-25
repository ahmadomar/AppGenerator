using AppsGenerator.Classes;
using AppsGenerator.Classes.Utilities;
using AppsGenerator.Classes.Utilities.Messages.Notification;
using AppsGenerator.Models;
using AppsGenerator.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace AppsGenerator.Controllers
{
    public class MemberController : Controller
    {
        private MemberRepository rpMember = new MemberRepository();

        [AllowAnonymous]
        [Route("signup")]
        public ActionResult Signup()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("signup")]
        public ActionResult Signup(SignupViewModel memberData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool valid = ValidateSignup(memberData);
                    if (!valid)
                        return View();

                    Member member = new Member();
                    member.username = memberData.UserName;
                    member.email = memberData.Email;
                    member.created_at = DateTime.Now;
                    string hashedPass = Security.GenerateMd5(memberData.Password);
                    member.password = hashedPass;
                    member.is_active = false;
                    member.confirm_token = Security.Encrypt("OMAR*" + member.username + "*" + member.created_at + "*" + memberData.Password, true);

                    rpMember.Insert(member);

                    //Send verification link to email
                    string activationUrl = Url.Action("ConfirmSignup","Member", new { token = member.confirm_token }, "http");

                    NotifyMember.AccountActivation(member.email, activationUrl);
                    return View("Success", new MessageView()
                    {
                        Message = "We sent a link to " + member.email + " to activate your account."
                    });

                }
                else
                {
                    ModelState.AddModelError("", "There is an error in the input data, try again.");
                }
            }
            catch (Exception e)
            {
            }
            return View();
        }

        private bool ValidateSignup(SignupViewModel memberData)
        {
            if (Globals.IsReservedUsername(memberData.UserName))
            {
                ModelState.AddModelError("RegistrationError", "Username already exists");
                return false;
            }

            bool emailExists = IsExists(mm => mm.email == memberData.Email);
            bool usernameExists = IsExists(mm => mm.username == memberData.UserName);
            if (emailExists && usernameExists)
            {
                ModelState.AddModelError("RegistrationError", "The username and the email is already registered");
                return false;
            }
            else if (usernameExists)
            {
                ModelState.AddModelError("RegistrationError", "Username already exists");
                return false;
            }
            else if (emailExists)
            {
                ModelState.AddModelError("RegistrationError", "Email already exists");
                return false;
            }
            return true;
        }

        public ActionResult ConfirmSignup(String token)
        {
            if (String.IsNullOrEmpty(token))
                return RedirectToAction("Index", "Home");
            else
            {
                Member _member = rpMember.FindFirst(m => m.confirm_token == token && m.is_active == false);
                if (_member != null)
                {
                    string decryptedText = Security.Decrypt(token, true);

                    string password = decryptedText.Split('*')[3];

                    if (!string.IsNullOrEmpty(password))
                    {

                        string public_id = Security.GenerateMd5(_member.id+"_"+_member.username);
                        _member.is_active = true;
                        _member.confirm_token = null;
                        _member.public_id = public_id;
                        rpMember.Edit(_member);

                        NotifyMember.LoginInformation(_member.email, _member.username, password);

                        Directory.CreateDirectory(Server.MapPath("~/App_Data") + "\\" + public_id);
                        return View("Success", new MessageView()
                        {
                            Message = "Your account has been activated successfully"
                        });
                    }
                    else
                    {
                        return View("Success", new MessageView()
                        {
                            Message = "Please try this link later"
                        });
                    }

                }
            }
            return RedirectToAction("Index", "Home");
        }

        [Route("Login")]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("MyApps","Application");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult Login(LoginViewModel memberData, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string hashedPass = Security.GenerateMd5(memberData.Password);
                if (IsValid(memberData.UserName, hashedPass))
                {
                    FormsAuthentication.SetAuthCookie(memberData.UserName, memberData.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("IncorrectUserPass", "Username or password is incorrect!");
                }
            }
            return View();
        }

        [Authorize]
        [Route("ChangePassword")]
        public ActionResult ChangePassword()
        {
            if (!Request.IsAuthenticated)
                ViewBag.ReturnUrl = Url.Action("ChangePassword", "Member");
            return View();
        }

        [Authorize]
        [HttpPost]
        [Route("ChangePassword")]
        public ActionResult ChangePassword(ManageUserViewModel manage)
        {
            ViewBag.ReturnUrl = Url.Action("Index", "Home");

            string username = User.Identity.Name;
            string oldPassword = Security.GenerateMd5(manage.OldPassword);
            bool isValid = IsValid(username, oldPassword);
            if (isValid)
            {
                Member _member = rpMember.FindFirst(mm => mm.username == username);
                string newPassword = Security.GenerateMd5(manage.NewPassword);
                _member.password = newPassword;
                rpMember.Edit(_member);

                NotifyMember.ChangePassword(_member.email);

                return View("Success", new MessageView()
                {
                    Message = "Your password has been updated successfully"
                });
            }
            else
                ModelState.AddModelError("ChangePassword", "The current password is incorrect, try again");

            // If we got this far, something failed, redisplay form
            return View();
        }

        private bool IsExists(Expression<Func<Member, bool>> predicate)
        {
            Member mem = rpMember.FindFirst(predicate);
            if (mem == null)
                return false;
            return true;
        }

        [Route("logout")]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private bool IsValid(string username, string password)
        {
            bool IsValid = false;

            var user = rpMember.FindFirst(m => m.username == username && m.password == password && m.is_active == true);
            if (user != null)
            {
                IsValid = true;
            }

            return IsValid;
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("ResetPassRequest")]
        [AllowAnonymous]
        public ActionResult ResetPasswordRequest()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("ResetPassRequest")]
        public ActionResult ResetPasswordRequest(ResetPasswordRequestViewModel resetPassword)
        {
            if (ModelState.IsValid)
            {
                string email = resetPassword.Email;
                Member _member = rpMember.FindFirst(m => m.email == email);
                if (_member != null)
                {
                    string today = DateTime.Today.ToString();
                    string resetPasswordString = _member.email + today;
                    string md5ResetPasswordHashed = Security.GenerateMd5("OMAR" + email.Length + resetPasswordString.Length + resetPasswordString);

                    //Save Reset Token to DB 
                    _member.reset_token = md5ResetPasswordHashed;
                    rpMember.Edit(_member);


                    string resetPasswordUrl = Url.Action("ResetPassword", "Member", new { email = email, token = md5ResetPasswordHashed }, "http");

                    NotifyMember.ResetPassword(email, resetPasswordUrl);

                    return View("Success", new MessageView()
                    {
                        Message = "An e-mail has been sent to " + email + " to recover the password."
                    });

                }
                else
                {
                    ModelState.AddModelError("EmailNotFound", "Email is not registered.");
                }
            }
            return View();
        }


       
        [AllowAnonymous]
        public ActionResult ResetPassword(string email, string token)
        {
            if (email != null && token != null)
            {
                Member _member = rpMember.FindFirst(m => m.email == email && m.reset_token == token);

                if (_member != null)
                {
                    string today = DateTime.Today.ToString();
                    string resetPasswordString = email + today;
                    string md5ResetPasswordHashed = Security.GenerateMd5("OMAR" + email.Length + resetPasswordString.Length + resetPasswordString);

                    if (token == md5ResetPasswordHashed)
                    {
                        ResetPasswordViewModel resetPasswordModel = new ResetPasswordViewModel();
                        resetPasswordModel.email = email;
                        resetPasswordModel.token = token;
                        return View(resetPasswordModel);
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                    return RedirectToAction("Index", "Home");

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel resetPassword)
        {
            if (ModelState.IsValid)
            {
                string email = resetPassword.email;
                string token = resetPassword.token;
                string password = resetPassword.Password;

                string today = DateTime.Today.ToString();
                string resetPasswordString = email + today;
                string md5ResetPasswordHashed = Security.GenerateMd5("OMAR" + email.Length + resetPasswordString.Length + resetPasswordString);

                if (token == md5ResetPasswordHashed)
                {
                    Member _member = rpMember.FindFirst(mm => mm.email == email);
                    password = Security.GenerateMd5(password);
                    _member.password = password;
                    _member.reset_token = null;
                    rpMember.Edit(_member);

                    NotifyMember.ChangePassword(email);

                    return View("Success", new MessageView()
                    {
                        Message = "The password was successfully changed."
                    });
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
