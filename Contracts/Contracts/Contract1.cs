﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Contracts
{
    public class Contract1 :IContract
    {
        public string Message { get; set; }
        public int Value { get; set; }
    }
}
