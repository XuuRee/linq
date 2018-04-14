using System.Collections.Generic;
using PV178.Homeworks.HW04.Model;

namespace PV178.Homeworks.HW04.DataLoading.DataContext
{
    public class DataContext : IDataContext
    {
        public IReadOnlyList<SharkAttack> SharkAttacks { get; }

        public IReadOnlyList<AttackedPerson> AttackedPeople { get; }

        public IReadOnlyList<SharkSpecies> SharkSpecies { get; }

        public IReadOnlyList<Country> Countries { get; }

        public DataContext(IReadOnlyList<SharkAttack> sharkAttacks,  
            IReadOnlyList<AttackedPerson> attackedPeople, 
            IReadOnlyList<SharkSpecies> sharkSpecies,
            IReadOnlyList<Country> countries)
        {
            SharkAttacks = sharkAttacks;
            AttackedPeople = attackedPeople;
            SharkSpecies = sharkSpecies;
            Countries = countries;
        }
    }
}
