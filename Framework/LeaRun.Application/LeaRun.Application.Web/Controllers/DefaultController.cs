using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using LeaRun.Application.Service.CustomerManage;
using LeaRun.Application.Service.ExtendManage;
using LeaRun.Data.Repository;
using LeaRun.Util;
using LeaRun.Util.Attributes;
using LeaRun.Util.Extension;
using LeaRun.Util.WebControl;

namespace LeaRun.Application.Web.Controllers {

    #region 

    public class NewsEntityThumbUp : NewsEntity {
        private ThumbUpService service = new ThumbUpService();
        private CommentsService commentsService = new CommentsService();

        public int ThumbUp
        {
            get
            {
                if (!string.IsNullOrEmpty(NewsId)) {
                    return service.GetCount(NewsId, "");
                }
                else {
                    return 0;
                }
            }
        }

        public bool IsThumbed
        {
            get
            {
                if (!string.IsNullOrEmpty(NewsId) && OperatorProvider.Provider.Current() != null &&
                    OperatorProvider.Provider.Current().UserId != null) {
                    return service.GetCount(NewsId, OperatorProvider.Provider.Current().UserId) > 0;
                }
                else {
                    return true;
                }
            }
        }

        public int CommentsCount
        {
            get
            {
                if (!string.IsNullOrEmpty(NewsId)) {
                    return commentsService.GetList("", NewsId).ToList().Count;
                }
                else {
                    return 0;
                }
            }
        }
    }

    public class HomeViewModel {
        public List<NewsEntity> NewsList = new List<NewsEntity>();
        public List<NewsEntityThumbUp> NewsEntityThumbUp = new List<NewsEntityThumbUp>();
        public List<NewsEntity> MapNewsList = new List<NewsEntity>();
        public List<NewsEntity> PBooksList = new List<NewsEntity>();
        public List<BannerNewsEntity> BannerNewsList = new List<BannerNewsEntity>();
        public List<DataItemModel> MapNav = new List<DataItemModel>();
        public List<DataItemModel> NewsNav = new List<DataItemModel>();
        public List<NewsEntity> AboutUs = new List<NewsEntity>();
        public NewsEntity CurrentArticle = new NewsEntity();
        public NewsEntityThumbUp Detial = new NewsEntityThumbUp();
        public CustomerEntity Customer = new CustomerEntity();
        public List<SuggestionEntity> MySuggestionList = new List<SuggestionEntity>();
        public List<ContributionEntity> ContributionList = new List<ContributionEntity>();
        public List<CustomizationEntity> CustomizationList = new List<CustomizationEntity>();
        public NewsEntity Ebook = new NewsEntity();
    }

    public class UserComments : CommentsEntity {
        public string FullName { get; set; }
        public string HeadIcon { get; set; }
        public string ReplyName { get; set; }
        public string ReplyEmail { get; set; }
    }

    public class SuggestionReply : SuggestionEntity {
        public string FullName { get; set; }
        public string HeadIcon { get; set; }
        public string Contents { get; set; }
        public DateTime? ReplyDate { get; set; }
        public string ReplyContents { get; set; }
    }

    #endregion

    [HandlerFrontLogin(LoginMode.Ignore, LoginType.FrontEnd)]
    public class DefaultController : MvcControllerBase {
        #region bll

        private CustomerBLL customerbll = new CustomerBLL();
        private NewsBLL newsBll = new NewsBLL();
        private BannerNewsBLL bannerNewsBll = new BannerNewsBLL();
        private SecurityCodeService securityCodeService = new SecurityCodeService();
        private DataItemCache dataItemCache = new DataItemCache();
        private SuggestionBLL suggestionBll = new SuggestionBLL();
        private ContributionBLL contributionBll = new ContributionBLL();
        private ThumbUpService service = new ThumbUpService();
        private CustomizationService customizationService = new CustomizationService();

        #endregion

        #region empty action

        public ActionResult SignUp() {
            return View();
        }

        public ActionResult Books() {
            return View();
        }

        public ActionResult ForgotPassWord() {
            return View();
        }

