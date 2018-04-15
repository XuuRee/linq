using System;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PV178.Homeworks.HW04.DataLoading.DataContext;
using PV178.Homeworks.HW04.DataLoading.Factory;
using PV178.Homeworks.HW04.Model;
using PV178.Homeworks.HW04.Model.Enums;

namespace PV178.Homeworks.HW04
{
    public class Queries
    {
        private IDataContext dataContext;
        public IDataContext DataContext => dataContext ??
            (dataContext = new DataContextFactory().CreateDataContext());
        
        /// <summary>
        /// Najdete jmena vsech osob, ktere od 3. 3 roku 1960 - do 12. 11 roku 1980 (vcetne) 
        /// zarucene prezili napadeni zralokem, kteremu se prezdiva "White death". 
        /// Z nalezenych jmen dale vyberte pouze ty, ktere zacinaji pismenem 'K'
        /// Nalezena jmena pak seradte abecedne (a->z).
        /// </summary>
        /// <returns>The query result</returns>
        public List<string> WhiteDeathSurvivorsStartingWithKQuery()
        {
            var dateFrom = new DateTime(1960, 03, 03);
            var dateTo = new DateTime(1980, 11, 12);
            
            var WhiteDeathSurvivorsStartingWithK = DataContext.SharkSpecies.
                Where(x => x.AlsoKnownAs.Equals("White death")).
                Join(DataContext.SharkAttacks,
                    ss_id => ss_id.Id,
                    sa_id => sa_id.SharkSpeciesId,
                    (ss_id, sa_id) => new {
                        person_id = sa_id.AttackedPersonId,
                        date_attack = sa_id.DateTime,
                        severenity = sa_id.AttackSeverenity
                    }
                ).
                Where(x => x.severenity != AttackSeverenity.Fatal && x.date_attack >= dateFrom && x.date_attack <= dateTo).
                Join(DataContext.AttackedPeople,
                    p_id => p_id.person_id,
                    ap_id => ap_id.Id,
                    (p_id, ap_id) => new {
                        name = ap_id.Name
                    }
                ).
                Where(x => x.name.StartsWith("K")).
                OrderBy(x => x.name).
                Select(x => x.name.ToString()).
                ToList();
            
            return WhiteDeathSurvivorsStartingWithK;
        }

        /// <summary>
        /// Vedci robia štúdiu o tom, či dlhé žraloky pri svojich útokoch berú ohľad 
        /// na pohlavie obete. Pomôžte im preto zistiť, či boli zaznamenané útoky
        /// na obidve pohlavia pre každý druh žraloka dlhšieho ako 2 metre.
        /// -----------------------
        /// Vráti informáciu, či každý druh žraloka, ktorý je dlhší ako 2 metre,
        /// útočil aj na muža, aj na ženu.
        /// </summary>
        /// <returns>The query result</returns>
        public bool AreAllLongSharksGenderIgnoringQuery()
        {
            var areAllLongSharksGenderIgnoring = DataContext.SharkSpecies.
                Where(x => x.Length.CompareTo(2.00) > 0).
                Join(DataContext.SharkAttacks,
                    ss_id => ss_id.Id,
                    sa_id => sa_id.SharkSpeciesId,
                    (ss_id, sa_id) => new {
                        attackedPersonId = sa_id.AttackedPersonId
                    }).
                Join(DataContext.AttackedPeople,
                    sa_id => sa_id.attackedPersonId,
                    ap_id => ap_id.Id,
                    (sa_id, ap_id) => new {
                        sex = ap_id.Sex
                }).
                Where(x => x.sex != Sex.Unknown).
                All(x => !(x.sex == Sex.Male || x.sex == Sex.Female));

            return areAllLongSharksGenderIgnoring;
        }

