using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WsWeb.Models
{
    //Nomeando a tabela como Paciente e colocando as propriedades
    [Table("Paciente")]
    public class Paciente
    {
        //O Key é utilizado com o intuito de dizer que essa propriedade é uma Primary Key
        [Key]
        public int Id { get; set; }
        //O Required é utilizado para que o campo seja preenchido, ou seja, não permite ser nulo. Caso o Usuário deixe nulo irá retornar essa mensagem
        [Required(ErrorMessage = "Por favor digite o Nome do Paciente!")]
        //O Display é a personalização do Label no Front-end
        [Display(Name = "Nome do Paciente:")]
        public string NomePaciente { get; set; }

        //O ForeignKey é utilizado com o intuito de dizer que essa propriedade é uma Foreign Key
        [ForeignKey("Medico")]
        [Required(ErrorMessage = "Por favor digite o Nome do Médico!")]
        [Display(Name = "Nome do Médico:")]
        public int MedicoId { get; set; }
        //Propriedade de Navegação para a tabela Medico
        public Medico Medico { get; set; }

        [Required(ErrorMessage = "Por favor digite o Dia da Consulta!")]
        [Display(Name = "Dia da Paciente:")]
        public string Dia { get; set; }

        [Required(ErrorMessage = "Por favor digite o Horário da Consulta!")]
        [Display(Name = "Horário da Consulta:")]
        public string Horario { get; set; }
    }
}
