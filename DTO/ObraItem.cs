using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class ObraItem
    {
        public int IdObra { get; set; }
        public string numObra { get; set; }
        public string contrato { get; set; }
        public string codigo { get; set; }
        public string descricao { get; set; }
        public string tipo { get; set; }
        public string MO_classe { get; set; }
        public decimal MO_ponto { get; set; }
        public decimal MO_baremo { get; set; }
        public decimal MO_itemOPT { get; set; }
        public decimal MO_itemIND { get; set; }
        public decimal MO_valorOPT { get; set; }
        public decimal MO_valorIND { get; set; }
        public decimal MT_valor { get; set; }

        [Required(ErrorMessage = "OBRIGATÓRIO")]
        public decimal MT_itemLMN { get; set; }

        [Required(ErrorMessage = "OBRIGATÓRIO")]
        public decimal MT_itemOPT { get; set; }

        [Required(ErrorMessage = "OBRIGATÓRIO")]
        public decimal MT_itemIND { get; set; }

        public decimal MT_itemDIF { get; set; }
        public decimal MT_valorLMN { get; set; }
        public decimal MT_valorOPT { get; set; }
        public decimal MT_valorIND { get; set; }
        public decimal MT_valorDIF { get; set; }
    }
}
