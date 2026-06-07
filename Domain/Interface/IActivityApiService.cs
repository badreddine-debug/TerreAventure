using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerreAventure.Domain.Interface
{
    public interface IActivityApiService
    {
        Task<ActivityResponse?> FetchActivitiesAsync(string url);
    }
}
