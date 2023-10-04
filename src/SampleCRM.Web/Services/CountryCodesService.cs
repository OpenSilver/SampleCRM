using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using SampleCRM.Web.Models;
using System.Data.Entity.Migrations;
using System.Linq;

namespace SampleCRM.Web
{
    [EnableClientAccess]
    public class CountryCodesService : SampleCRMService
    {
        [Query]
        public IQueryable<CountryCodes> GetCountries()
        {
            return _context.CountryCodes;
        }

        public CountryCodes GetCountryById(string countryCodeID)
        {
            return _context.CountryCodes.SingleOrDefault(x => x.CountryCodeID == countryCodeID);
        }

        [Delete]
        public void DeleteCountry(CountryCodes country)
        {
            _context.CountryCodes.Remove(country);
        }

        [Insert]
        public void InsertCountry(CountryCodes country)
        {
            _context.CountryCodes.AddOrUpdate(country);
        }

        [Update]
        public void UpdateCountry(CountryCodes country)
        {
            _context.CountryCodes.AddOrUpdate(country);
        }
    }
}