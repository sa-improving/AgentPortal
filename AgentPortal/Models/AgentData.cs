using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AgentPortal.Models
{
    public class AgentData
    {
        private readonly IConfiguration _configuration;

        public AgentData(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Agent> AllAgentData()
        {
            var agents = new List<Agent>();

            var connString = _configuration.GetConnectionString("default");

            using (var conn = new SqlConnection(connString))
            {
                conn.Open();

                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "Select * FROM Agents";

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var agentCode = reader["AgentCode"].ToString();
                    var agentName = reader["AgentName"].ToString();
                    var workingArea = reader["WorkingArea"].ToString();
                    var commission = Convert.ToDouble(reader["Commission"]);
                    var phoneNo = reader["PhoneNo"].ToString();

                    agents.Add(new Agent
                    {
                        AgentCode = agentCode,
                        AgentName = agentName,
                        WorkingArea = workingArea,
                        Commission = commission,
                        PhoneNo = phoneNo
                    });

                }
            }
            return agents;
        }

        public Agent GetAgentByAgentCode(string agentCode) {
            var connString = _configuration.GetConnectionString("default");
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();

                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@agentCode", Value = agentCode, SqlDbType = System.Data.SqlDbType.Char });
                cmd.CommandText = "SELECT * FROM Agents WHERE AgentCode = @agentCode";

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                    {
                    var agentCodeQuery = reader["AgentCode"].ToString();
                    var agentName = reader["AgentName"].ToString();
                    var workingArea = reader["workingArea"].ToString();
                    var commission = Convert.ToDouble(reader["Commission"]);
                    var phoneNo = reader["PhoneNo"].ToString();

                    return new Agent
                    {
                        AgentCode = agentCodeQuery,
                        AgentName = agentName,
                        WorkingArea = workingArea,
                        Commission = commission,
                        PhoneNo = phoneNo,
                    };
                }
            }
            return null;
        }
    }
}
