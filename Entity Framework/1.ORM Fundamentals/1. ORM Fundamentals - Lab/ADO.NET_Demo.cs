using Microsoft.Data.SqlClient;

namespace EF_Demo
{
    internal class Program
    {
        static  void Main(string[] args)
        {

            //ADO.NET
            //1.Connection
            //2.Command
            //3.Reader

            //Connection string

            //Interated security = true enables using the current logged user, without user and passowrd
            string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=SoftUni;Trusted_Connection=True;TrustServerCertificate = true";

            //Without Integrated security with password and user
            //string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=SoftUni;User Id=VkaraiUser;Password=VkaraiParola;Trusted_Connection=True;";

            //Sql query. @departmentId is a parameter
            string query = "SELECT EmployeeId,FirstName,LastName,JobTitle,Salary FROM Employees WHERE DepartmentId  = @departmentId ORDER BY FirstName";

            int departmentId = 7;

            //We start to use a new SqlConnection. We have to write Using in the beginning
            //Using implements IDisposable and closes the connection automatically
            using SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Create a new command and pass the sql query, and the connection with the connection address insie
            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            //Through the command's parameters we use AddWithValue method, passing the parameter name "@departmentId" and and the object value departmentId = 7
            //Also protects from SQL injections 
            cmd.Parameters.AddWithValue("@departmentId", departmentId);


            //Use try/catch block

            try
            {   
                //Open the connection
                sqlConnection.Open();

                //Create reader, who reads the DB through the connection
                SqlDataReader reader = cmd.ExecuteReader();

                //SqlDataReader reader = await cmd.ExecuteReaderAsync();

                //When the result of reader.Read() is true, we continue to read the database
                while (reader.Read()) //while reader.Read() is true
                {
                    //FirstName(1),LastName(2),JobTitle(3), Salary(4)
                    Console.WriteLine($" {reader[1]} {reader[2]}: {reader[3]} - {reader[4]}");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message); 
            }
        }
    }
}
