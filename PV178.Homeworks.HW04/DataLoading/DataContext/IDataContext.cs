using System.Collections.Generic;
using PV178.Homeworks.HW04.Model;

namespace PV178.Homeworks.HW04.DataLoading.DataContext
{
    public interface IDataContext
    {
        IReadOnlyList<AttackedPerson> AttackedPeople { get; }
        IReadOnlyList<Country> Countries { get; }
        IReadOnlyList<SharkAttack> SharkAttacks { get; }
        IReadOnlyList<SharkSpecies> SharkSpecies { get; }
    }
}