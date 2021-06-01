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
                    var isDeleted = Convert.ToBoolean(reader["IsDeleted"]);
                  
                        agents.Add(new Agent
                        {
                            AgentCode = agentCode,
                            AgentName = agentName,
                            WorkingArea = workingArea,
                            Commission = commission,
                            PhoneNo = phoneNo,
                            IsDeleted = isDeleted
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

        public string newAgentCode()
        {
            var agents = AllAgentData();
            string latestCode =  agents[agents.Count - 1].AgentCode;
            string numbersString = latestCode.Substring(1, latestCode.Length - 1);
            int numbers = Convert.ToInt32(numbersString);
            string fmt = "000";
            return (numbers+1).ToString(fmt);
        }

        public void CreateNewAgent(Agent agent)
        {
            var connString = _configuration.GetConnectionString("default");
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();

                var cmd = new SqlCommand();

                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "Insert INTO Agents (AgentCode, AgentName, WorkingArea, Commission, PhoneNo) VALUES (@agentCode, @agentName, @workingArea, @commission, @phoneNo)";
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@agentCode", Value = agent.AgentCode, SqlDbType = System.Data.SqlDbType.Char });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@agentName", Value = agent.AgentName, SqlDbType = System.Data.SqlDbType.VarChar });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@workingArea", Value = agent.WorkingArea, SqlDbType = System.Data.SqlDbType.VarChar });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "commission", Value = agent.Commission, SqlDbType = System.Data.SqlDbType.Decimal });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@phoneNo", Value = agent.PhoneNo, SqlDbType = System.Data.SqlDbType.Char });
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();

            }
        }

        public List<Agent> VisibleAgents(List<Agent> agentList)
        {
            List<Agent> visible = new List<Agent>();
            foreach(var agent in agentList)
            {
                if(agent.IsDeleted == false)
                {
                    visible.Add(agent);
                }
            }
            return visible;
        }

        public void HideAgent(string agentCode)
        {
            var connString = _configuration.GetConnectionString("default");
            using(var conn = new SqlConnection(connString))
            {
                conn.Open();

                var cmd = new SqlCommand();

                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "UPDATE Agents SET IsDeleted = 1 WHERE AgentCode = @agentCode";
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@agentCode", Value = agentCode , SqlDbType = System.Data.SqlDbType.Char});
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
