namespace Interfaces
{
    public interface IView<in TBaseController>
    {
        void Init(TBaseController controller);
    }
}