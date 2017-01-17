using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaRun.Application.Busines.AuthorizeManage;
using LeaRun.Application.Busines.BaseManage;
using LeaRun.Application.Busines.CustomerManage;
using LeaRun.Application.Busines.ExtendManage;
using LeaRun.Application.Busines.PublicInfoManage;
using LeaRun.Application.Busines.SystemManage;
using LeaRun.Application.Cache;
using LeaRun.Application.Code;
using LeaRun.Application.Entity.BaseManage;
using LeaRun.Application.Entity.CustomerManage;
using LeaRun.Application.Entity.ExtendManage;
using LeaRun.Application.Entity.PublicInfoManage;
using LeaRun.Application.Entity.SystemManage;
using LeaRun.Application.Entity.SystemManage.ViewModel;
using LeaRun.Application.Service.ExtendManage;
using LeaRun.Util;
using LeaRun.Util.Attributes;
using LeaRun.Util.Extension;
using LeaRun.Util.WebControl;

namespace LeaRun.Application.Web.Controllers {
    public class HomeViewModel {
        public List<NewsEntity> NewsList { get; set; }
        public List<NewsEntity> MapNewsList { get; set; }
        public List<NewsEntity> PBooksList { get; set; }
        public List<BannerNewsEntity> BannerNewsList { get; set; }
        public List<DataItemModel> DataItemList { get; set; }
        public List<NewsEntity> AboutUs { get; set; }
        public NewsEntity CurrentArticle { get; set; }
    }
    [HandlerLogin(LoginMode.Ignore)]
    public class DefaultController : MvcControllerBase {
        private CustomerBLL customerbll = new CustomerBLL();
        private NewsBLL newsBll = new NewsBLL();
        private BannerNewsBLL bannerNewsBll = new BannerNewsBLL();
        private SecurityCodeService securityCodeService = new SecurityCodeService();
        private DataItemCache dataItemCache = new DataItemCache();


        public ActionResult Index() {
            var viewModel = new HomeViewModel();
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 4;
            pagination.sidx = "CreateDate";
            pagination.sord = "DESC";
            viewModel.NewsList = newsBll.GetPageList(pagination, "{ TypeId:3,EnabledMark:1 }").ToList();

            pagination.rows = 6;
            viewModel.MapNewsList = newsBll.GetPageList(pagination, "{ TypeId:4,EnabledMark:1 }").ToList();

            pagination.rows = 10000;
            viewModel.PBooksList = newsBll.GetPageList(pagination, "{ TypeId:6,EnabledMark:1,IsRecommend:1 }").ToList();

            viewModel.BannerNewsList = bannerNewsBll.GetPageList(pagination, "{Type:1}").ToList();
            return View(viewModel);
        }

        public ActionResult SignUp() {
            return View();
        }
        public ActionResult SignIn() {
            return View();
        }
        public ActionResult News() {
            var viewModel = new HomeViewModel();
            viewModel.DataItemList = dataItemCache.GetDataItemList("MapNews").ToList();
            return View(viewModel);
        }
        public ActionResult MapCulture() {
            var viewModel = new HomeViewModel();
            viewModel.DataItemList = dataItemCache.GetDataItemList("MapKn").ToList();
            return View(viewModel);
        }
        public ActionResult Service(string id) {
            var viewModel = new HomeViewModel();
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.sidx = "CreateDate";
            pagination.sord = "DESC";
            pagination.rows = 10000;
            viewModel.NewsList = newsBll.GetPageList(pagination, "{ TypeId:7,EnabledMark:1 }").ToList();

            if (!string.IsNullOrEmpty(id)) {
                viewModel.CurrentArticle = newsBll.GetEntity(id);
            }
            else if (viewModel.NewsList.Count > 0) {
                viewModel.CurrentArticle = viewModel.NewsList.First();
            }
            return View(viewModel);
        }

        public ActionResult AboutUs(string id) {
            var viewModel = new HomeViewModel();
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.sidx = "CreateDate";
            pagination.sord = "DESC";
            pagination.rows = 10000;
            viewModel.AboutUs = newsBll.GetPageList(pagination, "{ TypeId:8,EnabledMark:1 }").ToList();

            if (!string.IsNullOrEmpty(id)) {
                viewModel.CurrentArticle = newsBll.GetEntity(id);
            }
            else if (viewModel.AboutUs.Count > 0) {
                viewModel.CurrentArticle = viewModel.AboutUs.First();
            }
            return View(viewModel);
        }
        public ActionResult Detail(string id) {
            var viewModel = new HomeViewModel();
            if (!string.IsNullOrEmpty(id)) {
                viewModel.CurrentArticle = newsBll.GetEntity(id);
            }
            return View(viewModel);
        }

