using System.Windows.Controls.Primitives;

namespace JonglaInterview.Helpers
{
    public interface IMultiSelectCollectionView
    {
        void AddControl(Selector selector);
        void RemoveControl(Selector selector);
    }
}
