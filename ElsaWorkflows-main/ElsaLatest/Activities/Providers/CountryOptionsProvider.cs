using Elsa.Design;
using Elsa.Metadata;
using Elsa.Samples.UserRegistration.Web.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Samples.UserRegistration.Web.Activities.Providers
{
    public class CountryOptionsProvider : IActivityPropertyOptionsProvider, IRuntimeSelectListItemsProvider
    {
        private readonly SampleDbContext _store;

        public CountryOptionsProvider(SampleDbContext store)
        {
            _store = store;
        }

        public ValueTask<IEnumerable<SelectListItem>> GetItemsAsync(object context = null, CancellationToken cancellationToken = default)
        {
            var countries = new[] { "India", "UK", "USA" };
            var items = countries.Select(x => new SelectListItem(x)).ToList();
            return new ValueTask<IEnumerable<SelectListItem>>(items);
        }

        public object GetOptions(PropertyInfo property)
        {
            return new RuntimeSelectListItemsProviderSettings(typeof(CountryOptionsProvider));
        }
    }
}
