using PUPAcadPortal.PHAddress;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace PUPAcadPortal.Utils
{
    public static class InfoFormatter
    {
        public static string FormatPhilippinePhoneNumber(string rawPhone)
        {
            if (string.IsNullOrWhiteSpace(rawPhone)) return "";

            string clean = new string(rawPhone.Where(char.IsDigit).ToArray());

            if (clean.Length == 10 && clean.StartsWith("9"))
            {
                clean = "0" + clean;
            }

            if (clean.Length == 11 && clean.StartsWith("0"))
            {
                return $"(+63) {clean.Substring(1, 3)}-{clean.Substring(4, 3)}-{clean.Substring(7, 4)}";
            }

            if (clean.Length == 12 && clean.StartsWith("63"))
            {
                return $"(+{clean.Substring(0, 2)}) {clean.Substring(2, 3)}-{clean.Substring(5, 3)}-{clean.Substring(8, 4)}";
            }

            return rawPhone;
        }

        public static string NormalizeCity(string rawCity)
        {
            if (string.IsNullOrWhiteSpace(rawCity)) return "";

            string normalizedInput = rawCity.Trim().ToUpper();

            normalizedInput = normalizedInput.Replace("STA.", "SANTA")
                                             .Replace("STA ", "SANTA ")
                                             .Replace("STO.", "SANTO")
                                             .Replace("STO ", "SANTO ");

            AddToAddressCMB.LoadData();

            var exactMatch = AddToAddressCMB.Cities
                .FirstOrDefault(c => c.Name.ToUpper() == normalizedInput);

            if (exactMatch != null)
            {
                return exactMatch.Name;
            }

            return normalizedInput;
        }

        public static string NormalizeRegion(string rawRegion)
        {
            if (string.IsNullOrWhiteSpace(rawRegion)) return "";

            AddToAddressCMB.LoadData();

            string normalizedInput = rawRegion.Trim().ToUpper();

            var exactMatch = AddToAddressCMB.Regions
                .FirstOrDefault(r => r.Name.ToUpper() == normalizedInput);

            if (exactMatch != null)
            {
                return exactMatch.Name;
            }

            string searchKey = normalizedInput switch
            {
                "REGION 1" or "ILOCOS" => "REGION I ",
                "REGION 2" or "CAGAYAN VALLEY" => "REGION II ",
                "REGION 3" or "CENTRAL LUZON" => "REGION III",
                "REGION 4A" or "CALABARZON" => "REGION IV-A",
                "REGION 4B" or "MIMAROPA" => "MIMAROPA",
                "REGION 5" or "BICOL" => "REGION V ",
                "REGION 6" or "WESTERN VISAYAS" => "REGION VI",
                "REGION 7" or "CENTRAL VISAYAS" => "REGION VII",
                "REGION 8" or "EASTERN VISAYAS" => "REGION VIII",
                "NCR" or "METRO MANILA" => "NCR",
                "CAR" or "CORDILLERA" => "CAR",
                "BARMM" or "ARMM" => "BARMM",
                _ => normalizedInput
            };

            var fuzzyMatch = AddToAddressCMB.Regions
                .FirstOrDefault(r => r.Name.ToUpper().Contains(searchKey));

            return fuzzyMatch != null ? fuzzyMatch.Name : normalizedInput;
        }
    }
}
