#pragma checksum "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "cf396355478205e12d95718a7f400e55a4fe43d0"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__ProductModalPartial), @"mvc.1.0.view", @"/Views/Shared/_ProductModalPartial.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cf396355478205e12d95718a7f400e55a4fe43d0", @"/Views/Shared/_ProductModalPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"215b113bcedaad543cefa7ec22695e2f56ae83ca", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__ProductModalPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Product>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("width:301px;height:241px;object-fit:contain"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("alt", new global::Microsoft.AspNetCore.Html.HtmlString(""), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("width:92px;height:62px;object-fit:contain;padding:10px"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "home", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "productdetail", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("add-to-basket"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "addbasket", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "shop", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
  
        int counter = 0;
        int thumbcounter = 0;    

#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"modal-dialog modal-lg\">\r\n    <div class=\"modal-content\">\r\n        <div class=\"row\">\r\n            <div class=\"col-lg-5\" style=\"margin-top:50px\">\r\n");
#nullable restore
#line 10 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                 foreach (var item in Model.ProductImages)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <div");
            BeginWriteAttribute("class", " class=\"", 340, "\"", 389, 3);
            WriteAttributeValue("", 348, "image-block", 348, 11, true);
            WriteAttributeValue(" ", 359, "image-block-", 360, 13, true);
#nullable restore
#line 12 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
WriteAttributeValue("", 372, thumbcounter++, 372, 17, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("style", " style=\"", 390, "\"", 455, 1);
#nullable restore
#line 12 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
WriteAttributeValue("", 398, thumbcounter == 1 ? "display:block;" : "display:none;", 398, 57, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "cf396355478205e12d95718a7f400e55a4fe43d07890", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 541, "~/uploads/products/", 541, 19, true);
#nullable restore
#line 13 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
AddHtmlAttributeValue("", 560, item.Image, 560, 11, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                </div>\r\n");
#nullable restore
#line 15 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("                <div class=\"row\">\r\n                    <ul class=\"product-images\" style=\"margin-top:50px\">\r\n");
#nullable restore
#line 18 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                         foreach (var item in Model.ProductImages)
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <li class=\"product-image-item\">\r\n                                <button style=\"border:1px solid #fcbe00\"");
            BeginWriteAttribute("class", " class=\"", 961, "\"", 1000, 2);
            WriteAttributeValue("", 969, "product-image-item-", 969, 19, true);
#nullable restore
#line 21 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
WriteAttributeValue("", 988, counter++, 988, 12, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "cf396355478205e12d95718a7f400e55a4fe43d010894", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 1113, "~/uploads/products/", 1113, 19, true);
#nullable restore
#line 22 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
AddHtmlAttributeValue("", 1132, item.Image, 1132, 11, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                                </button>\r\n                            </li>\r\n");
#nullable restore
#line 25 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                        }

#line default
#line hidden
#nullable disable
            WriteLiteral("                    </ul>\r\n                </div>\r\n            </div>\r\n            <div class=\"col-lg-7\">\r\n                <div class=\"custom-modal-header\">\r\n                    <h6>");
#nullable restore
#line 31 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                   Write(Model.Brand.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral(" ");
#nullable restore
#line 31 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                                     Write(Model.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h6>\r\n                </div>\r\n                <div class=\"custom-modal-rate\">\r\n");
#nullable restore
#line 34 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                     for (int i = 1; i <= 5; i++)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <i");
            BeginWriteAttribute("class", " class=\"", 1650, "\"", 1699, 2);
#nullable restore
#line 36 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
WriteAttributeValue("", 1658, Model.Rate>= i ? "fas" : "far", 1658, 33, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue(" ", 1691, "fa-star", 1692, 8, true);
            EndWriteAttribute();
            WriteLiteral("></i>\r\n");
#nullable restore
#line 37 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                </div>\r\n                <div class=\"custom-modal-price\">\r\n");
#nullable restore
#line 40 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                     if (Model.DiscountPercent > 0)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <div class=\"price-box\">\r\n                        </div>\r\n                        <span>$");
#nullable restore
#line 44 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                           Write((Model.SalePrice* (1- Model.DiscountPercent/100)).ToString("0.00"));

#line default
#line hidden
#nullable disable
            WriteLiteral(" <del>");
#nullable restore
#line 44 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                                                                                                     Write(Model.SalePrice);

#line default
#line hidden
#nullable disable
            WriteLiteral("</del></span>\r\n");
#nullable restore
#line 45 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                    }
                    else
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <span>$");
#nullable restore
#line 48 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                          Write(Model.SalePrice);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n");
#nullable restore
#line 49 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                </div>\r\n                <ul class=\"custom-modal-features\">\r\n                    <li>\r\n                        <p>Category: <span style=\"color:#fcbe00;cursor:pointer\">");
#nullable restore
#line 53 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                                                                           Write(Model.SubCategory.Category.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span></p>\r\n                    </li>\r\n                    <li>\r\n                        <p>Sub Category: <span style=\"color:#fcbe00;cursor:pointer\">");
#nullable restore
#line 56 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                                                                               Write(Model.SubCategory.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span></p>\r\n                    </li>\r\n                    <li>\r\n                        <p>RAM: <span style=\"color:#fcbe00;cursor:pointer\">");
#nullable restore
#line 59 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                                                                      Write(Model.Ram);

#line default
#line hidden
#nullable disable
            WriteLiteral(" GB</span></p>\r\n                    </li>\r\n                    <li>\r\n                        <p>Memory: <span style=\"color:#fcbe00;cursor:pointer\">");
#nullable restore
#line 62 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                                                                         Write(Model.Memory);

#line default
#line hidden
#nullable disable
            WriteLiteral(" GB</span></p>\r\n                    </li>\r\n                    <li>\r\n                        <p>Brand: <span style=\"color:#fcbe00;cursor:pointer\">");
#nullable restore
#line 65 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                                                                        Write(Model.Brand.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span></p>\r\n                    </li>\r\n                    <li>\r\n                        <p>Color: <span style=\"color:#fcbe00;cursor:pointer\">");
#nullable restore
#line 68 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                                                                        Write(Model.Color.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span></p>\r\n                    </li>\r\n                    <li>\r\n                        <p>Additional Information : <span style=\"font-size:14px;color:#666\">");
#nullable restore
#line 71 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                                                                                       Write(Model.Desc);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span></p>\r\n                    </li>\r\n                </ul>\r\n                <div class=\"custom-modal-buttons\">\r\n                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "cf396355478205e12d95718a7f400e55a4fe43d020229", async() => {
                WriteLiteral("Details");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 75 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                                                                          WriteLiteral(Model.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "cf396355478205e12d95718a7f400e55a4fe43d022664", async() => {
                WriteLiteral("Add to Basket");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_6.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_6);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_7.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 76 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Shared\_ProductModalPartial.cshtml"
                                                                                            WriteLiteral(Model.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Product> Html { get; private set; }
    }
}
#pragma warning restore 1591
