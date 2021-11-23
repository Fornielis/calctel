using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTO;

namespace WEB.Controllers
{
    public class SistemaController : Controller
    {
        public void calculaMT(DTO.ObraItem obraItem)
        {
            obraItem.MT_valorLMN = obraItem.MT_itemLMN * obraItem.MT_valor;
            obraItem.MT_valorOPT = obraItem.MT_itemOPT * obraItem.MT_valor;
            obraItem.MT_valorIND = obraItem.MT_itemIND * obraItem.MT_valor;
            obraItem.MT_valorDIF = obraItem.MT_valorLMN - (obraItem.MT_valorOPT + obraItem.MT_valorIND);
            obraItem.MT_itemDIF = obraItem.MT_itemLMN - (obraItem.MT_itemOPT + obraItem.MT_itemIND);
        }
        public void calculaMO(DTO.ObraItem obraItem)
        {
            obraItem.MO_valorOPT = obraItem.MO_itemOPT * obraItem.MO_ponto * obraItem.MO_baremo;
            obraItem.MO_valorIND = obraItem.MO_itemIND * obraItem.MO_ponto * obraItem.MO_baremo;
        }
        public PartialViewResult Erro()
        {
            return PartialView("~/Views/Sistema/_Erro.cshtml");
        }
        public PartialViewResult ErroSessao()
        {
            return PartialView("~/Views/Sistema/_ErroSessao.cshtml");
        }
        public PartialViewResult ContratoExiste()
        {
            return PartialView("~/Views/Sistema/_AvisoContratoJaExiste.cshtml");
        }
        public ActionResult Portal()
        {
            try
            {
                // TESTA SE A SESSÃO ESTA ATIVA
                if (Session["Perfil"] == null)
                {
                    return RedirectToAction("Credenciais", "Login");
                }
                return View("Portal");
            }
            catch (Exception exception)
            {
                return PartialView("~/Views/Sistema/_Erro.cshtml");
            }
        }
        public PartialViewResult ObraLista()
        {
            try
            {
                // TESTA SE A SESSÃO ESTA ATIVA
                if (Session["Perfil"] == null)
                {
                    return PartialView("~/Views/Sistema/_ErroSessao.cshtml");
                }

                // INSTÂNCIAS
                var bll = new BLL.Sistema();
                var contratoLista = new DTO.ConttratoLista();
                var obraLista = new DTO.ObraLista();

                // RESGATA LISTA CONTRATOS
                contratoLista = bll.ContratoListaTodos();
                ViewBag.Contratos = contratoLista;

                // RESGATA LISTA OBRAS CADASTRADAS
                // PERFIL ADM
                if (Session["Perfil"].Equals("ADM"))
                {
                    obraLista = bll.ObraListarTodas();
                }
                else
                {
                    obraLista = bll.ObraListarPorRe(Session["RE"].ToString());
                }

                // VERIFICA SE A LISTA DE OBRA ESTÁ VAZIA
                if (obraLista == null)
                {
                    ViewBag.ListaVazia = "True";
                }

                return PartialView("~/Views/Sistema/_ListaObras.cshtml", obraLista);
            }
            catch (Exception exception)
            {
                return PartialView("~/Views/Sistema/_Erro.cshtml");
            }
        }
        public void ObraNova(Obra obra)
        {
            try
            {
                // TESTA SE A SESSÃO ESTA ATIVA
                if (Session["Perfil"] == null)
                {
                    Response.Redirect("~/Sistema/ErroSessao");
                }

                // INSTÂNCIA BLL
                var bll = new BLL.Sistema();

                // VERIFICA SE CONTRATO JÁ EXISTE PARA OBRA ESCOLHIDA
                int retorno = bll.VerificaSeObraExiste(obra.numObra, obra.contrato);
                if (retorno > 0)
                {
                    Response.Redirect("~/Sistema/ContratoExiste");
                }
                else
                {
                    // SALVA OBRA NO BANCO
                    bll.ObraNova(obra, Session["RE"].ToString(), Session["Nome"].ToString());

                    Response.Redirect("~/Sistema/ObraLista");
                }             
            }
            catch (Exception exception)
            {
                Response.Redirect("~/Sistema/Erro");
            }
        }
        public PartialViewResult ObraItens(string numObra)
        {
            try
            {
                // TESTA SE A SESSÃO ESTA ATIVA
                if (Session["Perfil"] == null)
                {
                    return PartialView("~/Views/Sistema/_ErroSessao.cshtml");
                }

                // INSTÂNCIAS
                var bll = new BLL.Sistema();
                var obra = new DTO.Obra();
                var obraItemLista = new DTO.ObraItemLista();
                var codigo = new DTO.Codigo();

                // RESGATA OBRA PELO ID
                obra = bll.ObraListarPorNumObra(numObra);
                ViewBag.Obra = obra;

                //RESGATA LISTA ITENS - OBRA
                obraItemLista = bll.ObraItemListarPorNumObra(numObra);
                ViewBag.Itens = obraItemLista;

                return PartialView("~/Views/Sistema/_ListaObraItens.cshtml", codigo);
            }
            catch (Exception exception)
            {
                return PartialView("~/Views/Sistema/_Erro.cshtml");
            }
        }
        public PartialViewResult ObraItemSelecionado(string numObra, string codigo)
        {
            try
            {
                // TESTA SE A SESSÃO ESTA ATIVA
                if (Session["Perfil"] == null)
                {
                    return PartialView("~/Views/Sistema/_ErroSessao.cshtml");
                }

                // RESGATA INFORMAÇÕES DO ITEM
                var bll = new BLL.Sistema();
                var obraItem = new DTO.ObraItem();
                obraItem = bll.ObraItemSelecionado(numObra,codigo);              

                // SE FOR MÃO OBRA
                if (obraItem.tipo == "MO")
                {
                    return PartialView("~/Views/Sistema/_AlterarItemMO.cshtml", obraItem);
                }
                // SE FOR MATERIAL
                else
                {
                    return PartialView("~/Views/Sistema/_AlterarItemMT.cshtml", obraItem);
                }               
            }
            catch (Exception exception)
            {
                return PartialView("~/Views/Sistema/_Erro.cshtml");
            }
        }
        public void ObraGravarMT(DTO.ObraItem obraItem)
        {
            try
            {
                // TESTA SE A SESSÃO ESTA ATIVA
                if (Session["Perfil"] == null)
                {
                    Response.Redirect("~/Sistema/ErroSessao");
                }
                // CALCULA VALORES MT
                calculaMT(obraItem);

                // SALVA ITEM NO BANCO
                var bll = new BLL.Sistema();
                bll.ObraGravarMT(obraItem);

                Response.Redirect("~/Sistema/ObraItens?numObra=" + obraItem.numObra);
            }
            catch (Exception exception)
            {
                Response.Redirect("~/Sistema/Erro");
            }
        }
        public void ObraGravarMO(DTO.ObraItem obraItem)
        {
            try
            {
                // TESTA SE A SESSÃO ESTA ATIVA
                if (Session["Perfil"] == null)
                {
                    Response.Redirect("~/Sistema/ErroSessao");
                }

                // CALCULA VALORES MO
                calculaMO(obraItem);

                // SALVA ITEM NO BANCO
                var bll = new BLL.Sistema();
                bll.ObraGravarMO(obraItem);

                Response.Redirect("~/Sistema/ObraItens?numObra=" + obraItem.numObra);
            }
            catch (Exception exception)
            {
                Response.Redirect("~/Sistema/Erro");
            }
        }
        public void ObraAlterarMT(DTO.ObraItem obraItem)
        {
            try
            {
                // TESTA SE A SESSÃO ESTA ATIVA
                if (Session["Perfil"] == null)
                {
                    Response.Redirect("~/Sistema/ErroSessao");
                }
                // CALCULA VALORES MT
                calculaMT(obraItem);

                // SALVA ITEM NO BANCO
                var bll = new BLL.Sistema();
                bll.ObraAlterarMT(obraItem);

                Response.Redirect("~/Sistema/ObraItens?numObra=" + obraItem.numObra);
            }
            catch (Exception exception)
            {
                Response.Redirect("~/Sistema/Erro");
            }
        }
        public void ObraAlterarMO(DTO.ObraItem obraItem)
        {
            try
            {
                // TESTA SE A SESSÃO ESTA ATIVA
                if (Session["Perfil"] == null)
                {
                    Response.Redirect("~/Sistema/ErroSessao");
                }

                // CALCULA VALORES MO
                calculaMO(obraItem);

                // SALVA ITEM NO BANCO
                var bll = new BLL.Sistema();
                bll.ObraAlterarMO(obraItem);

                Response.Redirect("~/Sistema/ObraItens?numObra=" + obraItem.numObra);
            }
            catch (Exception exception)
            {
                Response.Redirect("~/Sistema/Erro");
            }
        }
        public void ObraDeletarItem(string numObra, string codigo, string contrato)
        {
            try
            {
                // TESTA SE A SESSÃO ESTA ATIVA
                if (Session["Perfil"] == null)
                {
                    Response.Redirect("~/Sistema/ErroSessao");
                }

                // DELETA ITEM NO BANCO
                var bll = new BLL.Sistema();
                var obraItem = new ObraItem();
                    obraItem.numObra = numObra;
                    obraItem.contrato = contrato;
                    obraItem.codigo = codigo;

                bll.ObraDeletarItem(obraItem);

                Response.Redirect("~/Sistema/ObraItens?numObra=" + obraItem.numObra);
            }
            catch (Exception exception)
            {
                Response.Redirect("~/Sistema/Erro");
            }
        }
        public PartialViewResult CodigoPorCod(FormCollection dadosFormilario)
        {
            try
            {
                // TESTA SE A SESSÃO ESTA ATIVA
                if (Session["Perfil"] == null)
                {
                    return PartialView("~/Views/Sistema/_ErroSessao.cshtml");
                }

                // INSTÂNCIA BLL
                var bll = new BLL.Sistema();

                // RESGATA OBRA PELO ID
                var obra = new DTO.Obra();
                obra = bll.ObraListarPorNumObra(dadosFormilario["numObra"]);
                ViewBag.Obra = obra;

                // RESGATA DADOS DO CÓDIGO SELECIONADO
                var dadosCodigo = new DTO.Codigo();
                dadosCodigo = bll.CodigoListaPorCOD(dadosFormilario["cod"]);
                ViewBag.Codigo = dadosCodigo;

                // MODELO PARA VIEW
                var obraItem = new DTO.ObraItem();

                // VERIFICA SE O CÓDIGO EXISTE NA BASE DE DADOS
                // CASO NÃO EXISTA MENSAGEM AVISO É ENVIADA
                if (dadosCodigo.cod == null)
                {
                    return PartialView("~/Views/Sistema/_AvisoCodigoEnexistente.cshtml", dadosCodigo);
                }

                // VERIFICA SE CÓDIGO INFORMADO JÁ EXISTE PARA OBRA
                int retorno = bll.VerificaSeExsiteItem(obra.numObra,obra.contrato,dadosCodigo.cod);
                if (retorno > 0)
                {
                    return PartialView("~/Views/Sistema/_AvisoItemExistParaObra.cshtml", dadosCodigo);
                }

                // VERIFICA TIPO DE CÓDIGO
                // SE FOR MÃO DE OBRA RESGATA VALOR BAREMO POR CONTRATO E CLASSE
                if (dadosCodigo.tipo == "MO")
                {
                    dadosCodigo.MO_baremo = bll.ValorBaremoPorContratoClasse(obra.contrato,dadosCodigo.MO_classe);
                    return PartialView("~/Views/Sistema/_GravarItemMO.cshtml", obraItem);
                }
                
                return PartialView("~/Views/Sistema/_GravarItemMT.cshtml", obraItem);
            }
            catch (Exception exception)
            {
                return PartialView("~/Views/Sistema/_Erro.cshtml");
            }
        }
        public PartialViewResult ObraAcaoAlterar(int idObra)
        {
            try
            {
                // TESTA SE A SESSÃO ESTA ATIVA
                if (Session["Perfil"] == null)
                {
                    return PartialView("~/Views/Sistema/_ErroSessao.cshtml");
                }

                // INSTÂNCIA BLL
                var bll = new BLL.Sistema();
                
                // RESGATA LISTA CONTRATOS
                var contratoLista = new DTO.ConttratoLista();
                contratoLista = bll.ContratoListaTodos();
                ViewBag.Contratos = contratoLista;

                // RESGATA DASO OBRA PELO ID
                var obra = new DTO.Obra();
                obra = bll.ObraListarPorID(idObra);

                // VARIAVEIS PARA CONFIRMAR SE OBRA OU CONTRATO FRAM ALTERADOS
                obra.numObraChecagem = obra.numObra;
                obra.contratoChecagem = obra.contrato;

                return PartialView("~/Views/Sistema/_AcaoObraAlterar.cshtml", obra);
            }
            catch (Exception exception)
            {
                return PartialView("~/Views/Sistema/_Erro.cshtml");
            }
        }
        public PartialViewResult ObraAcaoAlterarEstatus(int idObra)
        {
            try
            {
                // TESTA SE A SESSÃO ESTA ATIVA
                if (Session["Perfil"] == null)
                {
                    return PartialView("~/Views/Sistema/_ErroSessao.cshtml");
                }
                
                // RESGATA DASO OBRA PELO ID
                var obra = new DTO.Obra();
                var bll = new BLL.Sistema();
                obra = bll.ObraListarPorID(idObra);

                return PartialView("~/Views/Sistema/_AcaoObraAlterarEstatus.cshtml", obra);
            }
            catch (Exception exception)
            {
                return PartialView("~/Views/Sistema/_Erro.cshtml");
            }
        }
        public PartialViewResult ObraAcaoDeletar(int idObra)
        {
            try
            {
                // TESTA SE A SESSÃO ESTA ATIVA
                if (Session["Perfil"] == null)
                {
                    return PartialView("~/Views/Sistema/_ErroSessao.cshtml");
                }

                // RESGATA DASO OBRA PELO ID
                var obra = new DTO.Obra();
                var bll = new BLL.Sistema();
                obra = bll.ObraListarPorID(idObra);

                return PartialView("~/Views/Sistema/_AcaoObraDeletar.cshtml", obra);
            }
            catch (Exception exception)
            {
                return PartialView("~/Views/Sistema/_Erro.cshtml");
            }
        }
        public void ObraAlterar(Obra obra)
        {
            try
            {
                // TESTA SE A SESSÃO ESTA ATIVA
                if (Session["Perfil"] == null)
                {
                    Response.Redirect("~/Sistema/ErroSessao");
                }

                // INSTÂNCIA BLL
                var bll = new BLL.Sistema();

                // VERIFICA SE CONTRATO JÁ EXISTE PARA OBRA ESCOLHIDA
                if ((obra.numObra != obra.numObraChecagem) || (obra.contrato != obra.contratoChecagem))
                {
                    int retorno = bll.VerificaSeObraExiste(obra.numObra, obra.contrato);
                    if (retorno > 0)
                    {
                        Response.Redirect("~/Sistema/ContratoExiste");
                    }
                    else
                    {
                        // ALTERA OBRA NO BANCO
                        bll.ObraAltera(obra);
                        Response.Redirect("~/Sistema/ObraLista");
                    }
                }              
                else
                {
                    // ALTERA OBRA NO BANCO
                    bll.ObraAltera(obra);
                    Response.Redirect("~/Sistema/ObraLista");
                }
            }
            catch (Exception exception)
            {
                Response.Redirect("~/Sistema/Erro");
            }
        }
        public void ObraAlterarEstatus(Obra obra)
        {
            try
            {
                // TESTA SE A SESSÃO ESTA ATIVA
                if (Session["Perfil"] == null)
                {
                    Response.Redirect("~/Sistema/ErroSessao");
                }

                // ALTERA OBRA NO BANCO
                var bll = new BLL.Sistema();
                bll.ObraAlteraEstatus(obra);

                Response.Redirect("~/Sistema/ObraLista");
            }
            catch (Exception exception)
            {
                Response.Redirect("~/Sistema/Erro");
            }
        }
        public void ObraDeletar(int idObra)
        {
            try
            {
                // TESTA SE A SESSÃO ESTA ATIVA
                if (Session["Perfil"] == null)
                {
                    Response.Redirect("~/Sistema/ErroSessao");
                }

                // DELETA E ELEMENTOS NO BANCO
                var bll = new BLL.Sistema();
                bll.ObraDeletar(idObra);

                Response.Redirect("~/Sistema/ObraLista");
            }
            catch (Exception exception)
            {
                Response.Redirect("~/Sistema/Erro");
            }
        }
    }
}