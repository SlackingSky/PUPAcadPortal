using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace PUPAcadPortal.PHAddress
{
    public class LocationItem
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentCode { get; set; }
    }


    public static class AddToAddressCMB
    {
        public static List<LocationItem> Regions { get; private set; } = new List<LocationItem>();
        public static List<LocationItem> Provinces { get; private set; } = new List<LocationItem>();
        public static List<LocationItem> Cities { get; private set; } = new List<LocationItem>();
        public static List<LocationItem> Barangays { get; private set; } = new List<LocationItem>();

        private static bool isLoaded = false;

        public static void LoadData()
        {
            if (isLoaded) return;
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string PHAddress = Path.Combine(basePath, "PHAddress");

            string regionsPath = Path.Combine(PHAddress, "refregion.xml");
            string provincesPath = Path.Combine(PHAddress, "refprovince.xml");
            string citiesPath = Path.Combine(PHAddress, "refcitymun.xml");
            string barangaysPath = Path.Combine(PHAddress, "refbrgy.xml");

            if (File.Exists(regionsPath))
            {
                Regions = XDocument.Load(regionsPath).Descendants("RECORD")
                    .Select(x => new LocationItem
                    {
                        Code = x.Element("regCode")?.Value ?? x.Element("id")?.Value,
                        Name = x.Element("regDesc")?.Value
                    }).OrderBy(r => r.Name).ToList();
            }
            if (File.Exists(provincesPath))
            {
                Provinces = XDocument.Load(provincesPath).Descendants("RECORD")
                    .Select(x => new LocationItem
                    {
                        Code = x.Element("provCode")?.Value,
                        Name = x.Element("provDesc")?.Value,
                        ParentCode = x.Element("regCode")?.Value
                    }).OrderBy(p => p.Name).ToList();
            }
            if (File.Exists(citiesPath))
            {
                Cities = XDocument.Load(citiesPath).Descendants("RECORD")
                    .Select(x => new LocationItem
                    {
                        Code = x.Element("citymunCode")?.Value,
                        Name = x.Element("citymunDesc")?.Value,
                        ParentCode = x.Element("provCode")?.Value
                    }).OrderBy(c => c.Name).ToList();
            }

            if (File.Exists(barangaysPath))
            {
                Barangays = XDocument.Load(barangaysPath).Descendants("RECORD")
                    .Select(x => new LocationItem
                    {
                        Code = x.Element("brgyCode")?.Value,
                        Name = x.Element("brgyDesc")?.Value,
                        ParentCode = x.Element("citymunCode")?.Value
                    }).OrderBy(b => b.Name).ToList();
            }

            isLoaded = true;
        }
    }
}
