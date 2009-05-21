namespace bombali.infrastructure.app.monitorchecks
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using logging;

    internal class SqlDatabaseCheck : ICheck
    {
        IList<String> okay_responses;
        double failure_count=0d;

        public SqlDatabaseCheck()
        {
            build_list_of_okay_responses();
        }

        public void build_list_of_okay_responses()
        {
            okay_responses = new List<String>
                                 {
                                     "Success"
                                 };
        }

        public string last_response { get; private set; }

        public bool run_check(string what_to_check)
        {
            bool successful_check = true;

            string response_code = connect_to_database(what_to_check);

            last_response = response_code;

            if(okay_responses.Contains(response_code))
            {
                failure_count = 0;
                Log.bound_to(this).Info("{0} was able to successfully reach {1}. Response code was {2}.", ApplicationParameters.name,
                                        what_to_check, response_code);
            }
            else
            {
                successful_check = false;
                failure_count += 1;
                Log.bound_to(this).Warn(
                    "{0} warning! {1} is unreachable. Response code was {2}. This has happened {3} times.",
                    ApplicationParameters.name, what_to_check, response_code, failure_count);
            }

            return successful_check;
        }

        string connect_to_database(string sql_server_connection_string)
        {
            Log.bound_to(this).Debug("{0} is checking {1}.", ApplicationParameters.name, sql_server_connection_string);
            using (SqlConnection sql_connection = new SqlConnection(sql_server_connection_string))
            {
                try
                {
                    sql_connection.Open();
                    return "Success";
                }
                catch (Exception)
                {
                    return "Timeout";
                }
                finally
                {
                    if (sql_connection.State == ConnectionState.Open)
                    {
                        sql_connection.Close();
                    }
                }
            }
        }
    }
}