        /// <summary>
        /// Chceli by sme preskúmať, či aj žraloci bez prezývky útočia na ľudí. Ak áno, ako veľmi.
        /// -----------------------
        /// Vráti slovník, ktorý pre každého žraloka, ktorý nemá nejakú prezývku (AlsoKnownAs),
        /// uchováva počet krajín, v ktorých útočil. Ako kľúč sa použije meno žraloka.
        /// </summary>
        /// <returns>The query result</returns>
        public Dictionary<string, int> SharksWithoutNicknameAttacksQuery()
        {
            var sharksWithoutNicknameAttacks = DataContext.SharkSpecies.
                Where(x => x.AlsoKnownAs.Equals(String.Empty)).
                Join(DataContext.SharkAttacks,
                    ss_id => ss_id.Id,
                    sa_id => sa_id.SharkSpeciesId,
                    (ss_id, sa_id) => new {
                        ss_id.Name,
                        sa_id.CountryId
                }).
                Where(x => x.CountryId.HasValue).
                Distinct().
                GroupBy(x => x.Name).
                Select(item => new {
                    name = item.Key,
                    countries = item.Count()
                }).
                OrderByDescending(x => x.countries).
                ToDictionary(t => t.name, t => t.countries);

            return sharksWithoutNicknameAttacks;
        }

        /// <summary>
        /// Najdete 5 zralocich druhu, ktere maji na svedomi nejvice lidskych zivotu, 
        /// druhy seradte dle poctu obeti a vysledek vratte ve forme slovniku, kde: 
        ///   klicem je nazev zralociho druhu
        ///   hodnotou je souhrny pocet obeti, zpusobeny danym druhem zraloka
        /// </summary>
        /// <returns>The query result</returns>
        public  Dictionary<string, int> FiveSharkSpeciesWithMostFatalitiesQuery()
        {
            var fiveSharkSpeciesWithMostFatalities = DataContext.SharkSpecies.
                Join(DataContext.SharkAttacks,
                    ss_id => ss_id.Id,
                    sa_id => sa_id.SharkSpeciesId,
                    (ss_id, sa_id) => new {
                        ss_id.Name,
                        sa_id.AttackSeverenity
                }).
                Where(x => x.AttackSeverenity == AttackSeverenity.Fatal).
                GroupBy(x => x.Name).
                Select(item => new {
                    name = item.Key,
                    victims = item.Count()
                }).
                OrderByDescending(x => x.victims).
                Take(5).
                ToDictionary(t => t.name, t => t.victims);

            return fiveSharkSpeciesWithMostFatalities;
        }

        /// <summary>
        /// Pro vsechny hodnoty vyctoveho typu GovernmentForm spocitejte jejich procentualni zastoupeni podle zemi 
        /// (procentualni zastoupeni predstavuje jednoduse podil poctu zemi s danym typem vlady ku poctu vsech zemi)
        /// Z vysledku pak zformujte retezec (pro slouceni pouzijte metodu Aggregate(...)), ktery bude mit nasledujici format:
        /// "{GovermentForm}: {procentualni_zastoupeni}%, ..." kde procentualni zastoupeni bude zformatovano na 1 desetinne misto.
        /// Ocekavany vystup bude serazeny sestupne dle procentualniho zastoupeni, tedy: 
        /// "Republic: 59.9%, Monarchy: 18.6%, Territory: 15.8%, ... (na konci vypisu jiz neni carka)
        /// </summary>
        /// <returns>The query result</returns>
        public string GovernmentTypePercentagesQuery()
        {
            double onePercent = (double) DataContext.Countries.Count() / 100;

            var governmentTypePercentagesQuery = DataContext.Countries.
                GroupBy(x => x.GovernmentForm).
                Select(item => new {
                    id = item.Key,
                    gov_count = item.Count() / onePercent
                }).
                OrderByDescending(x => x.gov_count).
                Aggregate(new StringBuilder(), (c, n) => {
                    if (c.Length > 0)
                        c.Append(", ");
                    c.Append(n.id.ToString() + ": " + n.gov_count.ToString("0.0") + "%");
                    return c;
                }).
                ToString();

            return governmentTypePercentagesQuery;
        }

