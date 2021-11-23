using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTO;
using BLL;

namespace WEB.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Credenciais()
        {
            // ABANDONA SESSÕES CRIADAS
            Session.Abandon();

            // MODELO USUÁRIO
            var usuario = new Usuario();
            return View("Credenciais",usuario);
        }

        [HttpPost]
        public ActionResult Validacao(Usuario usuario)
        {
            //INSTANCIAS
            var bll = new BLL.Login();
            var ususarioAutenticado = new DTO.Usuario();
            ususarioAutenticado = bll.usuarioAutenticado(usuario);

            // VERIFICA SE USUÁRIO EXISTE
            if (ususarioAutenticado.id > 0)
            {
                // CRIA SESSÃO NO SERVIDOR
                Session["Nome"] = ususarioAutenticado.nome;
                Session["RE"] = ususarioAutenticado.re;
                Session["Perfil"] = ususarioAutenticado.perfil;
                Session.Timeout = 10;

                return RedirectToAction("Portal","Sistema");
            }
            else
            {
                ViewBag.Retorno = "erro";
                usuario = null;
                return View("Credenciais", usuario);
            }         
        }
    }
}