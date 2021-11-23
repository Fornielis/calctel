using System.Web;
using System.Web.Optimization;

namespace WEB
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // LOGIN ///////////////////////////////////////////////////////////
            // CSS - CREDENCIAIS
            bundles.Add(new StyleBundle("~/Content/Credenciais").Include(
                      "~/Content/Global/bootstrap.css",
                      "~/Content/Animacoes/animate.css",
                      "~/Content/Sistema/Credenciais.css"));
            // JS - CREDENCIAIS
            bundles.Add(new ScriptBundle("~/bundles/Credenciais").Include(
                      "~/Scripts/Global/jquery-{version}.js",
                      "~/Scripts/Global/jquery.unobtrusive-ajax.js",
                      "~/Scripts/Validacao/jquery.validate*",
                      "~/Scripts/Global/bootstrap.js",
                      "~/Scripts/Sistema/login.js"));
            // SISTEMA ///////////////////////////////////////////////////////////
            // CSS - PORTAL
            bundles.Add(new StyleBundle("~/Content/Portal").Include(
                      "~/Content/Global/bootstrap.css",
                      "~/Content/Animacoes/animate.css",
                      "~/Content/Sistema/Portal.css"));
            // JS - PORTAL
            bundles.Add(new ScriptBundle("~/bundles/Portal").Include(
                      "~/Scripts/Global/jquery-{version}.js",
                      "~/Scripts/Global/jquery.mask.js",
                      "~/Scripts/Global/jquery.unobtrusive-ajax.js",
                      "~/Scripts/Validacao/jquery.validate*",
                      "~/Scripts/Global/bootstrap.js",
                      "~/Scripts/Sistema/portal.js"));
        }
    }
}