        /// <summary>
        /// Našiel sa kus papiera a starobylý notebook, ktorý je ale bohužiaľ chránený trojmiestnym
        /// číselným heslom. Aby sme sa do neho dostali, potrebujeme rozlúštiť hádanku na papieri:
        /// 
        /// Heslo je súčet písmen, ktoré obsahujú názvy krajín, v ktorých
        /// útočil žralok, ktorý má najvyššiu maximálnu rýchlosť zo všetkých žralokov.
        /// -----------------------
        /// Vráti tajné heslo k starobylému notebooku.
        /// </summary>
        /// <returns>The query result</returns>
        public  string GetSecretCodeQuery()
        {
            var longestShark = DataContext.SharkSpecies.
                OrderByDescending(x => x.TopSpeed).
                First();

            var secretCode = DataContext.SharkAttacks.
                Where(x => x.SharkSpeciesId == longestShark.Id).
                Join(DataContext.Countries,
                    sa_id => sa_id.CountryId,
                    c_id => c_id.Id,
                    (sa_id, c_id) => new {
                        c_id.Name
                }).
                Distinct().
                Aggregate(new StringBuilder(),
                    (c, n) => c.Append(n.Name)).
                Length.
                ToString();

            return secretCode;
        }

        /// <summary>
        /// Najdete vsechny neprazdne oblasti (Area) v Recku, kde utocili zraloci s hodnotou ID mensi nez 6, 
        /// nalezene oblasti seskupte (pouzijte metodu GroupJoin(...)) dle nazvu zraloka
        /// do slovniku* kde:
        ///   klicem je nazev zraloka
        ///   hodnotou je list vsech oblasti kde zralok utocil, ktere jsou abecedne serazeny (A -> Z)
        /// 
        /// *Do vysledku nezahrnujte polozky, ktere maji jako hodnotu prazdny list.
        /// </summary>
        /// <returns>The query result</returns>
        public Dictionary<string, List<string>> GreeceLocationsPerSharkQuery()
        {
            var greece = DataContext.Countries.
                Where(x => x.Name.Equals("Greece")).
                First();

            var greeceLocationsPerShark = DataContext.SharkSpecies.
                Where(x => x.Id < 6).
                GroupJoin(DataContext.SharkAttacks.
                    Where(x => x.CountryId.HasValue && x.CountryId.Value == greece.Id && !x.Area.Equals(String.Empty)),
                    ss_id => ss_id.Id,
                    sa_id => sa_id.SharkSpeciesId,
                    (id_s, result) => new {
                        id_s.Name,
                        result
                }).
                Where(x => x.result.Any()).
                Select(x => new {
                    names = x.Name,
                    areas = x.result.OrderBy(c => c.Area).Select(c => c.Area).Distinct().ToList()
                }).
                ToDictionary(t => t.names, t => t.areas);
               
            return greeceLocationsPerShark;
        }

        /// <summary>
        /// Zistilo sa, že 10 najľahších žralokov sa správalo veľmi
        /// podozrivo počas útokov v štátoch Južnej Ameriky.
        /// ---------------------------------------
        /// Táto funkcia preto vráti zoznam dvojíc, kde pre každý štát z danej množiny
        /// bude uvedený zoznam žralokov z danej množiny, ktorí v tom štáte útočili.
        /// Pokiaľ v nejakom štáte neútočil žiaden z najľahších žralokov, zoznam žralokov bude prázdny.
        /// </summary>
        /// <returns>The query result</returns>
        public List<Tuple<string, List<SharkSpecies>>> LightestSharksInSouthAmericaQuery()
        {
            var tenLightestSharks = DataContext.SharkSpecies.
                OrderBy(s => s.Weight).
                Take(10);
                
            var attacksWithSharks = DataContext.SharkAttacks.
                Join(DataContext.SharkSpecies,
                    a_id => a_id.SharkSpeciesId,
                    s_id => s_id.Id,
                    (a_id, s_id) => new {
                        a_id.CountryId,
                        s_id
                });

            var countriesWithLightestSharks = DataContext.Countries.
                Where(c => c.Continent.Equals("South America")).
                GroupJoin(attacksWithSharks,
                    c_id => c_id.Id,
                    v_id => v_id.CountryId,
                    (c_id, result) => new {
                        names = c_id.Name,
                        sharks = result.Select(x => x.s_id).Where(x => tenLightestSharks.Contains(x)).Distinct().ToList()
                }).
                Select(x => new Tuple<string, List<SharkSpecies>>(x.names, x.sharks)).
                ToList();

            return countriesWithLightestSharks;
        }

