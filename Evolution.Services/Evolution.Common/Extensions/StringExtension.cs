using Evolution.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Evolution.Common.Extensions
{
    ///TODO : Move all the Hard Coded Char into Constant.
    public static class StringExtension
    {
        public static bool ToTrueFalse(this string source) => (string.IsNullOrEmpty(source) ? false : (source.Trim().ToUpper() == "YES" ? true : false));

        public static bool ToTrueFalse(this string source, string destination) => (string.IsNullOrEmpty(source)) ? false : (source.Trim().ToUpper() == destination.Trim().ToUpper() ? true : false);

        public static bool IsRecordStatusNew(this string source) => (string.IsNullOrEmpty(source)) ? false : (source.Trim().ToUpper() == "N" ? true : false);

        public static bool IsRecordStatusDeleted(this string source) => (string.IsNullOrEmpty(source)) ? false : (source.Trim().ToUpper() == "D" ? true : false);

        public static bool IsRecordStatusModified(this string source) => (string.IsNullOrEmpty(source)) ? false : (source.Trim().ToUpper() == "M" ? true : false);

        public static string ToAssignmentStatus(this string source) => (string.IsNullOrEmpty(source) ? "P" : source.Trim().Substring(0, 1).ToUpper());

        public static string ToTimeSheetStatus(this string source) => (string.IsNullOrEmpty(source) ? "A" : source.Trim().Substring(0, 1).ToUpper());

        public static bool IsCustomerDocument(this string source) => (string.IsNullOrEmpty(source)) ? false : (source.Trim().ToUpper() == "C" ? true : false);

        public static bool IsCompanyDocument(this string source) => (string.IsNullOrEmpty(source)) ? false : (source.Trim().ToUpper() == "CO" ? true : false);

        public static bool IsContractDocument(this string source) => (string.IsNullOrEmpty(source)) ? false : (source.Trim().ToUpper() == "CON" ? true : false);

        public static bool IsProjectDocument(this string source) => (string.IsNullOrEmpty(source)) ? false : (source.Trim().ToUpper() == "P" ? true : false);

        public static bool IsSupplierDocument(this string source) => (string.IsNullOrEmpty(source)) ? false : (source.Trim().ToUpper() == "S" ? true : false);

        public static bool IsSupplierPODocument(this string source) => (string.IsNullOrEmpty(source)) ? false : (source.Trim().ToUpper() == "SPO" ? true : false);

        public static bool IsAssignmentDocument(this string source) => (string.IsNullOrEmpty(source)) ? false : (source.Trim().ToUpper() == "A" ? true : false);

        public static bool IsVisitDocument(this string source) => (string.IsNullOrEmpty(source)) ? false : (source.Trim().ToUpper() == "V" ? true : false);

        public static bool IsTimesheetDocument(this string source) => (string.IsNullOrEmpty(source)) ? false : (source.Trim().ToUpper() == "T" ? true : false);

        public static bool IsTechnicalSpecialistDocument(this string source) => (string.IsNullOrEmpty(source)) ? false : (source.Trim().ToUpper() == "TS" ? true : false);

        public static bool HasEvoWildCardChar(this string source) => string.IsNullOrEmpty(source) ? false : (source.StartsWith("*") || source.EndsWith("*"));

        public static bool IsContractClosed(this string source) => (string.IsNullOrEmpty(source)) ? false : (source.Trim().ToUpper() == "C" ? true : false);

        public static bool IsProjectClosed(this string source) => (string.IsNullOrEmpty(source)) ? false : (source.Trim().ToUpper() == "C" ? true : false);

        public static bool IsSalesTax(this string source) => (string.IsNullOrEmpty(source)) ? false : (source.Trim().ToUpper() == "S" ? true : false);

        public static bool IsWithholdingTax(this string source) => (string.IsNullOrEmpty(source)) ? false : (source.Trim().ToUpper() == "W" ? true : false);

        public static bool IsCompleted(this string source) => (string.IsNullOrEmpty(source)) ? false : (source.Trim().ToUpper() == "C" ? true : false);

        public static T ToEnum<T>(this string source, bool ignoreCase = true, bool throwException = true) where T : struct
        {
            return (T)ToEnum<T>(source, default(T), ignoreCase, throwException);
        }

        public static T ToEnum<T>(this string source, T defaultValue, bool ignoreCase = true, bool throwException = false) where T : struct
        {
            T returnEnum = defaultValue;

            if (!typeof(T).IsEnum || String.IsNullOrEmpty(source))
            {
                throw new InvalidOperationException("Invalid Enum Type or Input String 'source'. " + typeof(T).ToString() + "  must be an Enum");
            }

            try
            {
                bool success = Enum.TryParse<T>(source, ignoreCase, out returnEnum);
                if (!success && throwException)
                {
                    throw new InvalidOperationException("Invalid Cast");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Invalid Cast", ex);
            }

            return returnEnum;
        }

        public static bool IsValidFileExtenstionType(this string source, FileExtensionType type)
        {
            if (!string.IsNullOrEmpty(source) && source.ToLower().Contains(type.ToString().ToLower()))
                return true;
            else
                return false;
        }

        public static string RemoveEmptyRowAndControlChar(this string source)
        {
            if (!string.IsNullOrEmpty(source))
                return Regex.Replace(new string(source?.Select(c => !char.IsControl(c) ? c : ' ').ToArray<char>()), @"\s+", " ", RegexOptions.Multiline);
            else
                return source;
        }

        public static bool IsVisitEntry(this string source) => (string.IsNullOrEmpty(source)) ? false : (source.Trim().ToUpper() == "V" || source.Trim().ToUpper() == "S" ? true : false);

        public static bool ContainsDuplicates<T>(this IEnumerable<T> enumerable)
        {
            var keys = new HashSet<T>();
            return enumerable.All(item => !keys.Add(item));
        }
    }
}