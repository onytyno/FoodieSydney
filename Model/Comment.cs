using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace FoodieSydney.Model
{
    public class Comment : IValidatableObject
    {
        public string CommentName { get; set; }

        [Required(ErrorMessage = "Please Write your name")]
        [StringLength(30, ErrorMessage = "Unable to enter over 30 characters")]
        [Display(Name = "Your Name")]
        public string CommentorName { get; set; }

        [Required(ErrorMessage = "Please enter the comments")]
        [Display(Name = "Comment Content")]
        public string CommentContent { get; set; }

        public IPublishedContent Content { get; set; }

        public int ContentId { get; set; }

        public int ParentContentId { get; set; }

        public string GetNewCommentName()
        {
            return GetLastCommentName() + GetLastCommentNumber();
        }

        private string GetLastCommentName()
        {
            if (Content != null && Content.Children.Any() && Content.Children.LastOrDefault() != null)
            {
                return GetCommentName(Content.Children.LastOrDefault().Name).Groups[1].Value;
            }
            return "Comment";
        }

        private static Match GetCommentName(string input)
        {
            Regex re = new Regex(@"([a-zA-Z]+)(\d+)");
            Match result = re.Match(input);

            string alphaPart = result.Groups[1].Value;
            string numberPart = result.Groups[2].Value;

            return result;
        }

        private int GetLastCommentNumber()
        {
            if (Content != null)
            {
                var CommentNumber = Content.Children.Count();
                return CommentNumber + 1;
            }
            return 0;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            const string errorMsgStringNull = "Please Enter {0}.";

            if (string.IsNullOrWhiteSpace(CommentorName))
            {
                results.Add(new ValidationResult(string.Format(errorMsgStringNull, "Your Name"), new[] { "CommentorName" }));
            }
            if (string.IsNullOrWhiteSpace(CommentContent))
            {
                results.Add(new ValidationResult(string.Format(errorMsgStringNull, "Comments"), new[] { "CommentContent" }));
            }


            return results;
        }
    }

}