        public ActionResult SignIn() {
            return View();
        }

        public ActionResult Customization() {
            return View();
        }

        #endregion

        public ActionResult Index() {
            var viewModel = new HomeViewModel();
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 4;
            pagination.sidx = "CreateDate";
            pagination.sord = "DESC";
            var NewsList = SubStringList(newsBll.GetPageList(pagination, "{ TypeId:3,EnabledMark:1 }").ToList(), 57);
            NewsList.ForEach(
                x => viewModel.NewsEntityThumbUp.Add(CommonHelper.AutoCopy<NewsEntity, NewsEntityThumbUp>(x)));
            pagination.rows = 6;
            viewModel.MapNewsList = SubStringList(
                newsBll.GetPageList(pagination, "{ TypeId:4,EnabledMark:1 }").ToList(), 57);

            pagination.rows = 10000;
            viewModel.PBooksList =
                SubStringList(newsBll.GetPageList(pagination, "{ TypeId:6,EnabledMark:1,IsRecommend:1 }").ToList(), 57);

            viewModel.BannerNewsList = bannerNewsBll.GetPageList(pagination, "{Type:1}").ToList();
            return View(viewModel);
        }

        public ActionResult News(string Category) {
            var viewModel = new HomeViewModel();
            viewModel.NewsNav = dataItemCache.GetDataItemList("MapNews").OrderBy(t => t.SortCode).ToList();
            return View(viewModel);
        }
        public ActionResult MapCulture(string Category) {
            var viewModel = new HomeViewModel();
            viewModel.MapNav = dataItemCache.GetDataItemList("MapKn").OrderBy(t => t.SortCode).ToList();
            return View(viewModel);
        }
        public ActionResult Service(string id) {
            var viewModel = new HomeViewModel();
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.sidx = "CreateDate";
            pagination.sord = "DESC";
            pagination.rows = 10000;
            viewModel.NewsList = newsBll.GetPageList(pagination, "{ TypeId:7,EnabledMark:1 }").OrderBy(t => t.SortCode).ToList();

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
            viewModel.AboutUs = newsBll.GetPageList(pagination, "{ TypeId:8,EnabledMark:1 }").OrderBy(t => t.SortCode).ToList();

            if (!string.IsNullOrEmpty(id)) {
                viewModel.CurrentArticle = newsBll.GetEntity(id);
            }
            else if (viewModel.AboutUs.Count > 0) {
                viewModel.CurrentArticle = viewModel.AboutUs.First();
            }
            return View(viewModel);
        }

        public ActionResult PBook(string id) {
            var viewModel = new HomeViewModel();
            if (!string.IsNullOrEmpty(id)) {
                viewModel.CurrentArticle = newsBll.GetEntity(id);
                viewModel.CurrentArticle.PV = viewModel.CurrentArticle.PV + 1;
                newsBll.SaveForm(viewModel.CurrentArticle.NewsId, viewModel.CurrentArticle);
            }
            return View(viewModel);
        }
        public ActionResult EBook(string id) {
            var viewModel = new HomeViewModel();
            if (!string.IsNullOrEmpty(id)) {
                viewModel.Ebook = newsBll.GetEntity(id);
            }
            return View(viewModel);
        }
        public ActionResult Detail(string id) {
            var viewModel = new HomeViewModel();
            if (!string.IsNullOrEmpty(id)) {
                viewModel.Detial = CommonHelper.AutoCopy<NewsEntity, NewsEntityThumbUp>(newsBll.GetEntity(id));
            }
            return View(viewModel);
        }

        #region comments

