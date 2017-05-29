using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace lm.Comol.Core.DomainModel.DocTemplateVers.Helpers
{
    /// <summary>
    /// Measure converter, (72px/inch)
    /// </summary>
    [Serializable]
    public static class Measure
    {
        /// <summary>
        /// Convert mm to Px (72px/inch)
        /// </summary>
        /// <param name="mm">value in mm</param>
        /// <returns>value in Px</returns>
        public static Single mm_To_Px(Single mm)
        {
            return (float) System.Math.Round(mm / 25.4f * 72, 0);
        }
        /// <summary>
        /// Convert mm to cm
        /// </summary>
        /// <param name="mm">value in mm</param>
        /// <returns>value in cm</returns>
        public static Single mm_To_cm(Single mm)
        {
            return (float) System.Math.Round(mm / 10, 1);
        }
        /// <summary>
        /// Convert mm to Inch
        /// </summary>
        /// <param name="mm">value in mm</param>
        /// <returns>value in inch</returns>
        public static Single mm_To_Inch(Single mm)
        {
            return (float) System.Math.Round(mm / 25.4f, 2);
        }


        /// <summary>
        /// Convert cm to Px (72px/inch)
        /// </summary>
        /// <param name="mm">value in cm</param>
        /// <returns>value in Px</returns>
        public static Single cm_To_Px(Single cm)
        {
            return (float) System.Math.Round(cm / 2.54f * 72, 0);
        }
        /// <summary>
        /// Convert cm to mm
        /// </summary>
        /// <param name="mm">value in cm</param>
        /// <returns>value in mm</returns>
        public static Single cm_To_mm(Single cm)
        {
            return (float) System.Math.Round(cm * 10, 0);
        }
        /// <summary>
        /// Convert cm to inch
        /// </summary>
        /// <param name="mm">value in cm</param>
        /// <returns>value in inch</returns>
        public static Single cm_To_Inch(Single cm)
        {
            return (float) System.Math.Round(cm / 2.54f, 2);
        }


        /// <summary>
        /// Convert Px to cm (72px/inch)
        /// </summary>
        /// <param name="mm">value in px</param>
        /// <returns>value in cm</returns>
        public static Single Px_To_cm(Single Px)
        {
            return (float) System.Math.Round(Px / 72 * 2.54f, 1);
        }
        /// <summary>
        /// Convert Px to mm (72px/inch)
        /// </summary>
        /// <param name="mm">value in px</param>
        /// <returns>value in mm</returns>
        public static Single Px_To_mm(Single Px)
        {
            return (float) System.Math.Round(Px / 72 * 25.4f, 0);
        }
        /// <summary>
        /// Convert Px to inch (72px/inch)
        /// </summary>
        /// <param name="mm">value in px</param>
        /// <returns>value in inch</returns>
        public static Single Px_To_Inch(Single Px)
        {
            return (float) System.Math.Round(Px / 72, 2);
        }

        /// <summary>
        /// Convert inch to cm
        /// </summary>
        /// <param name="mm">value in inch</param>
        /// <returns>value in cm</returns>
        public static Single Inch_To_cm(Single Inch)
        {
            return (float) System.Math.Round(Inch * 2.54f, 1);
        }
        /// <summary>
        /// Convert inch to mm
        /// </summary>
        /// <param name="mm">value in inch</param>
        /// <returns>value in mm</returns>
        public static Single Inch_To_mm(Single Inch)
        {
            return (float) System.Math.Round(Inch * 25.4f, 0);
        }
        /// <summary>
        /// Convert inch to px (72px/inch)
        /// </summary>
        /// <param name="mm">value in inch</param>
        /// <returns>value in px</returns>
        public static Single Inch_To_Px(Single Inch)
        {
            return (float) System.Math.Round(Inch * 72, 0);
        }

        /// <summary>
        /// Get Pagesize from specific format
        /// </summary>
        /// <param name="mm">value in inch</param>
        /// <returns>value in cm</returns>
        public static PageSizeValue GetSize(PageSize Size, String Measure)
        {
            PageSizeValue OutSize = new PageSizeValue();
            OutSize.Width = 0;
            OutSize.Height = 0;
            
            //ToDo...

            return OutSize;
        }
    }


    [Serializable]
    public class PageSizeValue
    {
        public Single Width { get; set; }
        public Single Height { get; set; }
    }


}
