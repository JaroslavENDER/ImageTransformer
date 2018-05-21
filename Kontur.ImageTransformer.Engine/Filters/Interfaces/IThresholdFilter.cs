using System.Drawing;

namespace Kontur.ImageTransformer.Engine.Filters
{
    public interface IThresholdFilter : IFilter
    {
        void Process(Bitmap image, int param);
    }
}
