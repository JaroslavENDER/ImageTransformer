using System.Drawing;

namespace Kontur.ImageTransformer.Transformer.Filters
{
    public interface IFilter
    {
        void Process(Bitmap image);
    }
}
