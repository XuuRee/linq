using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PV178.Homeworks.HW04.Model;

namespace PV178.Homeworks.HW04.Tests
{
    [TestClass]
    public class QueriesTest
    {
        private Queries queries;
        public Queries Queries => queries ?? (queries = new Queries());
        
        [TestMethod]
        public void WhiteDeathSurvivorsStartingWithKQueryTest_ReturnsCorrectResult()
        {
            // arrange
            var personNamesExpectedResult = new List<string> { "K. Tracy", "Karl Kuchnow", "Kazuhiho Kato", "Kevin Thompson" };

            // act
            var whiteDeathSurvivorsStartingWithK = Queries.WhiteDeathSurvivorsStartingWithKQuery();

            // assert
            var res = personNamesExpectedResult.SequenceEqual(whiteDeathSurvivorsStartingWithK);
            AssertBoolEqualsTrue(res);
        }

        [TestMethod]
        public void AreAllLongSharksGenderIgnoringQueryTest_ReturnsCorrectResult()
        {
            var areAllLongSharksGenderIgnoring = Queries.AreAllLongSharksGenderIgnoringQuery();

            AssertBoolEqualsTrue(areAllLongSharksGenderIgnoring.Equals(false));
        }
        
        [TestMethod]
        public void SharksWithoutNicknameAttacksQueryTest_ReturnsCorrectResult()
        {
            var expectedResult = new Dictionary<string, int>
            {
                {"Nurse shark", 35},
                {"Spinner shark", 34},
                {"Mako shark", 30},
                {"Carpet shark", 33},
                {"Hammerhead shark", 46},
                {"Dusky shark", 31},
                {"Grey reef shark", 37},
                {"Lemon shark", 27},
                {"Blue shark", 18},
                {"Salmon shark", 17},
                {"Blacktip shark", 19}
            };

            var sharksWithoutNicknameAttacks = Queries.SharksWithoutNicknameAttacksQuery();

            AssertBoolEqualsTrue(sharksWithoutNicknameAttacks.OrderBy(x => x.Key).SequenceEqual(expectedResult.OrderBy(x => x.Key)));
        }

        [TestMethod]
        public void FiveSharkSpeciesWithMostFatalitiesQueryTest_ReturnsCorrectResult()
        {
            // arrange
            var fiveSharkSpeciesWithMostFatalitiesExpectedResult = new Dictionary<string, int>
            {
                {"White shark", 283},
                {"Hammerhead shark", 148},
                {"Sevengill shark", 131},
                {"Bronze whaler shark", 129},
                {"Wobbegong shark", 126}
            };

            // act
            var fiveSharkSpeciesWithMostFatalities = Queries.FiveSharkSpeciesWithMostFatalitiesQuery();

            // assert
            var res = fiveSharkSpeciesWithMostFatalitiesExpectedResult.SequenceEqual(fiveSharkSpeciesWithMostFatalities);
            AssertBoolEqualsTrue(res);
        }
        
        [TestMethod]
        public void GovernmentTypePercentagesQueryTest_ReturnsCorrectResult()
        {
            // arrange
            var expectedMessage = "Republic: 59,9%, Monarchy: 18,6%, Territory: 15,8%, AutonomousRegion: 2,0%, ParliamentaryDemocracy: 1,6%, AdministrativeRegion: 0,8%, OverseasCommunity: 0,8%, Federation: 0,4%";

            // act
            var governmentTypePercentages = Queries.GovernmentTypePercentagesQuery().Replace('.', ',');

            // assert
            Assert.AreEqual(expectedMessage, governmentTypePercentages, "Actual output message does not correspond to expected one");
        }

        [TestMethod]
        public void GetSecretCodeQueryTest_ReturnsCorrectResult()
        {
            var expectedCode = "238";

            var secretCode = Queries.GetSecretCodeQuery();

            AssertBoolEqualsTrue(secretCode.Equals(expectedCode));
        }

