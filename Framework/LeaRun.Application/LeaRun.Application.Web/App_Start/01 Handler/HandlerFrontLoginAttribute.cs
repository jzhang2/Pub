using System.Web.Mvc;
using LeaRun.Application.Code;
using LeaRun.Util;
using System.Web;
using System.Web.Security;
using LeaRun.Util.WebControl;

namespace LeaRun.Application.Web {
    /// <summary>
    /// 版 本 6.1
    /// Copyright (c) 2013-2016 上海力软信息技术有限公司
    /// 创建人：佘赐雄
    /// 日 期：2015.11.9 10:45
    /// 描 述：登录认证（会话验证组件）
    /// </summary>
    public class HandlerFrontLoginAttribute : AuthorizeAttribute {
        private LoginMode _customMode;
        private LoginType _loginType;
        /// <summary>默认构造</summary>
        /// <param name="Mode">认证模式</param>
        public HandlerFrontLoginAttribute(LoginMode Mode, LoginType Type = 0) {
            _customMode = Mode;
            _loginType = Type;
        }
        /// <summary>
        /// 响应前执行登录验证,查看当前用户是否有效 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext) {
            //登录拦截是否忽略
            if (_customMode == LoginMode.Ignore) {
                return;
            }
            var isAjax = filterContext.HttpContext.Request.IsAjaxRequest();
            //登录是否过期
            if (OperatorProvider.Provider.IsOverdue()) {
                WebHelper.WriteCookie("learun_login_error", "Overdue");//登录已超时,请重新登录
                if (_loginType == LoginType.FrontEnd) {
                    filterContext.Result = new ContentResult { Content = new AjaxResult { type = ResultType.warning, message = "请先登录", resultdata = "/SignIn" }.ToJson() };
                }
                else {
                    filterContext.Result = new RedirectResult("~/Login/Default");
                }
                return;
            }
            //是否已登录
            var OnLine = OperatorProvider.Provider.IsOnLine();
            if (OnLine == 0) {
                WebHelper.WriteCookie("learun_login_error", "OnLine");//您的帐号已在其它地方登录,请重新登录
                if (_loginType == LoginType.FrontEnd) {
                    filterContext.Result = new ContentResult { Content = new AjaxResult { type = ResultType.warning, message = "请先登录", resultdata = "/SignIn" }.ToJson() };
                }
                else {
                    filterContext.Result = new RedirectResult("~/Login/Default");
                }
                return;
            }
            else if (OnLine == -1) {
                WebHelper.WriteCookie("learun_login_error", "-1");//缓存已超时,请重新登录
                //filterContext.Result = new RedirectResult("~/Login/Default");
                return;
            }
        }
    }

}