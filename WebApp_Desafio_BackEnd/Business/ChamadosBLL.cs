using System;
using System.Collections.Generic;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.Business
{
    public class ChamadosBLL
    {
        private ChamadosDAL dal = new ChamadosDAL();

        public IEnumerable<Chamado> ListarChamados()
        {
            return dal.ListarChamados();
        }

        public Chamado ObterChamado(int idChamado)
        {
            return dal.ObterChamado(idChamado);
        }

        public bool GravarChamado(int ID, string Assunto, string Solicitante, int IdDepartamento, DateTime DataAbertura)
        {
            if (string.IsNullOrWhiteSpace(Assunto))
            {
                throw new ArgumentException("O assunto não pode estar vazio.");
            }

            if (Assunto.Length > 100)
            {
                throw new ArgumentException("O assunto não pode ter mais de 100 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(Solicitante))
            {
                throw new ArgumentException("O solicitante não pode estar vazio.");
            }

            if (Solicitante.Length > 50)
            {
                throw new ArgumentException("O solicitante não pode ter mais de 50 caracteres.");
            }

            if (IdDepartamento <= 0)
            {
                throw new ArgumentException("Informe o departamento.");
            }

            // Não permite data retroativa no cadastro de chamados
            if (ID == 0 && DataAbertura < DateTime.Now.Date)
            {
                throw new ArgumentException("A data não pode ser retroativa.");
            }

            return dal.GravarChamado(ID, Assunto, Solicitante, IdDepartamento, DataAbertura);
        }

        public bool ExcluirChamado(int idChamado)
        {
            return dal.ExcluirChamado(idChamado);
        }

        public IEnumerable<string> ObterSolicitantes(string termo)
        {
            return dal.ObterSolicitantes(termo);
        }
    }
}
