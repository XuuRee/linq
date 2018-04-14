using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using PV178.Homeworks.HW04.DataLoading.Mappers;
using PV178.Homeworks.HW04.Model;

namespace PV178.Homeworks.HW04.DataLoading.DataImporter
{
    public class DataImporter : IDataImporter
    {
        #region csv_file_paths

        private static readonly string DataPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\PV178.Homeworks.HW04\DataLoading\Data\"));

        private static readonly string SharkAttacksCsvFilePath = DataPath + "WorldwideSharkAttacks.csv";

        private static readonly string CountriesCsvFilePath = DataPath + "Countries.csv";

        private static readonly string SharkSpeciesCsvFilePath = DataPath + "SharkSpecies.csv";

        private static readonly string AttackedPeopleCsvFilePath = DataPath + "AttackedPeople.csv";

        #endregion

        private readonly IList<SharkAttack> sharkAttacks;

        private readonly IList<AttackedPerson> attackedPeople;

        private readonly IList<SharkSpecies> sharkSpecies;

        private readonly IList<Country> countries;

        public DataImporter()
        {
            sharkAttacks = ImportFromCsv<SharkAttack>(SharkAttacksCsvFilePath);
            attackedPeople = ImportFromCsv<AttackedPerson>(AttackedPeopleCsvFilePath);
            sharkSpecies = ImportFromCsv<SharkSpecies>(SharkSpeciesCsvFilePath);
            countries = ImportFromCsv<Country>(CountriesCsvFilePath);
        }
   
        public IReadOnlyList<SharkAttack> ListAllSharkAttacks()
        {
            return new ReadOnlyCollection<SharkAttack>(sharkAttacks);
        }

        public IReadOnlyList<AttackedPerson> ListAllAttackedPeople()
        {
            return new ReadOnlyCollection<AttackedPerson>(attackedPeople);
        }

        public ReadOnlyCollection<SharkSpecies> ListAllSharkSpecies()
        {
            return new ReadOnlyCollection<SharkSpecies>(sharkSpecies);
        }

        public ReadOnlyCollection<Country> ListAllCountries()
        {
            return new ReadOnlyCollection<Country>(countries);
        }

        private IList<T> ImportFromCsv<T>(string filepath) where T : new()
        {
            IList<T> items;
            using (TextReader reader = File.OpenText(filepath))
            {
                using (var csv = new CsvReader(reader, new CsvConfiguration { HasHeaderRecord = true, Delimiter = ";"}))
                {
                    csv.Configuration.RegisterClassMap<SharkMapper>();
                    csv.Configuration.RegisterClassMap<CountryMapper>();
                    items = csv.GetRecords<T>().ToList();
                }
            }
            return items;
        }
    }
}
