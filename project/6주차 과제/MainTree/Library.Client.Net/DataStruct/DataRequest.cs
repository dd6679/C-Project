using Library.Client.Net.Constance;

namespace Library.Client.Net.DataStruct
{
    class DataRequest
    {
        public DataRequest(string sql, object param, DatabaseCommands cmd, DataQryTypes qryType)
        {
            this.sql = sql;
            this.param = param;
            this.cmd = cmd.ToString();
            this.type = qryType.ToString();
            this.tran = TranIdGenerator.NewId;
            //db = db;
        }
        public string sql { get; set; }   // 질의문
        public object param { get; set; } // 인자(질의문 포맷팅할때 사용)
        public string cmd { get; set; }   // 질의 커맨드
        public string type { get; set; }  // 질의형식(일반, 디스트, 스토어드프로시저)
        public string db { get; set; }    // 데이터 베이스 이름
        public long tran { get; set; }    // 트랜(비동기 처리시 사용)

    }
}