        public ActionResult GetCommentsJson(string NewsId) {
            var treeList = new List<TreeGridEntity>();
            if (!string.IsNullOrEmpty(NewsId)) {
                var data =
                    new Repository<UserComments>(DbFactory.Base()).FindList(
                        "select co.*,c.[FullName],c.[HeadIcon],c1.[FullName] as ReplyName,c.Email,c1.Email as ReplyEmail from [dbo].[Extend_Comments] co left join [dbo].[Client_Customer] c on c.CustomerId=co.CustomerId left join [dbo].[Client_Customer] c1 on c1.CustomerId=co.ReplyCustomerId where co.NewsId='" +
                        NewsId + "' order by co.CreateDate");
                foreach (UserComments item in data) {
                    TreeGridEntity tree = new TreeGridEntity();
                    bool hasChildren = data.Count(t => t.ParentId == item.CommentsId) != 0;
                    tree.id = item.CommentsId;
                    tree.hasChildren = hasChildren;
                    tree.parentId = item.ParentId;
                    tree.expanded = true;
                    tree.entityJson = item.ToJson();
                    treeList.Add(tree);
                }
            }
            return Content(treeList.TreeJson());
        }


        [HttpGet]
        public ActionResult ReplyComments(string CommentsId, string Reply, string ReplyCustomerId, string NewsId) {
            if (OperatorProvider.Provider.Current() == null || OperatorProvider.Provider.Current().UserId == null) {
                return Error("您还未登录");
            }
            try {
                var entity = new CommentsEntity();
                entity.ParentId = CommentsId;
                entity.Comments = Reply;
                entity.CustomerId = OperatorProvider.Provider.Current().UserId;
                entity.ReplyCustomerId = ReplyCustomerId;
                entity.NewsId = NewsId;

                new CommentsService().SaveFormData("", entity);
                var data =
                    new Repository<UserComments>(DbFactory.Base()).FindList(
                        "select co.*,c.[FullName],c.[HeadIcon],c1.[FullName] as ReplyName,c.Email,c1.Email as ReplyEmail from [dbo].[Extend_Comments] co left join [dbo].[Client_Customer] c on c.CustomerId=co.CustomerId left join [dbo].[Client_Customer] c1 on c1.CustomerId=co.ReplyCustomerId where co.CommentsId='" +
                        entity.CommentsId + "' order by co.CreateDate");
                return Content(data.FirstOrDefault().ToJson());
            }
            catch (Exception ee) {
                return Error("评论失败");
            }

        }

        [HttpGet]
        public ActionResult AddComments(string Reply, string NewsId) {
            if (OperatorProvider.Provider.Current() == null || OperatorProvider.Provider.Current().UserId == null) {
                return Error("您还未登录");
            }
            try {
                var entity = new CommentsEntity();
                entity.Comments = Reply;
                entity.CustomerId = OperatorProvider.Provider.Current().UserId;
                entity.NewsId = NewsId;
                entity.ParentId = "0";
                new CommentsService().SaveFormData("", entity);
                var data =
                    new Repository<UserComments>(DbFactory.Base()).FindList(
                        "select co.*,c.[FullName],c.[HeadIcon],c1.[FullName] as ReplyName,c.Email,c1.Email as ReplyEmail from [dbo].[Extend_Comments] co left join [dbo].[Client_Customer] c on c.CustomerId=co.CustomerId left join [dbo].[Client_Customer] c1 on c1.CustomerId=co.ReplyCustomerId where co.CommentsId='" +
                        entity.CommentsId + "' order by co.CreateDate");
                return Content(data.FirstOrDefault().ToJson());
            }
            catch (Exception ee) {
                return Error("回复失败");
            }

        }

        #endregion

        public ActionResult GetSuggestionJson() {
            var data = new Repository<SuggestionReply>(DbFactory.Base()).FindList("select co.*,c.[FullName],c.[HeadIcon],c.Email,c1.Contents as ReplyContents,c1.CreateDate as ReplyDate from [dbo].[Extend_Suggestion] co left join [dbo].[Client_Customer] c on c.CustomerId=co.CreateUserId left join [dbo].[Extend_SuggestionAnswer] c1 on c1.SuggestionId=co.SuggestionId order by co.CreateDate");
            return Content(data.ToJson());
        }