        [TestMethod]
        public void GreeceLocationsPerSharkQueryTest_ReturnsCorrectResult()
        {
            // arrange
            var greeceLoactionsPerSharkExpectedResult = new Dictionary<string, List<string>>
            {
                { "White shark", new List<string> { "Corfu Island", "Cyclades", "Dodecanese Islands", "Pagasitikos Gulf" } },
                { "Nurse shark", new List<string> { "Carpathian Sea", "Corfu Island", "Thessaloniki" } }
            };

            // act
            var greeceLoactionsPerShark = Queries.GreeceLocationsPerSharkQuery();

            // assert
            var res = ComplexDictionaryEquals(greeceLoactionsPerSharkExpectedResult, greeceLoactionsPerShark);
            AssertBoolEqualsTrue(res);
        }

        [TestMethod]
        public void LightestSharksInSouthAmericaQueryTest_ReturnsCorrectResult()
        {
            var lightestSharksInSouthAmerica = Queries.LightestSharksInSouthAmericaQuery();

            AssertBoolEqualsTrue(lightestSharksInSouthAmerica != null);

            var sharkIds = lightestSharksInSouthAmerica
                .SelectMany(tuple => tuple.Item2)
                .Select(species => species.Id)
                .ToList();

            var expectedSharkIds = new List<int>
            {
                15, 13, 12, 14, 7, 2, 17, 15, 4, 4, 7, 3, 13, 17, 4, 13, 3, 17, 15
            };

            AssertBoolEqualsTrue(sharkIds.OrderBy(s => s).SequenceEqual(expectedSharkIds.OrderBy(s => s)));

            var sizes = lightestSharksInSouthAmerica
                .Select(tuple => tuple.Item2.Count)
                .ToList();

            var expectedSizes = new List<int>
            {
                1, 0, 8, 3, 2, 0, 0, 1, 0, 0, 0, 0, 1,3
            };

            AssertBoolEqualsTrue(sizes.OrderBy(s => s).SequenceEqual(expectedSizes.OrderBy(s => s)));

            var countries = lightestSharksInSouthAmerica
                .Select(tuple => tuple.Item1).ToList();

            var expectedCountries = new List<string>
            {
                "Argentina",
                "Bolivia",
                "Brazil",
                "Chile",
                "Ecuador",
                "Falkland Islands",
                "French Guiana",
                "Guyana",
                "Colombia",
                "Paraguay",
                "Peru",
                "Suriname",
                "Uruguay",
                "Venezuela"
            };

            AssertBoolEqualsTrue(countries.OrderBy(c => c).SequenceEqual(expectedCountries.OrderBy(c => c)));
        }

