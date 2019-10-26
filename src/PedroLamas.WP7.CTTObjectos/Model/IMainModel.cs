using System.Collections.Generic;

namespace PedroLamas.WP7.CTTObjectos.Model
{
    public interface IMainModel
    {
        IList<ObjectModel> TrackedObjects { get; }

        void Save();
    }
}