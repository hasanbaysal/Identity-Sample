using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Runtime;

namespace IdentityTutorial.Web.Taghelpers
{
    public class ProfilePhotoTagHelper:TagHelper
    {
        public string? PictureName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";

            if (PictureName == null || PictureName=="")
            {

                output.Attributes.SetAttribute("src", "/userpictures/defaultpp.jpg");

            }
            else
            {
                output.Attributes.SetAttribute("src", $"/userpictures/{PictureName}");
            }

        }

    }
}
