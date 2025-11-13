using System;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Reflection.Metadata;
using System.Text;

namespace testCase
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            PrintMenu();
            int option_number = GetOptionNumber();
            SelectOption(option_number);
        }

        static void ExecuteQueryWithoutReturnData(string sql, Dictionary<string, object>? parameters = null)
        {
            string server = @"DESKTOP-6ES6KUP\SQLEXPRESS";
            string data_base = "EmployeeDB";
            string trusted_connection = "True";
            string trust_server_certificate = "True";

            string connection_string = $"Server={server};Database={data_base};Trusted_Connection={trusted_connection};TrustServerCertificate={trust_server_certificate};";

            try
            {
                using (SqlConnection conn = new SqlConnection(connection_string))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                cmd.Parameters.AddWithValue(param.Key, param.Value);
                            }
                        }
                        cmd.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Операция успешна");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static List<Dictionary<string, object>> ExecuteQueryWithReturnData(string sql_string, Dictionary<string, object>? parameters = null)
        {
            string server = @"DESKTOP-6ES6KUP\SQLEXPRESS";
            string data_base = "EmployeeDB";
            string trusted_connection = "True";
            string trust_server_certificate = "True";

            string connection_string = $"Server={server};Database={data_base};Trusted_Connection={trusted_connection};TrustServerCertificate={trust_server_certificate};";
            var result = new List<Dictionary<string, object>>();
            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql_string, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.GetValue(i);
                            }
                            result.Add(row);
                        }
                    }
                }
                return result;
            }
        }

        static void PrintMenu()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("1. Добавить нового сотрудника.");
            Console.WriteLine("2. Посмотреть всех сотрудников.");
            Console.WriteLine("3. Обновить информацию о сотруднике.");
            Console.WriteLine("4. Удалить сотрудника.");
            Console.WriteLine("5. Получить зарплату сотрудников.");
            Console.WriteLine("6. Выйти из приложения.");
            Console.WriteLine("========================================");
        }

        static int GetOptionNumber()
        {
            int option_select;
            string console_input = Console.ReadLine() ?? string.Empty;
            bool parse_console_input = int.TryParse(console_input, out option_select);
            if (parse_console_input)
            {
                return option_select;
            }
            else
            {
                return 0;
            }

        }

        static void SelectOption(int option_select)
        {
            switch (option_select)
            {
                case 1: AddEmployee(); PrintMenu(); SelectOption(GetOptionNumber()); break;
                case 2: ShowAllEmployees(); PrintMenu(); SelectOption(GetOptionNumber()); break;
                case 3: UpdateEmployeeInfo(); PrintMenu(); SelectOption(GetOptionNumber()); break;
                case 4: DeleteEmployee(); PrintMenu(); SelectOption(GetOptionNumber()); break;
                case 5: GetEmployeeSalary(); PrintMenu(); SelectOption(GetOptionNumber()); break;
                case 6: ExitApp(); break;
                default: Console.WriteLine("Введена неверная опция"); PrintMenu(); SelectOption(GetOptionNumber()); break;
            }
        }
        static string CheckInputString(string prompt)
        {
            string? input_line;
            do
            {
                Console.Write(prompt);
                input_line = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input_line))
                {
                    Console.WriteLine("Ввод не может быть пустым. Попробуйте снова.");
                }
            } while (string.IsNullOrWhiteSpace(input_line));
            return input_line;
        }

        static string CheckValidEmail(string prompt)
        {
            string? input_line;
            Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
            do
            {
                Console.Write(prompt);
                input_line = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input_line))
                {
                    Console.WriteLine("Email не может быть пустым. Попробуйте снова.");
                }
                else if (!emailRegex.IsMatch(input_line))
                {
                    Console.WriteLine("Некорректный формат email. Попробуйте снова.");
                    input_line = null;
                }

            } while (string.IsNullOrWhiteSpace(input_line));

            return input_line;
        }

        static DateOnly CheckValidBirthDate(string prompt)
        {
            string? input_line;
            DateOnly birth_date;
            bool is_date_correct;

            do
            {
                Console.Write(prompt);
                input_line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input_line))
                {
                    Console.WriteLine("Дата рождения не может быть пустой. Попробуйте снова.");
                    is_date_correct = false;
                }
                else
                {
                    is_date_correct = true;
                }
                if (!DateOnly.TryParse(input_line, out birth_date))
                {
                    Console.WriteLine("Ошибка: введена некорректная дата рождения.");
                    is_date_correct = false;
                }
                else
                {
                    is_date_correct = true;
                }


            }
            while (is_date_correct == false);
            return birth_date;
        }

        static int CheckValidInt(string prompt)
        {
            string? input_line;
            int salary;
            bool is_salary_correct;

            do
            {
                Console.Write(prompt);
                input_line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input_line))
                {
                    Console.WriteLine("Значение не может быть пустым. Попробуйте снова.");
                    is_salary_correct = false;
                }
                else
                {
                    is_salary_correct = true;
                }
                if (!int.TryParse(input_line, out salary))
                {
                    Console.WriteLine("Ошибка: введено некорректное значение.");
                    is_salary_correct = false;
                }
                else
                {
                    is_salary_correct = true;
                }


            }

            while (is_salary_correct == false);
            return salary;
        }


        static void AddEmployee()
        {
            string employee_first_name;
            string employee_last_name;
            string employee_email;
            DateOnly employee_birth_date;
            int employee_salary;
            employee_first_name = CheckInputString("Введите имя сотрудника: ");
            employee_last_name = CheckInputString("Введите фамилию сотрудника: ");
            employee_email = CheckValidEmail("Введите корректный email сотрудника: ");
            employee_birth_date = CheckValidBirthDate("Введите корректную дату рождения сотрудника (YYYY-MM-DD): ");
            employee_salary = CheckValidInt("Введите корректную зарплату сотрудника: ");
            if (employee_salary < 0)
            {
                employee_salary *= -1;
            }

            string sql_command = "INSERT INTO Employees (FirstName, LastName, Email, DateOfBirth, Salary) VALUES (@first_name, @last_name, @email, @birth_date, @salary)";
            var parameters = new Dictionary<string, object>
            {
                {"@first_name", employee_first_name},
                {"@last_name", employee_last_name},
                {"@email", employee_email},
                {"@birth_date", employee_birth_date},
                {"@salary", employee_salary}
            };
            ExecuteQueryWithoutReturnData(sql_command, parameters);
        }

        static void ShowAllEmployees()
        {
            string sql_command = "SELECT * FROM Employees";
            var employees = ExecuteQueryWithReturnData(sql_command);
            foreach (var entry in employees)
            {

                foreach (var emp in entry)
                {
                    Console.WriteLine($"{emp.Key}: {emp.Value}");
                }
                Console.WriteLine("========================================");
            }
        }

        static void UpdateEmployeeInfo()
        {
            int employee_id;
            string? employee_first_name = null;
            string? employee_last_name = null;
            string? employee_email = null;
            DateOnly employee_birth_date;
            int employee_salary;
            var updates = new List<string>();
            var parameters = new Dictionary<string, object>();

            employee_id = CheckValidInt("Введите id сотрудника: ");
            Console.WriteLine("Вводите только те значения, которые хотите изменить");
            Console.Write("Введите имя сотрудника: ");
            employee_first_name = Console.ReadLine();
            Console.Write("Введите фамилию сотрудника: ");
            employee_last_name = Console.ReadLine();
            Console.Write("Введите email сотрудника: ");
            employee_email = Console.ReadLine();
            Console.Write("Введите корректную дату рождения сотрудника (YYYY-MM-DD): ");
            bool parse_employee_birth_date = DateOnly.TryParse(Console.ReadLine(), out employee_birth_date);
            Console.Write("Введите зарплату сотрудника: ");
            bool parse_employee_salary = int.TryParse(Console.ReadLine(), out employee_salary);

            if (!string.IsNullOrWhiteSpace(employee_first_name))
            {
                updates.Add("FirstName = @first_name");
                parameters["@first_name"] = employee_first_name;
            }
            if (!string.IsNullOrWhiteSpace(employee_last_name))
            {
                updates.Add("LastName = @last_name");
                parameters["@last_name"] = employee_last_name;
            }
            if (!string.IsNullOrWhiteSpace(employee_email))
            {
                updates.Add("Email = @email");
                parameters["@email"] = employee_email;
            }
            if (parse_employee_birth_date)
            {
                updates.Add("DateOfBirth = @birth_date");
                parameters["@birth_date"] = employee_birth_date;
            }
            if (parse_employee_salary)
            {
                updates.Add("Salary = @salary");
                parameters["@salary"] = employee_salary;
                if (employee_salary < 0)
                {
                    employee_salary *= -1;
                    parameters["@salary"] = employee_salary;
                }
            }

            if (updates.Count > 0)
            {
                string sql_command = $"UPDATE Employees SET {string.Join(", ", updates)} WHERE EmployeeID = @id";
                parameters["@id"] = employee_id;
                ExecuteQueryWithoutReturnData(sql_command, parameters);
            }
            else
            {
                Console.WriteLine("Нет корректных данных для обновления");
            }
        }

        static void DeleteEmployee()
        {
            int id;
            Console.Write("Введите id сотрудника: ");
            var parse_id = int.TryParse(Console.ReadLine(), out id);
            string sql_command = "DELETE FROM Employees WHERE EmployeeID = @EmployeeID";
            ExecuteQueryWithoutReturnData(sql_command, new Dictionary<string, object> { { "@EmployeeID", id } });
            Console.WriteLine("Удалил сотрудника");
        }

        static void GetEmployeeSalary()
        {
            string sql_command = @"
                        SELECT COUNT(*) AS EmployeeCount
                        FROM Employees
                        WHERE Salary > (SELECT AVG(Salary) FROM Employees)";

            var result = ExecuteQueryWithReturnData(sql_command);

            if (result.Count > 0 && result[0].TryGetValue("EmployeeCount", out object? value))
            {
                Console.WriteLine($"Количество сотрудников с зарплатой выше средней: {value}");
            }
            else
            {
                Console.WriteLine("Не удалось получить данные.");
            }
        }

        static void ExitApp()
        {
            Console.WriteLine("Вышел из приложения");
        }

    }
}