        /// <summary>
        /// Pro vsechny zraloky, kteri maji maximalni rychlost vyssi nez 56km/h, 
        /// vyberte vsechny utoky, ktere se staly pri aktivite "swimming" a je u nich znam plavec (AttackedPersonId), 
        /// Ziskane hodnoty budou serazeny primarne dle rychlosti zraloka,
        /// sekundarne pak podle poctu utoku dle jednotlivych druhu zraloka.
        /// Vystup dotazu pak bude v podobe slovniku, kde:
        ///   klicem je identifikator daneho druhu zraloka
        ///   hodnotou je retezec ve tvaru:  
        ///    "{zralok.ToString()} Attacked {pocet_utoku}x times these swimmers: {1_osoba.ToString()}, {2_osoba.ToString()}, {n-ta_osoba.ToString()}"
        /// 
        /// Pro vypis dotcenych osob pouzijte funkci Aggregate() primo nad IEnumerable<AttackedPerson>,  
        /// pri spojovani vice 'tabulek' pak vhodne pouzijte metodu GroupJoin()
        /// </summary>
        /// <returns>The query result</returns>
        public Dictionary<int, string> FastestSharksQuery()
        {
            var fastestSharks = DataContext.SharkSpecies.
                Where(x => x.TopSpeed > 56).
                GroupJoin(DataContext.SharkAttacks,
                    ss_id => ss_id.Id,
                    sa_id => sa_id.SharkSpeciesId,
                    (ss_id, sa_id) => new {
                        ss_id,
                        ssa_id = sa_id.
                        Where(x => x.Activity.ToLower().Contains("swimming") && x.AttackedPersonId.HasValue)
                        .Join(DataContext.AttackedPeople,
                            v_id => v_id.AttackedPersonId,
                            ap_id => ap_id.Id,
                            (v_id, ap_id) => new {
                                ap_id
                        })
                }).
                OrderByDescending(x => x.ss_id.TopSpeed).
                ThenByDescending(x => x.ssa_id.Count()).
                ToDictionary(t =>
                    t.ss_id.Id,
                    t => t.ss_id.ToString() + " Attacked " + t.ssa_id.Count() + "x times these swimmers: " + t.ssa_id.
                    Aggregate(new StringBuilder(),
                        (c, n) => {
                            if (c.Length > 0)
                                c.Append(", ");
                            c.Append(n.ap_id.ToString());
                            return c;
                        })
                );

            return fastestSharks;
        }

        /// <summary>
        /// Celý svet chce zákonom vekovo obmedziť predaj surfov, pretože vedci zistili, že žralokom
        /// sa nepáči, keď je človek pod istou vekovou hranicou na surfe. Samozrejme, na každom kontinente
        /// je to inak (iní žraloci, iná vláda), preto potrebujeme štatistické informácie, rozdelené podľa kontinentov.
        /// -----------------------
        /// Pre všetky kontinenty, v ktorých prišiel o život človek počas surfovania (Activity obsahuje "surf", "SURF" alebo "Surf")
        /// vráti informáciu o počte obetí, ktoré zahynuli pri surfovaní a taktiež ich priemerný vek,
        /// zaokrúhlený na dve desatinné miesta.
        /// </summary>
        /// <returns>The query result</returns>
        public Dictionary<string, Tuple<int, double>> ContinentInfoAboutSurfersQuery()
        {
            var continentInfoAboutSurfers = DataContext.Countries.
                Join(DataContext.SharkAttacks,
                    c_id => c_id.Id,
                    sa_id => sa_id.CountryId,
                    (c_id, sa_id) => new {
                        c_id,
                        sa_id
                }).
                Where(x => x.sa_id.Activity.ToLower().Contains("surf") && x.sa_id.AttackSeverenity == AttackSeverenity.Fatal).
                Join(DataContext.AttackedPeople,
                    sa_id => sa_id.sa_id.AttackedPersonId,
                    ap_id => ap_id.Id,
                    (sa_id, ap_id) => new {
                        continents = sa_id.c_id.Continent,
                        ages = ap_id.Age
                }).
                GroupBy(x => x.continents).
                Select(x => new {
                    ids = x.Key,
                    counters = x.Count(),
                    people_with_ages = x.Where(c => c.ages.HasValue).Count(),
                    sum_people_with_ages = x.Where(c => c.ages.HasValue).Sum(c => c.ages.Value)
                }).
                ToDictionary(t => t.ids, t => new Tuple<int, double>(t.counters, Math.Round(t.sum_people_with_ages / (double)t.people_with_ages, 2)));

            return continentInfoAboutSurfers;
        }

