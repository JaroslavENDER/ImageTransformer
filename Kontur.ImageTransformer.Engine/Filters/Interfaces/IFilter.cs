using System.Drawing;

namespace Kontur.ImageTransformer.Engine.Filters
{
    public interface IFilter
    {
        void Process(Bitmap image);
    }
}
