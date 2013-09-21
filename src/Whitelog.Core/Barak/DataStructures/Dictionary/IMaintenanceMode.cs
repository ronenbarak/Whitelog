namespace Whitelog.Barak.Common.DataStructures.Dictionary
{
    public interface IMaintenanceMode
    {
        int ExpectedSize(int nodes, int seeds, int maxduplicatehashcount);
    }
}