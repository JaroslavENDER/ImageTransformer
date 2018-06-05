using System.Drawing;

namespace Kontur.ImageTransformer.Engine.Filters
{
    public interface IFilter
    {
        Bitmap Process(Bitmap image);
    }
}
