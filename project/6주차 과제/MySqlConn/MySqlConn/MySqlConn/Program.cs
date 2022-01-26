using MySqlConnector;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;

namespace MySqlConn
{
    internal class Program 
    {
        static void Main(string[] args)
        {
            string connectString = string.Format("Server={0};Database={1};Uid ={2};Pwd={3};", "172.22.41.201", "vms", "root", "pass");
            //string sql = "select * from tree_node";
            string ctxSerial;
            string[] nodeSerials;
            string sql = "select * from user_context where user_serial = 1";
            //string sql = "select * from tree_node where user_serial = 1 and ctx_serial = " + ctxSerial;
            //string sql = "select * from node_item where user_serial = 1 and ctx_serial = " + ctxSerial + "and node_serial = " + nodeSerials[i];

            using (var conn = new MySqlConnection(connectString))
            {
                // 데이터베이스 오픈
                conn.Open();

                // 질의 실행
                var mySqlDataTable = new DataTable();
                var cmd = new MySqlCommand(sql, conn);
                var dataReader = cmd.ExecuteReader();

                // 데이터 로딩
                mySqlDataTable.Load(dataReader);

                // 데이터 Json 변환
                var colums = new List<string>();
                foreach (DataColumn col in mySqlDataTable.Columns) 
                {
                    colums.Add(col.ColumnName);
                }

                var rows = new JArray();
                foreach (DataRow page in mySqlDataTable.Rows)
                {
                    // 배열에서 객체로 변환
                    var itemArray = JObject.FromObject(page)["ItemArray"]; 
                    var row = new JObject();
                    var i = 0;
                    foreach (var item in itemArray)
                    {
                        if (colums[i] == "ctx_serial")
                        {
                            ctxSerial = (string)item;
                        }
                        row.Add(colums[i++], item);
                    }
                    rows.Add(row);
                }
                // 데이터 객체 닫음.
                dataReader.Close();
                
                // 출력
                Console.WriteLine(rows.ToString());
                Console.ReadKey();
            }
        }
    }
}
