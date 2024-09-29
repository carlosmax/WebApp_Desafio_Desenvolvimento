namespace WebApp_Desafio_API.ViewModels
{
    /// <summary>
    /// Solicitação do departamento
    /// </summary>
    public class DepartamentoRequest
    {
        /// <summary>
        /// ID do Chamado
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Descrição do Departamento
        /// </summary>
        public string descricao { get; set; }
    }
}
