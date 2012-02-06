using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;


namespace Btl.UserControls
{
    /// <summary>
    /// List of installed fonts.  
    /// See - http://peteohanlon.wordpress.com/2009/03/13/fun-with-fonts/
    /// </summary>
    public class InstalledFonts : List<FontFamily>
    {
        public InstalledFonts()
        {
            using (InstalledFontCollection fonts = new InstalledFontCollection())
            {
                this.AddRange(fonts.Families);
            }
        }
    }
}
 