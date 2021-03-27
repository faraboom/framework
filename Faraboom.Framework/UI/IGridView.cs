using System.Collections.Generic;

namespace Faraboom.Framework.UI
{
    public interface IGridView<TModel, TSearch>
        where TModel : class
        where TSearch : class
    {
        TSearch Search { get; set; }

        IEnumerable<TModel> Model { get; set; }
    }
}