        public ActionResult Contributor() {
            var viewModel = new HomeViewModel();
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.sidx = "CreateDate";
            pagination.sord = "DESC";
            pagination.rows = 10000;
            viewModel.NewsList = newsBll.GetPageList(pagination, "{ TypeId:7,EnabledMark:1 }").OrderBy(t => t.SortCode).ToList();
            if (OperatorProvider.Provider.Current() != null && OperatorProvider.Provider.Current().UserId != null) {
                viewModel.ContributionList = contributionBll.GetUserContribution(OperatorProvider.Provider.Current().UserId).ToList();
            }
            return View(viewModel);
        }
        public ActionResult Reader() {
            var viewModel = new HomeViewModel();
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.sidx = "CreateDate";
            pagination.sord = "DESC";
            pagination.rows = 10000;
            viewModel.NewsList = newsBll.GetPageList(pagination, "{ TypeId:7,EnabledMark:1 }").OrderBy(t => t.SortCode).ToList();
            if (OperatorProvider.Provider.Current() != null && OperatorProvider.Provider.Current().UserId != null) {
                viewModel.MySuggestionList = suggestionBll.GetUserSuggestion(OperatorProvider.Provider.Current().UserId).ToList();
            }
            return View(viewModel);
        }

        #region method

        [AjaxOnly]
        [ValidateInput(false)]
        public ActionResult GetEBookPage(string id, string page) {
            try {
                if (!string.IsNullOrEmpty(id)) {
                    var ebook = newsBll.GetEntity(id);
                    string url = string.Format("http://" + System.Web.HttpContext.Current.Request.Url.Authority + ebook.EPath + page + ".html");
                    var req = (HttpWebRequest)WebRequest.Create(url);
                    req.Method = "GET";
                    req.ContentType = "application/x-www-form-urlencoded";
                    HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    var result = reader.ReadToEnd();
                    result = result.Replace("href=\"", "href=\"" + ebook.EPath)
                        .Replace("src=\"", "src=\"" + ebook.EPath);
                    return Content(result);
                }
                return Content("获取书籍内容错误。");
            }
            catch (Exception) {
                return Content("获取书籍内容错误。");
            }
        }

        public List<NewsEntity> SubStringList(List<NewsEntity> data, int length) {
            foreach (NewsEntity entity in data) {
                if (entity.NewsContent == null) continue;
                var content =
                    WebHelper.StripTagsCharArray(System.Web.HttpContext.Current.Server.HtmlDecode(entity.NewsContent));
                entity.NewsContent = content.Length > length
                    ? content.Substring(0, length) + "..."
                    : content;
            }
            return data;
        }

