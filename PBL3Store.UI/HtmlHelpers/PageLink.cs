using PBL3Store.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
namespace PBL3Store.UI.HtmlHelpers
{
    public static class PageLink
    {
        public static MvcHtmlString Paging(
            this HtmlHelper helper,
            PagingInfo pagingInfo,
            Func<int, string> pageUrl)
        {
            if(pagingInfo.TotalPage > 1)
            {
                StringBuilder sb = new StringBuilder();
                int start = Math.Max(pagingInfo.CurrentPage - 2, 1);
                int end = Math.Min(start + 4, pagingInfo.TotalPage);
                if (pagingInfo.CurrentPage != 1)
                {
                    TagBuilder tagA = new TagBuilder("a");
                    tagA.SetInnerText("<<");
                    tagA.Attributes.Add("href", pageUrl(pagingInfo.CurrentPage - 1));
                    tagA.AddCssClass("page-link");
                    TagBuilder tagLi = new TagBuilder("li");
                    tagLi.AddCssClass("page-item");
                    tagLi.InnerHtml = tagA.ToString();
                    sb.Append(tagLi);
                }
                for (int i = start; i <= end; i++)
                {
                    TagBuilder tagA = new TagBuilder("a");
                    tagA.SetInnerText(i.ToString());
                    tagA.Attributes.Add("href", pageUrl(i));
                    tagA.AddCssClass("page-link");

                    TagBuilder tagLi = new TagBuilder("li");
                    if (i == pagingInfo.CurrentPage)
                    {
                        tagLi.AddCssClass("page-item active");
                    }
                    else
                    {
                        tagLi.AddCssClass("page-item");
                    }
                    tagLi.InnerHtml = tagA.ToString();
                    sb.Append(tagLi);
                }
                if (pagingInfo.CurrentPage != pagingInfo.TotalPage)
                {
                    TagBuilder tagA = new TagBuilder("a");
                    tagA.SetInnerText(">>");
                    tagA.Attributes.Add("href", pageUrl(pagingInfo.CurrentPage + 1));
                    tagA.AddCssClass("page-link");
                    TagBuilder tagLi = new TagBuilder("li");
                    tagLi.AddCssClass("page-item");
                    tagLi.InnerHtml = tagA.ToString();
                    sb.Append(tagLi);
                }
                TagBuilder tabUl = new TagBuilder("ul");
                tabUl.InnerHtml = sb.ToString();
                tabUl.AddCssClass("pagination");
                TagBuilder tagNav = new TagBuilder("nav");
                tagNav.InnerHtml = tabUl.ToString();
                return MvcHtmlString.Create(tagNav.ToString());
            }
            else
            {
                TagBuilder taba = new TagBuilder("a");
                return MvcHtmlString.Create(taba.ToString());
            }
        }
    }
}