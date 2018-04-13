using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenealogyWebAPI.Proxies
{
    // https://api.genderize.io/?name=igor&country_id=ua
    // https://genderize.io/

    [Headers("User-Agent: Genderize IO WebAPI Client 1.0")]
    public interface IGenderizeClient
    {
        [Get("/")]
        Task<string> GetGenderForName(string name);
    }
}
