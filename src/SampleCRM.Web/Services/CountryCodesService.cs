using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using SampleCRM.Web.Attributes;
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
        [RestrictAccessDeveloperMode]
        public void DeleteCountry(CountryCodes country)
        {
            _context.CountryCodes.Remove(country);
        }

        [Insert]
        [RestrictAccessDeveloperMode]
        public void InsertCountry(CountryCodes country)
        {
            _context.CountryCodes.AddOrUpdate(country);
        }

        [Update]
        [RestrictAccessDeveloperMode]
        public void UpdateCountry(CountryCodes country)
        {
            _context.CountryCodes.AddOrUpdate(country);
        }
    }
}