        [TestMethod]
        public void FastestSharksQueryTest_ReturnsCorrectResult()
        {
            // arrange
            var fastestSharksExpectedResult = new Dictionary<int, string>
            {
                {3, "ID: 3 Grey reef shark , max. length: 1.9m, weight: 38kg and can reach speeds up to 61 km/h. Attacked 36x times these swimmers: ID: 203 Male boy, ID: 232 Male a sailor, ID: 489 Male Nestor Valenzuela aged 20, ID: 547 Male James McKee aged 13, ID: 691 Male male, ID: 729 Male male, ID: 764 Male Gregory Moffatt, ID: 905 Male Servy LeRoux aged 23, ID: 969 Male Romasevic, ID: 1119 Male 2 males, ID: 1180 Male Jack Kardell aged 22, ID: 1403 Male Father Jose de Jesus Gomez, a priest aged 35, ID: 1567 Male William Tennant aged 33, ID: 1601 Male soldier, a private, ID: 1780 Male Michael Loxton aged 47, ID: 1950 Male Donald Bloom aged 38, ID: 2029 Male Norman Whiteley, ID: 2036 Male a German tourist aged 16, ID: 2289 Male Somali boatman aged 24, ID: 2342 Male Paul Guyot, ID: 2451 Male Jonathan Orth aged 17, ID: 2842 Male a ship's engineer, ID: 3049 Male Chris Humphrey aged 22, ID: 3080 Male Hans Pruss aged 68, ID: 3128 Male Erika Hailey aged 19, ID: 3228 Male Kimberly Popp aged 40, ID: 3394 Male Ian Martin Redmond aged 30, ID: 3719 Male male aged 9, ID: 3728 Male James Elliott aged 24, ID: 3900 Male John Hendrick Adrian Chandler, a prisoner of war aged 29, ID: 4559 Male Donald Gard, ID: 4597 Male boy aged 12, ID: 5160 Male Zulu male, ID: 5231 Male Kyle Gruen aged 29, ID: 5449 Male Edward Graham, ID: 5755 Male Cristina Ojeda-Thies aged 38"},
                {6, "ID: 6 Mako shark , max. length: 4.5m, weight: 800kg and can reach speeds up to 57 km/h. Attacked 50x times these swimmers: ID: 5 Male Ron Arney aged 19, ID: 37 Male male, ID: 115 Male Kyle Dickens aged 15, ID: 262 Male James Edward Clifton Donald aged 14, ID: 268 Male Joseph Meredith, ID: 506 Male _brahim Karagöz aged 16, ID: 558 Male Douglas Lawton aged 8, ID: 613 Male Jill Redenbaugh aged 14, ID: 744 Male woman, ID: 830 Male Akira Tuchiya, ID: 888 Male Majin Alvarez Piedra, ID: 893 Male Gertrude Holiday aged 20, ID: 909 Male Leroy Chadbourne aged 26, ID: 1069 Male Margoulis aged 15, ID: 1079 Male George Lawson, ID: 1192 Male Blake Tenville aged 12, ID: 1347 Male Richard  George Wilson aged 19, ID: 1357 Male Gary Kirakas, ID: 1414 Male Neil Buckley aged 23, ID: 1518 Male Mr. Daniels, ID: 1613 Male Jim Stevens, ID: 1766 Male Krishna Thompson aged 36, ID: 1819 Male George “Jimmy” Stevens, aborgine from the lugger Rebecca aged 17, ID: 1876 Male Ronald Kerwand aged 17, ID: 1882 Male Rupert Wade aged 57, ID: 2484 Male Ricky Karras, Jr., ID: 2633 Male Judy Fischman, ID: 2655 Male A visitor from Spain aged 26, ID: 2878 Male male aged 22, ID: 2920 Male John Cole aged 18, ID: 2968 Male Bruna Silva Gobbi aged 18, ID: 2998 Male Kristina Aleksandrova, ID: 3150 Male John Marrack aged 60, ID: 3341 Male Bryn Martin aged 64, ID: 3519 Male Jared Black aged 14, ID: 3891 Male female, ID: 3921 Male Mrs. Coe, ID: 3965 Male Maria Korcsmaros aged 52, ID: 4159 Male Lidua, a male, ID: 4221 Male Nicholas Nagurney aged 19, ID: 4598 Male Neil Tapp, cadet lifesaver aged 16, ID: 4820 Male Mary Jane Ryan aged 59, ID: 4878 Male male, ID: 4932 Male Josef Slivovic, ID: 4953 Male male, ID: 5037 Male E.W. Bilton aged 38, ID: 5365 Male Lester  Burton aged 33, ID: 5403 Male Malay pirates, ID: 5413 Male Gerardo Solis, ID: 5489 Male P. Kincaid Zifflewwiggett"},
                {7, "ID: 7 Lemon shark , max. length: 3m, weight: 183kg and can reach speeds up to 57 km/h. Attacked 44x times these swimmers: ID: 58 Male Yang Ching-Hui aged 20, ID: 295 Male Olive Heaton aged 61, ID: 328 Male David Vota, ID: 410 Male Glen Stoddart aged 23, ID: 719 Male William May aged 22, ID: 1036 Male Edwin Elford aged 12, ID: 1083 Male Bruce Bourke, ID: 1114 Male Robert Kay aged 18, ID: 1135 Male Manduray, a lifesaver, ID: 1252 Male Olivier Vilain aged 32, ID: 1353 Male Ralph Painter, ID: 1552 Male Percy Carroll aged 40, ID: 1726 Male Mark Moquin aged 35, ID: 1903 Male Thomas Richards, ID: 1930 Male Mr. Moriyoshi Takehara aged 52, ID: 2308 Male Corporal Kirkpatrick, ID: 2435 Male Robert Tennent aged 14, ID: 2572 Male a Portuguese soldier, ID: 2656 Male Hoang Thi Thuy Hong aged 41, ID: 2706 Male Mr. Meyer, ID: 2754 Male a sailor from the schooner Brother, ID: 2981 Male Jack Smedley aged 40, ID: 3073 Male Noah Green aged 30, ID: 3165 Male Fabrício José de Carvalho aged 19, ID: 3328 Male female aged 18, ID: 3375 Male Tracy Fasick aged 43, ID: 3673 Male Jordon Garosalo aged 16, ID: 3835 Male Scott Hall aged 23, ID: 3885 Male Lieut. James H. Stewart, ID: 3893 Male a Swedish sailor, ID: 3958 Male James Ogilvy, 31st in line for the British Throne aged 32, ID: 4210 Male Rupert Elford aged 13, ID: 4337 Male Goddard, ID: 4423 Male Mariko Haugen aged 51, ID: 4462 Male Mia Merlini aged 7, ID: 4502 Male Mike Spaulding aged 61, ID: 4594 Male Clyde Leeper aged 35, ID: 4753 Male Fábio Fernandes Silva aged 16, ID: 4798 Male Dave Martin aged 66, ID: 5464 Male male, ID: 5589 Male Puerto Rican aged 25, ID: 5601 Male David Miller aged 12, ID: 5678 Male male, ID: 5737 Male male aged 64"}
            };

            // act
            var fastestSharks = Queries.FastestSharksQuery();

            // assert
            var res = fastestSharksExpectedResult.SequenceEqual(fastestSharks);
            AssertBoolEqualsTrue(res);
        }

