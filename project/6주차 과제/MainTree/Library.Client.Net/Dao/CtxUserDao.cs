﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Client.Net.Dao
{
    public class CtxUserDao
    {
        public string SelectUserContext(int userSerial)
        {
            return "select * from user_context where user_serial = " + userSerial;
        }
    }
}
