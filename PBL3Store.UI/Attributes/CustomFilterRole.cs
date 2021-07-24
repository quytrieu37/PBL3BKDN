using PBL3Store.Domain;
using PBL3Store.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3Store.UI.Attributes
{
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple=true, Inherited =true)]
    public class CustomFilterRole : FilterAttribute, IAuthorizationFilter
    {
        public string RolesAllow { get; set; }
        private readonly IMainRepository _mainRepository;
        public CustomFilterRole()
        {
            _mainRepository = new MainRepository();
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            List<string> roles = new List<string>(); // lấy roles cho phép trên action tùy chọn
            if(RolesAllow.Contains(','))
            {
                roles = this.RolesAllow.Split(',').ToList();
            }
            else
            {
                roles.Add(RolesAllow);
            }
            string UserNameCurrent = HttpContext.Current.User.Identity.Name;
            User user = _mainRepository.Users.FirstOrDefault(x => x.UserName == UserNameCurrent);
            if(user!=null)
            {
                if (!roles.Any(x => x == user.Role.RoleName))
                {
                    ViewResult view = new ViewResult();
                    view.ViewName = "~/Views/Layout/Error403.cshtml"; // ko có quyền truy cập tài nguyên
                    filterContext.Result = view;
                }
            }
            else
            {
                ViewResult view1 = new ViewResult();
                view1.ViewName = "~/Views/Account/Login.cshtml";

                filterContext.Result = view1;
            }
        }
    }
}