using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;
using System.Data;

namespace BLL
{
    public class Sistema
    {
        // INSTANCIA CONECÇÃO SQL
        MySQL mySQL = new MySQL();

        public void CalculaValoresObra(DTO.Obra obra)
        {
            // TOTAL CONTRATADA
            obra.Valor_Total_Contratada = obra.Valor_MaoObra + obra.Valor_Material;
            // MATERIAL IMPACTO
            obra.Valor_Material_Impacto = obra.Valor_Material_OPT + obra.Valor_Material_IND;
            // MAO OBRA IMPACTO
            obra.Valor_MaoObra_Impacto = obra.Valor_MaoObra_OPT + obra.Valor_MaoObra_IND;
            // TOTAL IMPACTO   
            obra.Valor_Total_Impacto = obra.Valor_MaoObra_Impacto + obra.Valor_Material_Impacto;
            // MATERIAL FISCALIZAÇÃO
            obra.Valor_Material_Fiscalizacao = obra.Valor_Material - obra.Valor_Material_Impacto;
            // MÃO OBRA FISCALIZAÇÃO
            obra.Valor_MaoObra_Fiscalizacao = obra.Valor_MaoObra - obra.Valor_MaoObra_Impacto;
            // TOTAL FISCALIZAÇÃO
            obra.Valor_Total_Fiscaizacao = obra.Valor_Material_Fiscalizacao + obra.Valor_MaoObra_Fiscalizacao;
            // PERCENTAL MATERIAL IMPACTO
            if (obra.Valor_Material_Impacto > 0 && obra.Valor_Material > 0)
            {
                obra.Percentual_Material_Impacto = obra.Valor_Material_Impacto / obra.Valor_Material * 100;
            }
            // PERCENTAL MÃO OBRA IMPACTO
            if (obra.Valor_MaoObra_Impacto > 0 && obra.Valor_MaoObra > 0)
            {
                obra.Percentual_MaoObra_Impacto = obra.Valor_MaoObra_Impacto / obra.Valor_MaoObra * 100;
            }           
            // PERCENTAL TOTAL IMPACTO
            if (obra.Valor_Total_Impacto > 0 && obra.Valor_Total_Contratada > 0)
            {
                obra.Percentual_Total_Impacto = obra.Valor_Total_Impacto / obra.Valor_Total_Contratada * 100;
            }
            
        }
        public void ObraNova(Obra obra, string re, string tecnico)
        {
            mySQL.LimparParametros();
            mySQL.AdicionarParametro("varNumObra", obra.numObra);
            mySQL.AdicionarParametro("varContrato", obra.contrato);
            mySQL.AdicionarParametro("VarValorMaoObra", obra.ValorMaoObra);
            mySQL.AdicionarParametro("varRE", re);
            mySQL.AdicionarParametro("varTecnico", tecnico);
            mySQL.Persistir(CommandType.StoredProcedure, "ObraNova");
        }
        public void ObraAltera(Obra obra)
        {
            mySQL.LimparParametros();
            mySQL.AdicionarParametro("varID", obra.ID);
            mySQL.AdicionarParametro("varNumObra", obra.numObra);
            mySQL.AdicionarParametro("varContrato", obra.contrato);
            mySQL.AdicionarParametro("VarValorMaoObra", obra.ValorMaoObra);
            mySQL.Persistir(CommandType.StoredProcedure, "ObraAltera");
        }
        public void ObraAlteraEstatus(Obra obra)
        {
            mySQL.LimparParametros();
            mySQL.AdicionarParametro("varID", obra.ID);
            mySQL.AdicionarParametro("varEstatus", obra.Estatus);
            mySQL.Persistir(CommandType.StoredProcedure, "ObraAlteraEstatus");
        }
        public void ObraDeletar(int idObra)
        {
            mySQL.LimparParametros();
            mySQL.AdicionarParametro("varIdObra", idObra);
            mySQL.Persistir(CommandType.StoredProcedure, "ObraDeletar");
        }
        public DTO.ObraLista ObraListarTodas()
        {
            var obraLista = new DTO.ObraLista();

            mySQL.LimparParametros();
            DataTable dataTable = mySQL.Consultar(CommandType.StoredProcedure, "ObraListarTodas");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                var obra = new DTO.Obra();
                obra.ID = Convert.ToInt32(dataRow["ID"]);
                obra.numObra = Convert.ToString(dataRow["NumObra"]);
                obra.contrato = Convert.ToString(dataRow["Contrato"]);
                obra.Estatus = Convert.ToString(dataRow["Estatus"]);
                obra.RE = Convert.ToString(dataRow["RE"]);
                obra.Tecnico = Convert.ToString(dataRow["Tecnico"]);
                obra.DataCriacao = Convert.ToDateTime(dataRow["DataCriacao"]);

                // VERIFICA SE VALOR É NULLO
                string Valor_MaoObra = Convert.ToString(dataRow["Valor_MaoObra"]);
                if (Valor_MaoObra == "")
                { obra.Valor_MaoObra = 0; }
                else
                { obra.Valor_MaoObra = Convert.ToDecimal(dataRow["Valor_MaoObra"]); }

                // VERIFICA SE VALOR É NULLO
                string Valor_Material = Convert.ToString(dataRow["Valor_Material"]);
                if (Valor_Material == "")
                { obra.Valor_Material = 0; }
                else
                { obra.Valor_Material = Convert.ToDecimal(dataRow["Valor_Material"]); }

                // VERIFICA SE VALOR É NULLO
                string Valor_Material_OPT = Convert.ToString(dataRow["Valor_Material_OPT"]);
                if (Valor_Material_OPT == "")
                { obra.Valor_Material_OPT = 0; }
                else
                { obra.Valor_Material_OPT = Convert.ToDecimal(dataRow["Valor_Material_OPT"]); }

                // VERIFICA SE VALOR É NULLO
                string Valor_Material_IND = Convert.ToString(dataRow["Valor_Material_IND"]);
                if (Valor_Material_IND == "")
                { obra.Valor_Material_IND = 0; }
                else
                { obra.Valor_Material_IND = Convert.ToDecimal(dataRow["Valor_Material_IND"]); }

                // VERIFICA SE VALOR É NULLO
                string Valor_MaoObra_OPT = Convert.ToString(dataRow["Valor_MaoObra_OPT"]);
                if (Valor_MaoObra_OPT == "")
                { obra.Valor_MaoObra_OPT = 0; }
                else
                { obra.Valor_MaoObra_OPT = Convert.ToDecimal(dataRow["Valor_MaoObra_OPT"]); }


                // VERIFICA SE VALOR É NULLO
                string Valor_MaoObra_IND = Convert.ToString(dataRow["Valor_MaoObra_IND"]);
                if (Valor_MaoObra_IND == "")
                {obra.Valor_MaoObra_IND = 0;}
                else
                {obra.Valor_MaoObra_IND = Convert.ToDecimal(dataRow["Valor_MaoObra_IND"]);}


                CalculaValoresObra(obra);

                obraLista.Add(obra);
            }
            return obraLista;
        }
        public DTO.ObraLista ObraListarPorRe(string re)
        {
            var obraLista = new DTO.ObraLista();

            mySQL.LimparParametros();
            mySQL.AdicionarParametro("varRE", re);
            DataTable dataTable = mySQL.Consultar(CommandType.StoredProcedure, "ObraListarPorRe");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                var obra = new DTO.Obra();
                obra.ID = Convert.ToInt32(dataRow["ID"]);
                obra.numObra = Convert.ToString(dataRow["NumObra"]);
                obra.contrato = Convert.ToString(dataRow["Contrato"]);
                obra.ValorMaoObra = Convert.ToDecimal(dataRow["ValorMaoObra"]);
                obra.Estatus = Convert.ToString(dataRow["Estatus"]);
                obra.RE = Convert.ToString(dataRow["RE"]);
                obra.Tecnico = Convert.ToString(dataRow["Tecnico"]);
                obra.DataCriacao = Convert.ToDateTime(dataRow["DataCriacao"]);
                obraLista.Add(obra);
            }
            return obraLista;
        }
        public DTO.Obra ObraListarPorNumObra(string numObra)
        {
            var obra = new DTO.Obra();

            mySQL.LimparParametros();
            mySQL.AdicionarParametro("varNumObra", numObra);
            DataTable dataTable = mySQL.Consultar(CommandType.StoredProcedure, "ObraListarPorNumObra");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                obra.ID = Convert.ToInt32(dataRow["ID"]);
                obra.numObra = Convert.ToString(dataRow["NumObra"]);
                obra.contrato = Convert.ToString(dataRow["Contrato"]);
                obra.ValorMaoObra = Convert.ToDecimal(dataRow["ValorMaoObra"]);
                obra.Estatus = Convert.ToString(dataRow["Estatus"]);
                obra.RE = Convert.ToString(dataRow["RE"]);
                obra.Tecnico = Convert.ToString(dataRow["Tecnico"]);
                obra.DataCriacao = Convert.ToDateTime(dataRow["DataCriacao"]);
            }
            return obra;
        }
        public DTO.Obra ObraListarPorID(int idObra)
        {
            var obra = new DTO.Obra();

            mySQL.LimparParametros();
            mySQL.AdicionarParametro("varID", idObra);
            DataTable dataTable = mySQL.Consultar(CommandType.StoredProcedure, "ObraListarPorID");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                obra.ID = Convert.ToInt32(dataRow["ID"]);
                obra.numObra = Convert.ToString(dataRow["NumObra"]);
                obra.contrato = Convert.ToString(dataRow["Contrato"]);
                obra.ValorMaoObra = Convert.ToDecimal(dataRow["ValorMaoObra"]);
                obra.Estatus = Convert.ToString(dataRow["Estatus"]);
                obra.RE = Convert.ToString(dataRow["RE"]);
                obra.Tecnico = Convert.ToString(dataRow["Tecnico"]);
                obra.DataCriacao = Convert.ToDateTime(dataRow["DataCriacao"]);
            }
            return obra;
        }
        public DTO.ObraItemLista ObraItemListarPorNumObra(string numObra)
        {
            var obraItemLista = new DTO.ObraItemLista();

            mySQL.LimparParametros();
            mySQL.AdicionarParametro("varNumObra", numObra);
            DataTable dataTable = mySQL.Consultar(CommandType.StoredProcedure, "ObraItemListarPorNumObra");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                var obraItem = new ObraItem();
                obraItem.IdObra = Convert.ToInt32(dataRow["IdObra"]);
                obraItem.numObra = Convert.ToString(dataRow["NumObra"]);
                obraItem.contrato = Convert.ToString(dataRow["Contrato"]);
                obraItem.codigo = Convert.ToString(dataRow["Codigo"]);
                obraItem.descricao = Convert.ToString(dataRow["Descricao"]);
                obraItem.tipo = Convert.ToString(dataRow["Tipo"]);
                obraItem.MO_classe = Convert.ToString(dataRow["Mo_classe"]);
                obraItem.MO_ponto = Convert.ToDecimal(dataRow["MO_ponto"]);
                obraItem.MO_baremo = Convert.ToDecimal(dataRow["MO_baremo"]);
                obraItem.MO_itemOPT = Convert.ToDecimal(dataRow["MO_itemOPT"]);
                obraItem.MO_itemIND = Convert.ToDecimal(dataRow["MO_itemIND"]);
                obraItem.MO_valorOPT = Convert.ToDecimal(dataRow["MO_valorOPT"]);
                obraItem.MO_valorIND = Convert.ToDecimal(dataRow["MO_valorIND"]);
                obraItem.MT_valor = Convert.ToDecimal(dataRow["MT_valor"]);
                obraItem.MT_itemLMN = Convert.ToDecimal(dataRow["MT_itemLMN"]);
                obraItem.MT_itemOPT = Convert.ToDecimal(dataRow["MT_itemOPT"]);
                obraItem.MT_itemIND = Convert.ToDecimal(dataRow["MT_itemIND"]);
                obraItem.MT_itemDIF = Convert.ToDecimal(dataRow["MT_itemDIF"]);
                obraItem.MT_valorLMN = Convert.ToDecimal(dataRow["MT_valorLMN"]);
                obraItem.MT_valorOPT = Convert.ToDecimal(dataRow["MT_valorOPT"]);
                obraItem.MT_valorIND = Convert.ToDecimal(dataRow["MT_valorIND"]);
                obraItem.MT_valorDIF = Convert.ToDecimal(dataRow["MT_valorDIF"]);
                obraItemLista.Add(obraItem);
            }
            return obraItemLista;
        }
        public DTO.ObraItem ObraItemSelecionado(string numObra, string codigo)
        {
            var obraItem = new DTO.ObraItem();

            mySQL.LimparParametros();
            mySQL.AdicionarParametro("varNumObra", numObra);
            mySQL.AdicionarParametro("varCodigo", codigo);
            DataTable dataTable = mySQL.Consultar(CommandType.StoredProcedure, "ObraItemSelecionado");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                obraItem.IdObra = Convert.ToInt32(dataRow["IdObra"]);
                obraItem.numObra = Convert.ToString(dataRow["NumObra"]);
                obraItem.contrato = Convert.ToString(dataRow["Contrato"]);
                obraItem.codigo = Convert.ToString(dataRow["Codigo"]);
                obraItem.descricao = Convert.ToString(dataRow["Descricao"]);
                obraItem.tipo = Convert.ToString(dataRow["Tipo"]);
                obraItem.MO_classe = Convert.ToString(dataRow["Mo_classe"]);
                obraItem.MO_ponto = Convert.ToDecimal(dataRow["MO_ponto"]);
                obraItem.MO_baremo = Convert.ToDecimal(dataRow["MO_baremo"]);
                obraItem.MO_itemOPT = Convert.ToDecimal(dataRow["MO_itemOPT"]);
                obraItem.MO_itemIND = Convert.ToDecimal(dataRow["MO_itemIND"]);
                obraItem.MO_valorOPT = Convert.ToDecimal(dataRow["MO_valorOPT"]);
                obraItem.MO_valorIND = Convert.ToDecimal(dataRow["MO_valorIND"]);
                obraItem.MT_valor = Convert.ToDecimal(dataRow["MT_valor"]);
                obraItem.MT_itemLMN = Convert.ToDecimal(dataRow["MT_itemLMN"]);
                obraItem.MT_itemOPT = Convert.ToDecimal(dataRow["MT_itemOPT"]);
                obraItem.MT_itemIND = Convert.ToDecimal(dataRow["MT_itemIND"]);
                obraItem.MT_itemDIF = Convert.ToDecimal(dataRow["MT_itemDIF"]);
                obraItem.MT_valorLMN = Convert.ToDecimal(dataRow["MT_valorLMN"]);
                obraItem.MT_valorOPT = Convert.ToDecimal(dataRow["MT_valorOPT"]);
                obraItem.MT_valorIND = Convert.ToDecimal(dataRow["MT_valorIND"]);
                obraItem.MT_valorDIF = Convert.ToDecimal(dataRow["MT_valorDIF"]);
            }
            return obraItem;
        }
        public void ObraGravarMT(DTO.ObraItem obraItem)
        {
            mySQL.LimparParametros();
            mySQL.AdicionarParametro("varIdObra", obraItem.IdObra);
            mySQL.AdicionarParametro("varNumObra", obraItem.numObra);
            mySQL.AdicionarParametro("varContrato", obraItem.contrato);
            mySQL.AdicionarParametro("varCodigo", obraItem.codigo);
            mySQL.AdicionarParametro("varDescricao", obraItem.descricao);
            mySQL.AdicionarParametro("varTipo", obraItem.tipo);
            mySQL.AdicionarParametro("varMT_valor", obraItem.MT_valor);
            mySQL.AdicionarParametro("varMT_itemLMN", obraItem.MT_itemLMN);
            mySQL.AdicionarParametro("varMT_itemOPT", obraItem.MT_itemOPT);
            mySQL.AdicionarParametro("varMT_itemIND", obraItem.MT_itemIND);
            mySQL.AdicionarParametro("varMT_itemDIF", obraItem.MT_itemDIF);
            mySQL.AdicionarParametro("varMT_valorLMN", obraItem.MT_valorLMN);
            mySQL.AdicionarParametro("varMT_valorOPT", obraItem.MT_valorOPT);
            mySQL.AdicionarParametro("varMT_valorIND", obraItem.MT_valorIND);
            mySQL.AdicionarParametro("varMT_valorDIF", obraItem.MT_valorDIF);
            mySQL.Persistir(CommandType.StoredProcedure, "ObraGravarMT");
        }
        public void ObraGravarMO(DTO.ObraItem obraItem)
        {
            mySQL.LimparParametros();
            mySQL.AdicionarParametro("varIdObra", obraItem.IdObra);
            mySQL.AdicionarParametro("varNumObra", obraItem.numObra);
            mySQL.AdicionarParametro("varContrato", obraItem.contrato);
            mySQL.AdicionarParametro("varCodigo", obraItem.codigo);
            mySQL.AdicionarParametro("varDescricao", obraItem.descricao);
            mySQL.AdicionarParametro("varTipo", obraItem.tipo);
            mySQL.AdicionarParametro("varMo_classe", obraItem.MO_classe);
            mySQL.AdicionarParametro("varMO_ponto", obraItem.MO_ponto);
            mySQL.AdicionarParametro("varMO_baremo", obraItem.MO_baremo);
            mySQL.AdicionarParametro("varMO_itemOPT", obraItem.MO_itemOPT);
            mySQL.AdicionarParametro("varMO_itemIND", obraItem.MO_itemIND);
            mySQL.AdicionarParametro("varMO_valorOPT", obraItem.MO_valorOPT);
            mySQL.AdicionarParametro("varMO_valorIND", obraItem.MO_valorIND);
            mySQL.Persistir(CommandType.StoredProcedure, "ObraGravarMO");
        }
        public void ObraAlterarMT(DTO.ObraItem obraItem)
        {
            mySQL.LimparParametros();
            mySQL.AdicionarParametro("varNumObra", obraItem.numObra);
            mySQL.AdicionarParametro("varContrato", obraItem.contrato);
            mySQL.AdicionarParametro("varCodigo", obraItem.codigo);
            mySQL.AdicionarParametro("varMT_itemLMN", obraItem.MT_itemLMN);
            mySQL.AdicionarParametro("varMT_itemOPT", obraItem.MT_itemOPT);
            mySQL.AdicionarParametro("varMT_itemIND", obraItem.MT_itemIND);
            mySQL.AdicionarParametro("varMT_itemDIF", obraItem.MT_itemDIF);
            mySQL.AdicionarParametro("varMT_valorLMN", obraItem.MT_valorLMN);
            mySQL.AdicionarParametro("varMT_valorOPT", obraItem.MT_valorOPT);
            mySQL.AdicionarParametro("varMT_valorIND", obraItem.MT_valorIND);
            mySQL.AdicionarParametro("varMT_valorDIF", obraItem.MT_valorDIF);
            mySQL.Persistir(CommandType.StoredProcedure, "ObraAlterarMT");
        }
        public void ObraAlterarMO(DTO.ObraItem obraItem)
        {
            mySQL.LimparParametros();
            mySQL.AdicionarParametro("varNumObra", obraItem.numObra);
            mySQL.AdicionarParametro("varContrato", obraItem.contrato);
            mySQL.AdicionarParametro("varCodigo", obraItem.codigo);
            mySQL.AdicionarParametro("varMO_itemOPT", obraItem.MO_itemOPT);
            mySQL.AdicionarParametro("varMO_itemIND", obraItem.MO_itemIND);
            mySQL.AdicionarParametro("varMO_valorOPT", obraItem.MO_valorOPT);
            mySQL.AdicionarParametro("varMO_valorIND", obraItem.MO_valorIND);
            mySQL.Persistir(CommandType.StoredProcedure, "ObraAlterarMO");
        }
        public void ObraDeletarItem(DTO.ObraItem obraItem)
        {
            mySQL.LimparParametros();
            mySQL.AdicionarParametro("varNumObra", obraItem.numObra);
            mySQL.AdicionarParametro("varContrato", obraItem.contrato);
            mySQL.AdicionarParametro("varCodigo", obraItem.codigo);
            mySQL.Persistir(CommandType.StoredProcedure, "ObraDeletarItem");
        }
        public DTO.ConttratoLista ContratoListaTodos()
        {
            var contratoLista = new DTO.ConttratoLista();

            mySQL.LimparParametros();
            DataTable dataTable = mySQL.Consultar(CommandType.StoredProcedure, "ContratoListaTodos");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                var contrato = new DTO.Contrato();
                contrato.contrato = Convert.ToString(dataRow["Contrato"]);
                contratoLista.Add(contrato);
            }

            return contratoLista;
        }
        public decimal ValorBaremoPorContratoClasse(string contratoInformado, string classeInformada)
        {
            decimal valorBaremo = 0;

            mySQL.LimparParametros();
            mySQL.AdicionarParametro("varContrato", contratoInformado);
            mySQL.AdicionarParametro("varClasse", classeInformada);
            DataTable dataTable = mySQL.Consultar(CommandType.StoredProcedure, "ValorBaremoPorContratoClasse");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                valorBaremo = Convert.ToDecimal(dataRow["Valor"]);
            }

            return valorBaremo;
        }
        public int VerificaSeExsiteItem(string numObra, string contrato, string codigo)
        {
            int retorno = 0;

            mySQL.LimparParametros();
            mySQL.AdicionarParametro("varNumObra", numObra);
            mySQL.AdicionarParametro("varContrato", contrato);
            mySQL.AdicionarParametro("varCodigo", codigo);
            DataTable dataTable = mySQL.Consultar(CommandType.StoredProcedure, "VerificaSeExsiteItem");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                retorno = Convert.ToInt32(dataRow["ID"]);
            }

            return retorno;
        }
        public int VerificaSeObraExiste(string numObra, string contrato)
        {
            int retorno = 0;

            mySQL.LimparParametros();
            mySQL.AdicionarParametro("varNumObra", numObra);
            mySQL.AdicionarParametro("varContrato", contrato);
            DataTable dataTable = mySQL.Consultar(CommandType.StoredProcedure, "VerificaSeObraExiste");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                retorno = Convert.ToInt32(dataRow["ID"]);
            }

            return retorno;
        }
        public DTO.Codigo CodigoListaPorCOD(string codigoInformado)
        {
            var codigo = new DTO.Codigo();

            mySQL.LimparParametros();
            mySQL.AdicionarParametro("varCOD", codigoInformado);
            DataTable dataTable = mySQL.Consultar(CommandType.StoredProcedure, "CodigoListaPorCOD");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                codigo.cod = Convert.ToString(dataRow["COD"]);
                codigo.descricao = Convert.ToString(dataRow["descricao"]);
                codigo.tipo = Convert.ToString(dataRow["Tipo"]);
                codigo.MO_classe = Convert.ToString(dataRow["MO_classe"]);
                codigo.MO_ponto = Convert.ToDecimal(dataRow["MO_ponto"]);
                codigo.MT_valor = Convert.ToDecimal(dataRow["MT_valor"]);
            }
            return codigo;
        }
    }
}
 