        [TestMethod]
        public void ContinentInfoAboutSurfersQueryTest_ReturnsCorrectResult()
        {
            var expectedResult = new Dictionary<string, Tuple<int, double>>
            {
                { "Central America", new Tuple<int, double>(4, 20.25) },
                { "Australia", new Tuple<int, double>(23, 24.96) },
                { "Asia", new Tuple<int, double>(2, 58) },
                { "Africa", new Tuple<int, double>(8, 22.33) },
                { "Europe", new Tuple<int, double>(1, 47) },
                { "South America", new Tuple<int, double>(4, 19.67) }
            };

            var continentInfoAboutSurfers = Queries.ContinentInfoAboutSurfersQuery();

            AssertBoolEqualsTrue(expectedResult.OrderBy(x => x.Key).SequenceEqual(continentInfoAboutSurfers.OrderBy(x => x.Key)));
        }

        [TestMethod]
        public void TopThreeCountriesByPopulationIncreaseQueryTest_ReturnsCorrectResult()
        {
            // arrange        
            var topThreeCountriesByPopulationIncreaseExpectedResult = new Dictionary<string, int>
            {
                { "India", 153958556},
                { "China", 68374269},
                { "Nigeria", 44845827}
            };

            // act
            var topThreeCountriesByPopulationIncrease = Queries.TopThreeCountriesByPopulationIncreaseQuery();

            // assert
            var res = SimpleDictionaryEquals(topThreeCountriesByPopulationIncreaseExpectedResult,
                topThreeCountriesByPopulationIncrease);
            AssertBoolEqualsTrue(res);
        }

