using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace GenealogyWebAPI.ClientSdk
{
    public partial class GenealogyAPI
    {
        public GenealogyAPI(HttpClient client) : base(client, false)
        {
            Initialize();
            BaseUri = client.BaseAddress;
        }
    }
}
