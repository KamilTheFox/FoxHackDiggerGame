using System.ComponentModel;

namespace Cyotek.Drawing.Imaging.ColorReduction
{
    [Description("Atkinson")]
    public sealed class AtkinsonDithering : ErrorDiffusionDithering
    {
        #region Constructors

        public AtkinsonDithering()
          : base(new byte[,]
                 {
               {
                 0, 0, 1, 1
               },
               {
                 1, 1, 1, 0
               },
               {
                 0, 1, 0, 0
               }
                 }, 3, true)
        { }

        #endregion
    }
}