        public ActionResult Books() {
            return View();
        }
        public ActionResult _Footer() {
            var viewModel = new HomeViewModel();
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.sidx = "CreateDate";
            pagination.sord = "DESC";
            pagination.rows = 10000;
            viewModel.BannerNewsList = bannerNewsBll.GetPageList(pagination, "{Type:0}").ToList();
            viewModel.AboutUs = newsBll.GetPageList(pagination, "{ TypeId:8,EnabledMark:1 }").ToList();
            return PartialView(viewModel);
        }
        [HttpGet]
        public ActionResult GetNewsJson(Pagination pagination, string queryJson) {
            var watch = CommonHelper.TimerStart();
            var data = newsBll.GetPageList(pagination, queryJson).ToList();
            foreach (NewsEntity entity in data) {
                entity.NewsContent = WebHelper.GetText(entity.NewsContent).Length > 60
                    ? WebHelper.GetText(entity.NewsContent).Substring(0, 60)
                    : WebHelper.GetText(entity.NewsContent);
            }
            var JsonData = new {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
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
            try {

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
                operators.HeadIcon = userEntity.HeadIcon;
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
            catch (Exception) {

                return Error("注册失败。");
            }
        }
        [HttpPost]
        [AjaxOnly]
        public ActionResult CheckLogin(string username, string password, string verifycode, int autologin) {
            LogEntity logEntity = new LogEntity();
            logEntity.CategoryId = 1;
            logEntity.OperateTypeId = ((int)OperationType.Login).ToString();
            logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Login);
            logEntity.OperateAccount = username;
            logEntity.OperateUserId = username;
            logEntity.Module = Config.GetValue("SoftName");

            try {
                //#region 验证码验证
                //if (autologin == 0) {
                //    verifycode = Md5Helper.MD5(verifycode.ToLower(), 16);
                //    if (Session["session_verifycode"].IsEmpty() || verifycode != Session["session_verifycode"].ToString()) {
                //        throw new Exception("验证码错误，请重新输入");
                //    }
                //}
                //#endregion

                #region 第三方账户验证 modify by chengzg 关闭该验证
                //AccountEntity accountEntity = accountBLL.CheckLogin(username, password);
                //if (accountEntity != null)
                //{
                //    Operator operators = new Operator();
                //    operators.UserId = accountEntity.AccountId;
                //    operators.Code = accountEntity.MobileCode;
                //    operators.Account = accountEntity.MobileCode;
                //    operators.UserName = accountEntity.FullName;
                //    operators.Password = accountEntity.Password;
                //    operators.IPAddress = Net.Ip;
                //    operators.IPAddressName = IPLocation.GetLocation(Net.Ip);
                //    operators.LogTime = DateTime.Now;
                //    operators.Token = DESEncrypt.Encrypt(Guid.NewGuid().ToString());
                //    operators.IsSystem = true;
                //    OperatorProvider.Provider.AddCurrent(operators);
                //    //登录限制
                //    LoginLimit(username, operators.IPAddress, operators.IPAddressName);
                //    return Success("登录成功。");
                //}
                #endregion

                #region 内部账户验证
                CustomerEntity userEntity = new CustomerBLL().CheckLogin(username, password);
                if (userEntity != null) {
                    Operator operators = new Operator();
                    operators.UserId = userEntity.CustomerId;
                    operators.Code = userEntity.EnCode;
                    operators.HeadIcon = userEntity.HeadIcon;
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
                }
                return Success("登录成功。");
                #endregion
            }
            catch (Exception ex) {
                WebHelper.RemoveCookie("learn_autologin");                  //清除自动登录
                logEntity.ExecuteResult = -1;
                logEntity.ExecuteResultJson = ex.Message;
                logEntity.WriteLog();
                return Error(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult GetSecurityCode(string mobileCode) {
            if (!ValidateUtil.IsEmail(mobileCode)) {
                throw new Exception("邮箱地址格式不正确,请输入正确格式的邮箱地址。");
            }
            var exists = customerbll.ExistEmail(mobileCode, "");
            if (!exists) {
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