        /// <summary>
        /// Vyberte prvni 3 zeme dle nejvetsiho prirustku obyvatelstva, 
        /// ten spocitejte jako rozdil Birthrate a Deathrate (je treba prevest na desetinna cisla z procent) 
        /// vynasobeny soucasnou populaci dane zeme.
        /// Vysledne 3 zeme vratte ve slovniku kde:
        ///   klicem je nazev zeme
        ///   hodnotou je vypocteny prirustek obyvatelstva (typu int)
        /// 
        /// Pri vypoctu pouzijte metodu Zip(...).
        /// </summary>
        /// <returns>The query result</returns>
        public IDictionary<string, int> TopThreeCountriesByPopulationIncreaseQuery()
        {
            /* 
            //version without zip
            var topThreeCountriesByPopulation = DataContext.Countries.
                Select(x => new {
                    name = x.Name,
                    growth = (int) (((x.Birthrate - x.Deathrate) / 100) * x.Population)
                }).
                OrderByDescending(x => x.growth).
                Take(3).
                ToDictionary(t => t.name, t => t.growth);
            */

            var names = DataContext.Countries.
                Select(x => new { name = x.Name });

            var growth = DataContext.Countries.
                Select(x => new {
                    growth = (int)(((x.Birthrate - x.Deathrate) / 100) * x.Population)
                });

            var topThreeCountriesByPopulationZip = names.
                Zip(growth, (c, n) => new {
                    z_name = c.name,
                    z_growth = n.growth
                }).
                OrderByDescending(x => x.z_growth).
                Take(3).
                ToDictionary(t => t.z_name, t => t.z_growth);
            
            return topThreeCountriesByPopulationZip;
        }

        /// <summary>
        /// V jednej cestovnej kancelárií si objednala dovolenku do Bahám veľká rodina,
        /// v ktorej všetkým členom začína meno na písmeno C. Cestovnú kanceláriu by zaujímalo,
        /// či ich tam môžu pustiť; závisí to od počtu takýchto ľudí, na ktorých tam zaútočil žralok.
        /// -----------------------
        /// Vráti zoznam, v ktorom je textová informácia o každom človeku,
        /// ktorého meno začína na písmeno C a na ktorého zaútočil žralok v štáte Bahamas.
        /// Táto informácia je v tvare:
        /// {meno človeka} was attacked in Bahamas by {latinský názov žraloka}
        /// </summary>
        /// <returns>The query result</returns>
        public List<string> InfoAboutPeopleThatNamesStartsWithCAndWasInBahamasQuery()
        {
            var infoAboutPeople = DataContext.Countries.
                Where(x => x.Name.Equals("Bahamas")).
                Join(DataContext.SharkAttacks,
                    c_id => c_id.Id,
                    sa_id => sa_id.CountryId,
                    (c_id, sa_id) => new {
                        sa_id.AttackedPersonId,
                        sa_id.SharkSpeciesId
                }).
                Join(DataContext.AttackedPeople,
                    v_id => v_id.AttackedPersonId,
                    ap_id => ap_id.Id,
                    (v_id, ap_id) => new {
                        ap_id.Name,
                        v_id.SharkSpeciesId
                }).
                Where(x => x.Name.StartsWith("C")).
                Join(DataContext.SharkSpecies,
                    v_id => v_id.SharkSpeciesId,
                    ss_id => ss_id.Id,
                    (v_id, ss_id) => new {
                        item = v_id.Name.ToString() + " was attacked in Bahamas by " + ss_id.LatinName.ToString()
                    }
                ).
                Select(x => (string) x.item).
                ToList();

            return infoAboutPeople;
        }

