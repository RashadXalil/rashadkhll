#pragma checksum "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Order\Checkout.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a8e1b4a6777ac2cb7f4fa350719e73357d75d47c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Order_Checkout), @"mvc.1.0.view", @"/Views/Order/Checkout.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a8e1b4a6777ac2cb7f4fa350719e73357d75d47c", @"/Views/Order/Checkout.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"215b113bcedaad543cefa7ec22695e2f56ae83ca", @"/Views/_ViewImports.cshtml")]
    public class Views_Order_Checkout : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<CheckOutViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "home", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "index", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Order\Checkout.cshtml"
  
    ViewData["Title"] = "Checkout";

#line default
#line hidden
#nullable disable
            WriteLiteral("<main>\r\n    <section id=\"custom-banner-section-checkout\">\r\n        <div class=\"page-header\">\r\n            <p class=\"page-header-text\">Checkout</p>\r\n            <span>");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a8e1b4a6777ac2cb7f4fa350719e73357d75d47c4295", async() => {
                WriteLiteral("Home / ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("Checkout</span>\r\n        </div>\r\n    </section>\r\n    <section id=\"checkout-section\">\r\n        <div class=\"container\">\r\n                <div class=\"row\">\r\n                    <div class=\"col-lg-6\">\r\n                        ");
#nullable restore
#line 16 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Order\Checkout.cshtml"
                   Write(Html.Partial("_CheckoutFormPartial", Model.Order));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                    </div>
                    <div class=""col-lg-6"">
                        <div class=""your-order mb-30"">
                            <h3>Your order</h3>
                            <div class=""your-order-table"">
                                <table>
                                    <thead>
                                        <tr>
                                            <td>Product</td>
                                            <td>Total</td>
                                        </tr>
                                    </thead>
                                    <tbody>
");
#nullable restore
#line 30 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Order\Checkout.cshtml"
                                         foreach (var item in Model.Basket.BasketProducts)
                                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                            <tr class=\"card-item\">\r\n                                                <td class=\"product-name\">\r\n                                                    ");
#nullable restore
#line 34 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Order\Checkout.cshtml"
                                               Write(item.Product.Brand.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral(" ");
#nullable restore
#line 34 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Order\Checkout.cshtml"
                                                                        Write(item.Product.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                                    <strong> * ");
#nullable restore
#line 35 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Order\Checkout.cshtml"
                                                          Write(item.Count);

#line default
#line hidden
#nullable disable
            WriteLiteral("</strong>\r\n                                                </td>\r\n                                                <td class=\"product-total\">$");
#nullable restore
#line 37 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Order\Checkout.cshtml"
                                                                       Write((item.Product.DiscountPercent>0? (item.Product.SalePrice*(1-item.Product.DiscountPercent/100))*item.Count :item.Product.SalePrice*item.Count).ToString("0.00"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                            </tr>\r\n");
#nullable restore
#line 39 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Order\Checkout.cshtml"
                                        }

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    </tbody>\r\n                                    <tfoot>\r\n                                        <tr>\r\n                                            <td>Order Total</td>\r\n                                            <td>$");
#nullable restore
#line 44 "C:\Users\User\Desktop\Desktop\Pull\Final\FinalBack\FinalBack\Views\Order\Checkout.cshtml"
                                            Write(Model.Basket.TotalPrice.ToString("0.00"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</td>
                                        </tr>
                                    </tfoot>
                                </table>
                            </div>
                            <div class=""payment-method"">
                                <div class=""accordion"">
                                    <div class=""accordionheader"" id=""acheader1"">
                                        Direct Bank Transfer
                                    </div>
                                    <div class=""accordionbody"" style=""display: block"">
                                        <p>
                                            Make your payment directly into our bank account.
                                            Please use your Order ID as the payment reference.
                                            Your order won’t be shipped until the funds have
                                            cleared in our account.
                                        </p>
          ");
            WriteLiteral(@"                          </div>
                                    <div class=""accordionheader"" id=""acheader2"">
                                        Cheque Payment
                                    </div>
                                    <div class=""accordionbody"">
                                        <p>
                                            Please send your cheque to Store Name, Store Street,
                                            Store Town, Store State / County, Store Postcode.
                                        </p>
                                    </div>
                                    <div class=""accordionheader"" id=""acheader3"">
                                        PayPal
                                    </div>
                                    <div class=""accordionbody"">
                                        <p>
                                            Pay via PayPal; you can pay with your credit card if you don’t have a PayPal account.
");
            WriteLiteral(@"                                        </p>
                                    </div>
                                </div>
                                <div class=""order-button-payment mt-20"">
                                    <button type=""submit"" form=""order-form"" class=""tp-btn-h1"">
                                        Place order
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
        </div>
    </section>
</main>
");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<CheckOutViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
