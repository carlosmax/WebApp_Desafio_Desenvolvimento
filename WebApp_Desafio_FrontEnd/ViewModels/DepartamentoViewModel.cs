using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Linq;
using WebApp_Desafio_FrontEnd.ViewModels.Interfaces;

namespace WebApp_Desafio_FrontEnd.ViewModels 
{
    [DataContract]
    public class DepartamentoViewModel : IViewModel
    {
        [Display(Name = "ID")]
        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Descrição")]
        [DataMember(Name = "Descricao")]
        [Required(ErrorMessage = "A descrição do departamento é obrigatória.")]
        [StringLength(100, ErrorMessage = "A descrição não pode exceder 100 caracteres.")]
        public string Descricao { get; set; }

    }
}