        [TestMethod]
        public void InfoAboutPeopleThatNamesStartsWithCAndWasInBahamasQueryTest_ReturnsCorrectResult()
        {
            var expectedResult = new List<string>
            {
                "Captain Masson was attacked in Bahamas by Rhincodon typus",
                "C.D. Dollar was attacked in Bahamas by Carcharhinus brachyurus",
                "Carl James Harth was attacked in Bahamas by Carcharhinus brachyurus",
                "Carl Starling was attacked in Bahamas by Carcharhinus amblyrhynchos"
            };

            var infoAboutPeopleThatNamesStartsWithCAndWasInBahamas =
                Queries.InfoAboutPeopleThatNamesStartsWithCAndWasInBahamasQuery();

            var res = infoAboutPeopleThatNamesStartsWithCAndWasInBahamas.OrderBy(x => x)
                .SequenceEqual(expectedResult.OrderBy(x => x));
            AssertBoolEqualsTrue(res);
        }

        [TestMethod]
        public void InfoAboutPeopleThatWasInBahamasHeroicModeQueryTest_ReturnsCorrectResult()
        {
            var expectedResult = new List<string>
            {
                #region PeopleInBahamasAttackedBySharks
                "male was attacked by Isurus oxyrinchus",
                "Patricia Hodge was attacked by Sphyrna lewini",
                "Karl Kuchnow was attacked by Carcharodon carcharias",
                "Doug Perrine was attacked by Carcharhinus brevipinna",
                "Captain Masson was attacked by Rhincodon typus",
                "Kevin G. Schlusemeyer was attacked by Carcharhinus obscurus",
                "14' boat, occupant: Jonathan Leodorn was attacked by Ginglymostoma cirratum",
                "Jerry Greenberg was attacked by Carcharhinus obscurus",
                "Bruce Johnson, rescuer was attacked by Isurus oxyrinchus",
                "Philip Sweeting was attacked by Isurus oxyrinchus",
                "C.D. Dollar was attacked by Carcharhinus brachyurus",
                "Stanton Waterman was attacked by Carcharodon carcharias",
                "Francisco Edmund Blanc, a scientist from National Museum in Paris was attacked by Carcharodon carcharias",
                "Roy Pinder was attacked by Carcharhinus brevipinna",
                "Joanie Regan was attacked by Carcharodon carcharias",
                "Richard  Winer was attacked by Carcharodon carcharias",
                "young girl was attacked by Orectolobus hutchinsi",
                "12' skiff, occupant: E.R.F. Johnson was attacked by Carcharodon carcharias",
                "E.F. MacEwan was attacked by Carcharhinus limbatus",
                "Nick Raich was attacked by Carcharodon carcharias",
                "Krishna Thompson was attacked by Isurus oxyrinchus",
                "Kevin King was attacked by Isurus oxyrinchus",
                "James Douglas Munn was attacked by Sphyrna lewini",
                "John DeBry was attacked by Rhincodon typus",
                "John Petty was attacked by Notorynchus cepedianus",
                "male was attacked by Isurus oxyrinchus",
                "Sean Connelly was attacked by Carcharodon carcharias",
                "Mr. Wichman was attacked by Sphyrna lewini",
                "Tip Stanley was attacked by Carcharias taurus",
                "Roger Yost was attacked by Orectolobus hutchinsi",
                "Luis Hernandez was attacked by Carcharodon carcharias",
                "Max Briggs was attacked by Carcharhinus amblyrhynchos",
                "Markus Groh was attacked by Prionace glauca",
                "male, a sponge Diver was attacked by Carcharhinus brachyurus",
                "Michael Dornellas was attacked by Carcharhinus obscurus",
                "Henry Kreckman was attacked by Notorynchus cepedianus",
                "Katie Hester was attacked by Rhincodon typus",
                "Mark Adams was attacked by Carcharodon carcharias",
                "Leslie Gano was attacked by Orectolobus hutchinsi",
                "Whitefield Rolle was attacked by Sphyrna lewini",
                "Nixon Pierre was attacked by Carcharhinus brachyurus",
                "Sabrina Garcia was attacked by Sphyrna lewini",
                "Benjamin Brown was attacked by Galeocerdo cuvier",
                "Andrew Hindley was attacked by Ginglymostoma cirratum",
                "Bryan Collins was attacked by Galeocerdo cuvier",
                "male was attacked by Rhincodon typus",
                "Kerry Anderson was attacked by Notorynchus cepedianus",
                "Lacy Webb was attacked by Carcharodon carcharias",
                "male was attacked by Carcharias taurus",
                "male was attacked by Carcharhinus obscurus",
                "Russell Easton was attacked by Ginglymostoma cirratum",
                "Wolfgang Leander was attacked by Negaprion brevirostris",
                "Richard Horton was attacked by Ginglymostoma cirratum",
                "Kent Bonde was attacked by Carcharhinus obscurus",
                "Robert Gunn was attacked by Carcharhinus plumbeus",
                "Jim Abernethy was attacked by Ginglymostoma cirratum",
                "Derek Mitchell was attacked by Carcharodon carcharias",
                "Alton Curtis was attacked by Carcharhinus brachyurus",
                "male was attacked by Carcharhinus plumbeus",
                "Burgess & 2 seamen was attacked by Carcharias taurus",
                "Wilber Wood was attacked by Orectolobus hutchinsi",
                "boy was attacked by Carcharhinus brevipinna",
                "Erik Norrie was attacked by Sphyrna lewini",
                "Scott Curatolo-Wagemann was attacked by Ginglymostoma cirratum",
                "Kenny Isham was attacked by Carcharhinus brachyurus",
                "Lowell Nickerson was attacked by Carcharodon carcharias",
                "Peter Albury was attacked by Notorynchus cepedianus",
                "Wyatt Walker was attacked by Ginglymostoma cirratum",
                "William Barnes was attacked by Carcharhinus brachyurus",
                "Valerie Fortunato was attacked by Lamna ditropis",
                "Ken Austin was attacked by Carcharhinus obscurus",
                "John Fenton was attacked by Carcharodon carcharias",
                "Jose Molla was attacked by Carcharodon carcharias",
                "Jane Engle was attacked by Carcharhinus obscurus",
                "Judson Newton was attacked by Carcharhinus limbatus",
                "John Cooper was attacked by Ginglymostoma cirratum",
                "Herbert J. Mann was attacked by Sphyrna lewini",
                "Bruce Cease was attacked by Isurus oxyrinchus",
                "Judy St. Clair was attacked by Ginglymostoma cirratum",
                "Larry Press was attacked by Carcharias taurus",
                "male from pleasure craft Press On Regardless was attacked by Galeocerdo cuvier",
                "Robert Marx was attacked by Notorynchus cepedianus",
                "Renata Foucre was attacked by Carcharhinus obscurus",
                "Hayward Thomas & Shalton Barr was attacked by Sphyrna lewini",
                "Bill Whitman was attacked by Carcharhinus leucas",
                "Eric Gijsendorfer was attacked by Carcharhinus obscurus",
                "Carl James Harth was attacked by Carcharhinus brachyurus",
                "Carl Starling was attacked by Carcharhinus amblyrhynchos",
                "George Vanderbilt was attacked by Carcharias taurus",
                "Kevin Paffrath was attacked by Carcharhinus brachyurus",
                "Erich Ritter was attacked by Rhincodon typus",
                "unknown was attacked by Carcharhinus brevipinna",
                "a pilot was attacked by Carcharhinus leucas",
                "Michael Beach was attacked by Notorynchus cepedianus",
                "Omar Karim Huneidi was attacked by Carcharhinus amblyrhynchos",
                "Richard Pinder was attacked by Carcharhinus brachyurus"
                #endregion
            };

            var infoAboutPeopleThatWasInBahamas =
                Queries.InfoAboutPeopleThatWasInBahamasHeroicModeQuery();

            var res = infoAboutPeopleThatWasInBahamas.OrderBy(x => x)
                .SequenceEqual(expectedResult.OrderBy(x => x));
            AssertBoolEqualsTrue(res);

        }
        
