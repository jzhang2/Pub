using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaRun.Application.Busines.CustomerManage;
using LeaRun.Application.Code;
using LeaRun.Application.Entity.CustomerManage;
using LeaRun.Application.Entity.ExtendManage;
using LeaRun.Application.Service.ExtendManage;
using LeaRun.Util;

namespace LeaRun.Application.Web.Controllers
{
    [HandlerLogin(LoginMode.Ignore)]
    public class DefaultController : MvcControllerBase {
        private CustomerBLL customerbll = new CustomerBLL();
        private SecurityCodeService securityCodeService = new SecurityCodeService();
        //
        // GET: /Default/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignUp() {
            return View();
        }
        public ActionResult SignIn() {
            return View();
        }

        /// <summary>
        /// 注册账户
        /// </summary>
        /// <param name="mobileCode">手机号</param>
        /// <param name="securityCode">短信验证码</param>
        /// <param name="fullName">姓名</param>
        /// <param name="password">密码（md5）</param>
        /// <param name="verifycode">图片验证码</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(string mobileCode, string securityCode, string password)
        {
            var code = securityCodeService.GetSecurityCode(mobileCode);
            if (code==null)
            {
                return Error("请先获取验证码。");
            }
            if (Convert.ToDateTime(code.CreateDate).AddMinutes(5)<DateTime.Now)
            {
                return Error("验证码已过期，请重新获取。");
            }
            if (code.SecurityCode!=securityCode)
            {
                return Error("验证码错误。");
            }
            CustomerEntity accountEntity = new CustomerEntity();
            accountEntity.Email = mobileCode;
            accountEntity.Secretkey = Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower();
            accountEntity.Password = Md5Helper.MD5(DESEncrypt.Encrypt(Md5Helper.MD5(accountEntity.Password, 32).ToLower(), accountEntity.Secretkey).ToLower(), 32).ToLower();
            accountEntity.EnabledMark = 1;
            customerbll.SaveForm("",accountEntity);
            return Success("注册成功。");
        }

        [HttpGet]
        public ActionResult GetSecurityCode(string mobileCode) {
            if (!ValidateUtil.IsEmail(mobileCode)) {
                throw new Exception("邮箱地址格式不正确,请输入正确格式的邮箱地址。");
            }
            try
            {
                var entity = new SecurityCodeEntity();
                entity.Email = mobileCode;
                entity.CreateDate = DateTime.Now;
                entity.SecurityCode = CommonHelper.RndNum(4);
                securityCodeService.SaveForm("", entity);
                var data = MailHelper.SendEmail(mobileCode,"西安地图出版社注册验证码","尊敬的用户您好：感谢您注册使用西安地图出版社，您的验证码为"+entity.SecurityCode+"，有效期10分钟。");
                if (data)
                {
                    return Success("验证码已发送到您的邮箱。");
                }
                else
                {
                    return Error("验证码发送错误。");
                }
            }
            catch (Exception ee)
            {
                return Error("验证码发送错误。");
            }
            
        }

    }
}
