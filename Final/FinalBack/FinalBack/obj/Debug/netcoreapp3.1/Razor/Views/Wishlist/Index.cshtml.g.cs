#pragma checksum "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Wishlist\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7215c436c2e9c5fb1ee350dc5bb23e62f93b47ef"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Wishlist_Index), @"mvc.1.0.view", @"/Views/Wishlist/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\_ViewImports.cshtml"
using FinalBack;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\_ViewImports.cshtml"
using FinalBack.ViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\_ViewImports.cshtml"
using FinalBack.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7215c436c2e9c5fb1ee350dc5bb23e62f93b47ef", @"/Views/Wishlist/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"215b113bcedaad543cefa7ec22695e2f56ae83ca", @"/Views/_ViewImports.cshtml")]
    public class Views_Wishlist_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<WishlistItem>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"    <!-- Main Start  -->
    <main>
        <section id=""custom-banner-section"">
            <div class=""page-header"">
                <p class=""page-header-text"">Wishlist</p>
                <span><a href=""index.html"">Home / </a>Wishlist</span>
            </div>
        </section>
        <section id=""wishlist-products-section"">
            <div class=""container"">
                <div class=""row"">
                    <div class=""col-12"">
                        <table class=""table table-bordered"">
                            <thead>
                                <tr>
                                    <th scope=""col"" class=""image-th"">Images</th>
                                    <th scope=""col"" class=""product-name-th"">Product</th>
                                    <th scope=""col"" class=""product-price-th"">Unit Price</th>
                                    <th scope=""col"" class=""button-th"">Quantity</th>
                                    <th scope=""col"" class=""total-price-th"">Total");
            WriteLiteral("</th>\r\n                                    <th scope=\"col\" class=\"remove-th\">Remove</th>\r\n                                </tr>\r\n                            </thead>\r\n");
            WriteLiteral("                            ");
#nullable restore
#line 26 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Wishlist\Index.cshtml"
                       Write(Html.Partial("_WishlistItemPartial",Model));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </table>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </section>\r\n    </main>\r\n    <!-- Main End -->");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<WishlistItem>> Html { get; private set; }
    }
}
#pragma warning restore 1591
