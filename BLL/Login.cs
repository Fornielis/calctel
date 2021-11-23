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
    public class Login
    {
        // INSTANCIA CONECÇÃO SQL
        MySQL mySQL = new MySQL();

        public Usuario usuarioAutenticado(Usuario usuario)
        {
            var usuarioAutendicado = new Usuario();

            mySQL.LimparParametros();

            mySQL.AdicionarParametro("varUsuario", usuario.nome);
            mySQL.AdicionarParametro("varSenha", usuario.senha);

            DataTable dataTable = mySQL.Consultar(CommandType.StoredProcedure, "UsuarioAutenticacao");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                usuarioAutendicado.id = Convert.ToInt32(dataRow["IdUsuario"]);
                usuarioAutendicado.nome = Convert.ToString(dataRow["Nome"]);
                usuarioAutendicado.re = Convert.ToString(dataRow["RE"]);
                usuarioAutendicado.perfil = Convert.ToString(dataRow["Perfil"]);
            }

            return usuarioAutendicado;
        }
    }
}
