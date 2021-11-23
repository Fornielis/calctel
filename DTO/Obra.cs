using System;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class Obra
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "OBRIGATÓRIO")]
        public string numObra { get; set; }
        public string numObraChecagem { get; set; }

        [Required(ErrorMessage = "OBRIGATÓRIO")]
        public string contrato { get; set; }
        public string contratoChecagem { get; set; }

        [Required(ErrorMessage = "OBRIGATÓRIO")]
        public decimal ValorMaoObra { get; set; }
        public string Estatus { get; set; }
        public string RE { get; set; }
        public string Tecnico { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime DataCriacao { get; set; }
        public decimal Valor_MaoObra { get; set; }
        public decimal Valor_Material { get; set; }
        public decimal Valor_Total_Contratada { get; set; }
        public decimal Valor_Material_OPT { get; set; }
        public decimal Valor_Material_IND { get; set; }
        public decimal Valor_Material_Impacto { get; set; }
        public decimal Valor_MaoObra_OPT { get; set; }
        public decimal Valor_MaoObra_IND { get; set; }
        public decimal Valor_MaoObra_Impacto { get; set; }
        public decimal Valor_Total_Impacto { get; set; }
        public decimal Percentual_Material_Impacto { get; set; }
        public decimal Percentual_MaoObra_Impacto { get; set; }
        public decimal Percentual_Total_Impacto { get; set; }
        public decimal Valor_MaoObra_Fiscalizacao { get; set; }
        public decimal Valor_Material_Fiscalizacao { get; set; }
        public decimal Valor_Total_Fiscaizacao { get; set; }
    }
}
