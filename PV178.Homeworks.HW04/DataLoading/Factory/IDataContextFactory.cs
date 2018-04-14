using PV178.Homeworks.HW04.DataLoading.DataContext;

namespace PV178.Homeworks.HW04.DataLoading.Factory
{
    public interface IDataContextFactory
    {
        IDataContext CreateDataContext();
    }
}
