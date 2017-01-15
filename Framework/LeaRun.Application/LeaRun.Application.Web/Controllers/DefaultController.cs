using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaRun.Application.Busines.AuthorizeManage;
using LeaRun.Application.Busines.CustomerManage;
using LeaRun.Application.Busines.PublicInfoManage;
using LeaRun.Application.Code;
using LeaRun.Application.Entity.CustomerManage;
using LeaRun.Application.Entity.ExtendManage;
using LeaRun.Application.Entity.PublicInfoManage;
using LeaRun.Application.Service.ExtendManage;
using LeaRun.Util;
using LeaRun.Util.WebControl;

namespace LeaRun.Application.Web.Controllers {
    public class HomeViewModel
    {
        public List<NewsEntity> NewsList { get; set; }
        public List<NewsEntity> MapNewsList { get; set; }
        public List<NewsEntity> PBooksList { get; set; }
    }
    [HandlerLogin(LoginMode.Ignore)]
    public class DefaultController : MvcControllerBase {
        private CustomerBLL customerbll = new CustomerBLL();
        private NewsBLL newsBll = new NewsBLL();
        private SecurityCodeService securityCodeService = new SecurityCodeService();
        //
        // GET: /Default/

        public ActionResult Index() {
            var viewModel = new HomeViewModel();
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 4;
            pagination.sidx = "CreateDate";
            pagination.sord = "DESC";
            viewModel.NewsList = newsBll.GetPageList(pagination, "{ TypeId:3,EnabledMark:1 }").ToList();
            return View(viewModel);
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
        public ActionResult Register(string mobileCode, string securityCode, string password) {
            try
            {

            //var code = securityCodeService.GetSecurityCode(mobileCode);
            //if (code == null) {
            //    return Error("请先获取验证码。");
            //}
            //if (Convert.ToDateTime(code.CreateDate).AddMinutes(5) < DateTime.Now) {
            //    return Error("验证码已过期，请重新获取。");
            //}
            //if (code.SecurityCode != securityCode) {
            //    return Error("验证码错误。");
            //}
            CustomerEntity userEntity = new CustomerEntity();
                userEntity.Email = mobileCode;
                userEntity.Secretkey = Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower();
                userEntity.Password = Md5Helper.MD5(DESEncrypt.Encrypt(password.ToLower(), userEntity.Secretkey).ToLower(), 32).ToLower(); userEntity.EnabledMark = 1;
            customerbll.SaveForm("", userEntity);
                Operator operators = new Operator();
                operators.UserId = userEntity.CustomerId;
                operators.Code = userEntity.EnCode;
                operators.Account = userEntity.Email;
                operators.Password = userEntity.Password;
                operators.Secretkey = userEntity.Secretkey;
                operators.RealName = userEntity.FullName;
                operators.IPAddress = Net.Ip;
                operators.IPAddressName = IPLocation.GetLocation(Net.Ip);
                operators.LogTime = DateTime.Now;
                operators.Token = DESEncrypt.Encrypt(Guid.NewGuid().ToString());
                AuthorizeDataModel dataAuthorize = new AuthorizeDataModel();
                operators.DataAuthorize = dataAuthorize;
                OperatorProvider.Provider.AddCurrent(operators);
                return Success("注册成功。");

            }
            catch (Exception)
            {

                return Error("注册失败。");
            }
        }

        [HttpGet]
        public ActionResult GetSecurityCode(string mobileCode) {
            if (!ValidateUtil.IsEmail(mobileCode)) {
                throw new Exception("邮箱地址格式不正确,请输入正确格式的邮箱地址。");
            }
            var exists = customerbll.ExistEmail(mobileCode,"");
            if (!exists)
            {
                throw new Exception("邮箱地址已被注册。");
            }
            try {
                var entity = new SecurityCodeEntity();
                entity.Email = mobileCode;
                entity.CreateDate = DateTime.Now;
                entity.SecurityCode = CommonHelper.RndNum(4);
                securityCodeService.SaveForm("", entity);
                MailHelper.SendEmailByThread(mobileCode, "西安地图出版社注册验证码", "尊敬的用户您好：感谢您注册使用西安地图出版社，您的验证码为" + entity.SecurityCode + "，有效期10分钟。");
                return Success("验证码已发送到您的邮箱。");
            }
            catch (Exception ee) {
                return Error("验证码发送错误。");
            }

        }

    }
}