        [TestMethod]
        public void InfoAboutFinesInEuropeQueryTest_ReturnsCorrectResult()
        {
            var infoAboutFinesInEurope = Queries.InfoAboutFinesInEuropeQuery();

            var states = infoAboutFinesInEurope.Select(state => state.Split(':')[0]);

            var expectedStates = new List<string>
            {
                #region States
                "Italy",
                "Croatia",
                "Greece",
                "France",
                "Ireland",
                "Albania",
                "Andorra",
                "Austria",
                "Belarus",
                "Belgium",
                "Bosnia and Herzegovina",
                "Bulgaria",
                "Czech Republic",
                "Denmark",
                "Estonia",
                "Faroe Islands",
                "Finland",
                "Germany",
                "Gibraltar",
                "Guernsey",
                "Holy See (Vatican City)",
                "Hungary",
                "Iceland",
                "Isle of Man",
                "Jersey",
                "Kosovo",
                "Latvia",
                "Liechtenstein",
                "Lithuania",
                "Luxembourg"
                #endregion
            };

            AssertBoolEqualsTrue(expectedStates.SequenceEqual(states));

            var fines = infoAboutFinesInEurope
                .Select(state => state.Split(':')[1])
                .Take(6);

            var exptectedFines = new List<string>
            {
                " 17900 EUR",
                " 8900 HRK",
                " 6750 EUR",
                " 3150 EUR",
                " 300 EUR",
                " 0 ALL"
            };
            AssertBoolEqualsTrue(exptectedFines.SequenceEqual(fines));
        }

