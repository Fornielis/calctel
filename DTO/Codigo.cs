using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class Codigo
    {
        [Required(ErrorMessage = "OBRIGATÓRIO")]
        public string cod { get; set; }
        public string descricao { get; set; }
        public string tipo { get; set; }
        public string MO_classe { get; set; }
        public decimal MO_ponto { get; set; }
        public decimal MO_baremo { get; set; }
        public decimal MT_valor { get; set; }
    }
}
