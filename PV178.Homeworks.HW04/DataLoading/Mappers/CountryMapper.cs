using System.Globalization;
using CsvHelper.Configuration;
using PV178.Homeworks.HW04.Model;
using System.Collections.Generic;

namespace PV178.Homeworks.HW04.DataLoading.Mappers
{
    public sealed class CountryMapper : CsvClassMap<Country>
    {

        public CountryMapper()
        {
            var countryMappings = new List<CsvPropertyMap> {
                Map(m => m.Id),
                Map(m => m.Name),
                Map(m => m.Area),
                Map(m => m.Continent),
                Map(m => m.CountryCode),
                Map(m => m.Currency),
                Map(m => m.CurrencyCode),
                Map(m => m.GovernmentForm),
                Map(m => m.Population),
                Map(m => m.Birthrate),
                Map(m => m.Deathrate),
                Map(m => m.LifeExpectancy)
            };
            countryMappings.ForEach(c => c.TypeConverterOption(CultureInfo.InvariantCulture));
        }
    }
}