        /// <summary>
        /// Výsledky z predchádzajúceho dotazu boli pre cestovnú kanceláriu tak zaujímavé, že by chceli vedieť
        /// podobné informácie o každom človeku, na ktorého zaútočil žralok v Bahamách. Medzičasom ale vyšlo
        /// nariadenie, že spájanie dvoch tabuliek sa považuje za hriech...
        /// -----------------------
        /// Vráti zoznam, v ktorom je textová informácia o KAŽDOM človeku na ktorého zaútočil žralok v štáte Bahamas.
        /// Táto informácia je taktiež v tvare:
        /// {meno človeka} was attacked by {latinský názov žraloka}
        /// 
        /// POZOR!
        /// Zistite tieto informácie bez spojenia hocijakých dvoch tabuliek (môžete ale použiť metódu Zip)
        /// </summary>
        /// <returns>The query result</returns>
        public List<string> InfoAboutPeopleThatWasInBahamasHeroicModeQuery()
        {
            var bahama = DataContext.Countries.
                Where(x => x.Name.Equals("Bahamas")).
                First();

            var infoAboutPeopleThatWasInBahamas = DataContext.SharkAttacks.
                OrderBy(x => x.AttackedPersonId).
                Where(x => x.AttackedPersonId.HasValue).
                Zip(DataContext.AttackedPeople.OrderBy(x => x.Id),
                    (c, n) => new {
                        name = n.Name,
                        shark_id = c.SharkSpeciesId,
                        c_id = c.CountryId
                }).
                Where(x => x.c_id == bahama.Id).
                Select(x => new {
                    item = x.name.ToString() + " was attacked by " + DataContext.SharkSpecies.First(y => y.Id == x.shark_id).LatinName.ToString()
                }).
                Select(x => (string) x.item).
                ToList();

            return infoAboutPeopleThatWasInBahamas;
        }

        /// <summary>
        /// Nedávno vyšiel zákon, že každá krajina Európy začínajúca na písmeno A až L (vrátane)
        /// musí zaplatiť pokutu 250 peňazí svojej meny za každý žraločí útok na ich území.
        /// Pokiaľ bol tento útok smrteľný, musia dokonca zaplatiť až 300 peňazí. Ak sa nezachovali
        /// údaje o tom, či bol daný útok smrteľný alebo nie, nemusia platiť nič.
        /// Áno, tento zákon je spravodlivý...
        /// -----------------------
        /// Vráti informácie o výške pokuty každej krajiny Európy začínajúcej na A až L
        /// zoradené zostupne podľa počtu peňazí, ktoré musia tieto krajiny zaplatiť.
        /// Príklad formátu výstupu v prípade, že na Slovensku boli dva smrteľné útoky žralokov,
        /// v Maďarsku 0 útokov a v Česku jeden smrteľný útok a jeden útok, pri ktorom obeť prežila:
        /// Slovakia: 600 EUR
        /// Czech Republic: 550 CZK
        /// Hungary: 0 HUF
        /// </summary>
        /// <returns>The query result</returns>
        public List<string> InfoAboutFinesInEuropeQuery()
        {
            var characters = new List<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l" };

            var infoAboutFinesEurope = DataContext.Countries.
                Where(x => x.Continent.Equals("Europe") && characters.Any(y => x.Name.ToLower().StartsWith(y))).
                GroupJoin(DataContext.SharkAttacks,
                    c_id => c_id.Id,
                    sa_id => sa_id.CountryId,
                    (c_id, result) => new {
                        name = c_id.Name,
                        currency = c_id.CurrencyCode,
                        money = result.Aggregate(0, (c, n) => {
                            if (n.AttackSeverenity == AttackSeverenity.Fatal)
                                c += 300;
                            else if (n.AttackSeverenity == AttackSeverenity.NonFatal)
                                c += 250;
                            else
                                c += 0;
                            return c;
                        })
                }).
                OrderByDescending(x => x.money).
                ThenBy(x => x.name).
                Select(x => new {
                    item = x.name.ToString() + ": " + x.money.ToString() + " " + x.currency.ToString()
                }).
                Select(x => (string) x.item).
                ToList();

            return infoAboutFinesEurope;
        }
    }
}
