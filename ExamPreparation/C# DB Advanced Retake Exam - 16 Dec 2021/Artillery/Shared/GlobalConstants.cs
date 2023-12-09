using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Artillery.Shared
{
    public class GlobalConstants
    {
        //Country
        public const int countryNameMin = 4;
        public const int countryNameMax = 60;
        public const int countryArmyMin = 50_000;
        public const int countryArmyMax = 10_000_000;

        //Manufacturer
        public const int manufacturerNameMin = 4;
        public const int manufacturerNameMax = 40;
        public const int manufacturerFoundedMin = 10;
        public const int manufacturerFoundedMax = 100;

        //Shell
        public const double shellWeightMin = 2;
        public const double shellWeightMax = 1_680;
        public const int shellCaliberMin = 4;
        public const int shellCaliberMax = 30;

        //Gun
        public const int gunWeightMin = 100;
        public const int gunWeightMax = 1_350_000;
        public const double gunBarrelMin = 2.00;
        public const double gunBarrelMax = 35.00;
        public const int gunRangeMin = 1;
        public const int gunRangeMax = 100_000;

    }
}