        [HttpGet]
        public ActionResult GetNewsJson(Pagination pagination, string queryJson) {
            var watch = CommonHelper.TimerStart();
            var data = newsBll.GetPageList(pagination, queryJson).ToList();
            foreach (NewsEntity entity in data) {
                if (entity.NewsContent == null) continue;
                var content =
                    WebHelper.StripTagsCharArray(System.Web.HttpContext.Current.Server.HtmlDecode(entity.NewsContent));
                entity.NewsContent = content.Length > 60
                    ? content.Substring(0, 60)
                    : content;
            }
            var thumb = new List<NewsEntityThumbUp>();
            data.ForEach(x => thumb.Add(CommonHelper.AutoCopy<NewsEntity, NewsEntityThumbUp>(x)));
            var JsonData = new {
                rows = thumb,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }

        #endregion


        [HandlerFrontLogin(LoginMode.Enforce, LoginType.FrontEnd)]
        public ActionResult MyAccount() {
            var viewModel = new HomeViewModel();
            viewModel.Customer = customerbll.GetEntity(OperatorProvider.Provider.Current().UserId);
            viewModel.MySuggestionList = suggestionBll.GetUserSuggestion(OperatorProvider.Provider.Current().UserId).ToList();
            viewModel.ContributionList = contributionBll.GetUserContribution(OperatorProvider.Provider.Current().UserId).ToList();
            viewModel.CustomizationList = customizationService.GetUserCustomization(OperatorProvider.Provider.Current().UserId).ToList();
            return View(viewModel);
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
        public ActionResult _Nav() {
            var viewModel = new HomeViewModel();
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.sidx = "CreateDate";
            pagination.sord = "DESC";
            pagination.rows = 10000;
            viewModel.BannerNewsList = bannerNewsBll.GetPageList(pagination, "{Type:0}").ToList();
            viewModel.AboutUs = newsBll.GetPageList(pagination, "{ TypeId:8,EnabledMark:1 }").OrderBy(t => t.SortCode).ToList();
            viewModel.NewsList = newsBll.GetPageList(pagination, "{ TypeId:7,EnabledMark:1 }").OrderBy(t => t.SortCode).ToList();
            viewModel.NewsNav = dataItemCache.GetDataItemList("MapNews").OrderBy(t => t.SortCode).ToList();
            viewModel.MapNav = dataItemCache.GetDataItemList("MapKn").OrderBy(t => t.SortCode).ToList();
            return PartialView(viewModel);
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

                var code = securityCodeService.GetSecurityCode(mobileCode);
                if (code == null) {
                    return Error("请先获取验证码。");
                }
                if (Convert.ToDateTime(code.CreateDate).AddMinutes(10) < DateTime.Now) {
                    return Error("验证码已过期，请重新获取。");
                }
                if (code.SecurityCode != securityCode) {
                    return Error("验证码错误。");
                }
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

        public ActionResult ResetPwd(string code) {
            var entity = securityCodeService.GetCode(code);
            if (entity == null) {
                ViewBag.Message = "非法请求。";
            }
            if (Convert.ToDateTime(entity.CreateDate).AddMinutes(10) < DateTime.Now) {
                ViewBag.Message = "重置密码邮件已过期，请<a href=\"/ForgotPassWord\">重新发送邮件</a>。";
            }
            return View();
        }

        public ActionResult ThumbUp(string NewsId) {
            if (service.GetCount(NewsId, OperatorProvider.Provider.Current().UserId) > 0) {
                return Error("您已经点过赞了。");
            }
            var entity = new ThumbUpEntity {
                CustomerId = OperatorProvider.Provider.Current().UserId,
                NewsId = NewsId
            };
            service.SaveForm("", entity);
            return Success("点赞成功。");
        }

        public ActionResult UpdatePwd(string securityCode, string password) {
            try {

                var code = securityCodeService.GetCode(securityCode);
                if (code == null) {
                    return Error("非法请求。");
                }
                if (Convert.ToDateTime(code.CreateDate).AddMinutes(10) < DateTime.Now) {
                    return Error("重置邮件已过期，请<a href=\"/ForgotPassWord\">重新发送邮件</a>。");
                }

                CustomerEntity userEntity = new CustomerService().CheckLogin(code.Email);
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
                return Success("重置密码成功。");

            }
            catch (Exception) {

                return Error("重置密码失败。");
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

        [HttpGet]
        public ActionResult SendPwd(string mobileCode) {
            if (!ValidateUtil.IsEmail(mobileCode)) {
                throw new Exception("邮箱地址格式不正确,请输入正确格式的邮箱地址。");
            }
            var exists = customerbll.ExistEmail(mobileCode, "");
            if (exists) {
                throw new Exception("邮箱地址不存在。");
            }
            try {
                var entity = new SecurityCodeEntity();
                entity.Email = mobileCode;
                entity.CreateDate = DateTime.Now;
                entity.SecurityCode = Md5Helper.MD5(CommonHelper.RndNum(4) + mobileCode, 16); ;
                securityCodeService.SaveForm("", entity);
                MailHelper.SendEmailByThread(mobileCode, "西安地图出版社重置密码", "尊敬的用户您好：感谢您使用西安地图出版社，请点击链接<a href=\"http://" + System.Web.HttpContext.Current.Request.Url.Authority + "/ResetPwd?code=" + entity.SecurityCode + "\" target=\"_blank\">http://" + System.Web.HttpContext.Current.Request.Url.Authority + "/ResetPwd?code=" + entity.SecurityCode + "</a>重置密码，有效期10分钟。");
                return Success("重置密码邮件已发送到您的邮箱。");
            }
            catch (Exception ee) {
                return Error("邮件发送错误。");
            }

        }

    }
}
