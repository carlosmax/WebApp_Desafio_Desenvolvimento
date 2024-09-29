using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public class DepartamentosDAL : BaseDAL
    {
        public IEnumerable<Departamento> ListarDepartamentos()
        {
            IList<Departamento> lstDepartamentos = new List<Departamento>();

            using (SQLiteConnection dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                using (SQLiteCommand dbCommand = dbConnection.CreateCommand())
                {

                    dbCommand.CommandText = "SELECT * FROM departamentos ";

                    dbConnection.Open();

                    using (SQLiteDataReader dataReader = dbCommand.ExecuteReader())
                    {
                        var departamento = Departamento.Empty;

                        while (dataReader.Read())
                        {
                            departamento = new Departamento();

                            if (!dataReader.IsDBNull(0))
                                departamento.ID = dataReader.GetInt32(0);
                            if (!dataReader.IsDBNull(1))
                                departamento.Descricao = dataReader.GetString(1);

                            lstDepartamentos.Add(departamento);
                        }
                        dataReader.Close();
                    }
                    dbConnection.Close();
                }

            }

            return lstDepartamentos;
        }

        public Departamento ObterDepartamento(int idDepartamento)
        {
            Departamento departamento = null;

            using (SQLiteConnection dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                using (SQLiteCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = $"SELECT * FROM departamentos WHERE ID = {idDepartamento}";
                    dbConnection.Open();

                    using (SQLiteDataReader dataReader = dbCommand.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            departamento = new Departamento
                            {
                                ID = dataReader.GetInt32(0),
                                Descricao = dataReader.GetString(1)
                            };
                        }
                        dataReader.Close();
                    }
                    dbConnection.Close();
                }
            }

            return departamento;
        }

        public bool GravarDepartamento(int id, string descricao)
        {
            using (SQLiteConnection dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                using (SQLiteCommand dbCommand = dbConnection.CreateCommand())
                {
                    if (id == 0)
                    {
                        dbCommand.CommandText = "INSERT INTO departamentos (Descricao) VALUES (@Descricao)";
                    }
                    else
                    {
                        dbCommand.CommandText = "UPDATE departamentos SET Descricao = @Descricao WHERE ID = @ID";
                        dbCommand.Parameters.AddWithValue("@ID", id);
                    }

                    dbCommand.Parameters.AddWithValue("@Descricao", descricao);
                    dbConnection.Open();

                    int rowsAffected = dbCommand.ExecuteNonQuery();
                    dbConnection.Close();

                    return rowsAffected > 0;
                }
            }
        }

        public bool ExcluirDepartamento(int idDepartamento)
        {
            using (SQLiteConnection dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                using (SQLiteCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = $"DELETE FROM departamentos WHERE ID = {idDepartamento}";
                    dbConnection.Open();

                    int rowsAffected = dbCommand.ExecuteNonQuery();
                    dbConnection.Close();

                    return rowsAffected > 0;
                }
            }
        }
    }
}