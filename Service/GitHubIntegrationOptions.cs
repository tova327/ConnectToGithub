﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class GitHubIntegrationOptions
    {
        public GitHubIntegrationOptions() { }
        public string Token { get; set; }
        public string UserName { get; set; }    
    }
}
