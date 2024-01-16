using FoodieSydney.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Website.ActionResults;
using Umbraco.Cms.Infrastructure.Serialization;
using FoodieSydney.Model;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Web.Common;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace FoodieSydney.Controller
{
    public class PageController : SurfaceController
    {
        public IContentService contentService;

        public PageController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, 
                                ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) 
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)

        {

        }

        public static List<Comment> GetCommentList(IPublishedContent Content)
        {
            
            var CommentFolder = Content.Children.Where(x => x.ContentType.Alias == "commentFolder").FirstOrDefault();
            var List = new List<Comment>();

            if (CommentFolder != null)
            {
                var CommentList = CommentFolder.Children.Where(x => x.ContentType.Alias == "comment");

                if (CommentList.Any())
                {
                    foreach (var comment in CommentList)
                    {
                        List.Add(GenerateCommentModel(comment));
                    }
                    return List;
                }
            }

            return List;
        }

        public static Comment GenerateCommentModel(IPublishedContent Content)
        {
            var Comment = new Comment
            {
                CommentorName = Content.Value<string>("CommentorName"),
                CommentContent = Content.Value<string>("CommentContent"),
                Content = Content,
                ContentId = Content.Id,
                CommentName = Content.Name,
                ParentContentId = Content.Parent.Id,
                ParentContentGuidId = Content.Parent.Key
            };

            return Comment;
        }

        public static Match GetCommentName(string input)
        {
            Regex re = new Regex(@"([a-zA-Z]+)(\d+)");
            Match result = re.Match(input);

            string alphaPart = result.Groups[1].Value;
            string numberPart = result.Groups[2].Value;

            return result;
        }

        //public static void CraeteNewComment(IPublishedContent Content, string commentorName, string commentContent)
        //{
        //    if (Content.ContentType.Alias == "commentFolder")
        //    {
        //        var lastCommentName = GetCommentName(Content.Children.LastOrDefault().Name);

        //        var newCommentNae = lastCommentName.Groups[1].Value + ((Convert.ToInt32(lastCommentName.Groups[2].Value)) + 1).ToString();

        //        var contentItem = ContentService.Create(newCommentNae, Content.Id, "comment");

        //        if (contentItem != null)
        //        {
        //            contentItem.SetValue("commentorName", commentorName);
        //            contentItem.SetValue("commentContent", commentContent);

        //            Current.Services.ContentService.SaveAndPublish(contentItem);
        //        }
        //    }
        //}


        public static void test(IHttpContextAccessor httpContextAccessor)
        {
            UmbracoHelper helper;

            
            //helper.S

            //helper = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<UmbracoHelper>();
        }

        public void CraeteNewCommentTest(Comment comment)
        {
            if (comment != null)
            {

                var contentItem = this.Services.ContentService.Create(comment.CommentName, comment.ContentId, "comment");
                ////var contentItem = ContentService.Create(comment.CommentName, comment.ContentId, "comment");

                if (contentItem != null)
                {
                    contentItem.SetValue("commentorName", comment.CommentorName);
                    contentItem.SetValue("commentContent", comment.CommentContent);

                    this.Services.ContentService.SaveAndPublish(contentItem);
                }
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewComment(Comment comment)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            CraeteNewCommentTest(comment);
            return RedirectToUmbracoPage(comment.ParentContentGuidId);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewCommentAsync(Comment comment)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            CraeteNewCommentTest(comment);
            return RedirectToUmbracoPage(comment.ParentContentGuidId);

        }

        //[HttpPost]
        //public JsonNetResult NewCommentTest(Comment comment)
        //{
        //    var result = new JsonNetResult
        //    {
        //        Data = "Test"
        //    };
        //    if (ModelState.IsValid)
        //    {
        //        CraeteNewCommentTest(comment);

        //        result.Data = "Success";
        //        return result;
        //    }

        //    //var node = Umbraco.Content(comment.ParentContentId);

        //    return result;
        //}
    }
}