        #region TestHelperEqualityMethods

        private static bool CheckTwoCollections<T1, T2>(IDictionary<T1, T2> first, IDictionary<T1, T2> second)
        {
            return first != null && second != null && first.Count == second.Count;
        }

        private static bool SimpleDictionaryEquals<T1, T2>(IDictionary<T1, T2> first, IDictionary<T1, T2> second)
        {
            return CheckTwoCollections(first, second) &&
                first.All(keyValuePair => second.ContainsKey(keyValuePair.Key) && keyValuePair.Value.Equals(second[keyValuePair.Key]));
        }

        private static bool ComplexDictionaryEquals<T1, T2>(Dictionary<T1, List<T2>> first, IDictionary<T1, List<T2>> second) where T2 : class
        {
            return CheckTwoCollections(first, second) &&
                first.All(keyValuePair => second.ContainsKey(keyValuePair.Key) && keyValuePair.Value.SequenceEqual(second[keyValuePair.Key]));
        }

        private static bool ComplexDictionaryEquals<T1, T2, T3>(IDictionary<T1, Dictionary<T2, List<T3>>> first,
            IDictionary<T1, Dictionary<T2, List<T3>>> second) where T3 : class
        {
            if (!CheckTwoCollections(first, second))
            {
                return false;
            }
            for (var i = 0; i < first.Count; i++)
            {
                var firstInnerDictionary = first.ElementAt(i).Value;
                if (!second.Select(item => item.Key).Contains(first.ElementAt(i).Key))
                {
                    return false;
                }
                var secondInnerDictionary = second.First(item => item.Key.Equals(first.ElementAt(i).Key)).Value;


                if (firstInnerDictionary.Count != secondInnerDictionary.Count)
                {
                    return false;
                }
                for (var j = 0; j < firstInnerDictionary.Count; j++)
                {
                    var firstInnerList = firstInnerDictionary.ElementAt(j).Value;
                    if (!second.Select(item => item.Key).Contains(first.ElementAt(j).Key))
                    {
                        return false;
                    }
                    var secondInnerList = secondInnerDictionary.First(item => item.Key.Equals(firstInnerDictionary.ElementAt(j).Key)).Value;
                    if (firstInnerList.Count != secondInnerList.Count)
                    {
                        return false;
                    }
                    if (firstInnerList.Where((t, k) => !secondInnerList.Contains(firstInnerList.ElementAt(k))).Any())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static void AssertBoolEqualsTrue(bool res)
        {
            Assert.AreEqual(true, res, "Actual result and the expected one are not equal.");
        }

        #endregion
    }
}
