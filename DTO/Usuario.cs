using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class Usuario
    {
        public int id { get; set; }

        [Required(ErrorMessage = "OBRIGATÓRIO")]
        public string nome { get; set; }

        [Required(ErrorMessage = "OBRIGATÓRIO")]
        public string senha { get; set; }
        public string re { get; set; }
        public string perfil { get; set; }
    }
}
