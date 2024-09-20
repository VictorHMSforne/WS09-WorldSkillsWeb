using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WsWeb.Models
{
    //Nomeando a tabela como Medico e colocando as propriedades
    [Table("Medico")]
    public class Medico
    {
        //O Key é utilizado com o intuito de dizer que essa propriedade é uma Primary Key
        [Key]
        public int Id { get; set; }
        //O Required é utilizado para que o campo seja preenchido, ou seja, não permite ser nulo. Caso o Usuário deixe nulo irá retornar essa mensagem
        [Required(ErrorMessage = "Por favor digite o nome do Médico!")]
        //O Display é a personalização do Label no Front-end
        [Display(Name = "Nome do Médico:")]
        public string NomeMedico { get; set; }

        [Required(ErrorMessage = "Por favor digite a especialidade do Médico!")]
        [Display(Name = "Especialiade do Médico:")]
        public string Especialidade { get; set; }

        //Propriedade de Navegação para a Tabela Paciente
        public ICollection<Paciente> Pacientes { get; set; }

    }
}
