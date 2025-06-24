using Autodesk.Revit.DB;
using System;

namespace Libs.RevitAPI._Common
{
    public class UnitConverter
    {
        public static double MMToInternal(double value)
        {

#if R2021 || R2022 || R2023 || R2024 || R2025
            return UnitUtils.ConvertToInternalUnits(value, UnitTypeId.Millimeters);
#else
            return UnitUtils.ConvertToInternalUnits(value, DisplayUnitType.DUT_MILLIMETERS);
#endif
        }

        public static double Milimetter2Feet(double a)
        {

#if R2021 || R2022 || R2023 || R2024 || R2025

            return UnitUtils.Convert(a, UnitTypeId.Millimeters, UnitTypeId.FeetFractionalInches);
#else
            return UnitUtils.Convert(a, DisplayUnitType.DUT_MILLIMETERS, DisplayUnitType.DUT_DECIMAL_FEET);
#endif
        }
        public static double Feet2Milimetter(double a)
        {

#if R2021 || R2022 || R2023 || R2024 || R2025
            return UnitUtils.Convert(a, UnitTypeId.FeetFractionalInches, UnitTypeId.Millimeters);
#else
            return UnitUtils.Convert(a, DisplayUnitType.DUT_DECIMAL_FEET, DisplayUnitType.DUT_MILLIMETERS);
#endif
        }

        public static double Inch2Milimetter(double a)
        {

#if R2021 || R2022 || R2023 || R2024 || R2025
            return UnitUtils.Convert(a, UnitTypeId.Inches, UnitTypeId.Millimeters);
#else
            return UnitUtils.Convert(a, DisplayUnitType.DUT_DECIMAL_INCHES, DisplayUnitType.DUT_MILLIMETERS);
#endif

        }
        public static double Milimetter2Inch(double a)
        {

#if R2021 || R2022 || R2023 || R2024 || R2025
            return UnitUtils.Convert(a, UnitTypeId.Millimeters, UnitTypeId.Inches);
#else
            return UnitUtils.Convert(a, DisplayUnitType.DUT_MILLIMETERS, DisplayUnitType.DUT_DECIMAL_INCHES);
#endif

        }
        public static double Degree2Radian(double a)
        {

#if R2021 || R2022 || R2023 || R2024 || R2025
            return UnitUtils.Convert(a, UnitTypeId.Degrees, UnitTypeId.Radians);
#else
            return UnitUtils.Convert(a, DisplayUnitType.DUT_DECIMAL_DEGREES, DisplayUnitType.DUT_RADIANS);
#endif

        }
        public static double Radian2Degree(double a)
        {

#if R2021 || R2022 || R2023 || R2024 || R2025
            return UnitUtils.Convert(a, UnitTypeId.Radians, UnitTypeId.Degrees);
#else
            return UnitUtils.Convert(a, DisplayUnitType.DUT_RADIANS, DisplayUnitType.DUT_DECIMAL_DEGREES);
#endif

        }

        public static double AngleVector( XYZ vec1, XYZ vec2)
        {
            return Math.Acos((vec1.X * vec2.X + vec1.Y * vec2.Y + vec1.Z * vec2.Z) / (Math.Sqrt(vec1.X * vec1.X + vec1.Y * vec1.Y + vec1.Z * vec1.Z) * Math.Sqrt(vec2.X * vec2.X + vec2.Y * vec2.Y + vec2.Z * vec2.Z)));
        }
    }
}