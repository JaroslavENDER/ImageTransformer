using System.Drawing;

namespace Kontur.ImageTransformer.Engine.Filters
{
    public interface IThresholdFilter : IFilter
    {
        Bitmap Process(Bitmap image, int param);
    }
}
