using Employee_Management.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace Employee_Management.employerepository
{
    public class Emrepository
    {

        private readonly SqlConnection _connection;

        public Emrepository()
        {
            _connection = new SqlConnection("server=.; database=shadia_db; integrated security=true; TrustServerCertificate=True;");
        }

        public List<Employee> GetAll()
        {
            List<Employee> list = new List<Employee>();
            using (_connection)
            {
                _connection.Open();
                string query = "SELECT * FROM Employees ORDER BY Name ASC";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Employee
                            {
                                Id = Convert.ToInt32(reader["ID"]),
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Department = reader["Department"].ToString(),
                                Salary = Convert.ToDecimal(reader["Salary"])
                            });
                        }
                    }
                }
            }
            return list;
        }

        public Employee GetById(int id)
        {
            Employee data = null;
            using (_connection)
            {
                _connection.Open();
                string query = $"SELECT * FROM Employees WHERE ID={id}";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            data = new Employee
                            {
                                Id = Convert.ToInt32(reader["ID"]),
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Department = reader["Department"].ToString(),
                                Salary = Convert.ToDecimal(reader["Salary"])
                            };
                        }
                    }
                }
            }
            return data;
        }

        public bool Create(Employee model)
        {
            using (_connection)
            {
                _connection.Open();
                string query = $"INSERT INTO Employees (Name, Email, Department, Salary) VALUES ('{model.Name}', '{model.Email}', '{model.Department}', {model.Salary})";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    int count = command.ExecuteNonQuery();
                    return count > 0;
                }
            }
        }

        public bool Update(Employee model)
        {
            using (_connection)
            {
                _connection.Open();
                string query = $"UPDATE Employees SET Name='{model.Name}', Email='{model.Email}', Department='{model.Department}', Salary={model.Salary} WHERE ID={model.Id}";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    int count = command.ExecuteNonQuery();
                    return count > 0;
                }
            }
        }

        public bool Delete(int id)
        {
            using (_connection)
            {
                _connection.Open();
                string query = $"DELETE FROM Employees WHERE ID={id}";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    int count = command.ExecuteNonQuery();
                    return count > 0;
                }
            }
        }
    }
}

