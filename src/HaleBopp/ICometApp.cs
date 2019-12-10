using IContainer = DryIoc.IContainer;

namespace HaleBopp
{
    public interface ICometApp
    {
        void RegisterServices(IContainer container);
    }
}
