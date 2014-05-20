using BDCommon.Network;

namespace BDCommon.EngineInterfaces
{
    public interface IAdminEngine
    {
        void ProcessAction(IConnection connection, string message);
    }
}
