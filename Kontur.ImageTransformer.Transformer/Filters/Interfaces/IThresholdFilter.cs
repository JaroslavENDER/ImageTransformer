using System.Drawing;

namespace Kontur.ImageTransformer.Transformer.Filters
{
    public interface IThresholdFilter : IFilter
    {
        void Process(Bitmap image, int param);
    }
}
