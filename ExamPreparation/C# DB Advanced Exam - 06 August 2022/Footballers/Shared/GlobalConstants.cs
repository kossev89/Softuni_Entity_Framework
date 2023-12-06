using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Footballers.Shared
{
    public class GlobalConstants
    {
        //Footballers
        public const int footballerNameMin = 2;
        public const int footballerNameMax = 40;

        //Teams
        public const int teamNameMin = 3;
        public const int teamNameMax = 40;
        public const string nameRegex = @"^[A-Za-z0-9\s.-]+$";
        public const int teamNatMin = 2;
        public const int teamNatMax = 40;

        //Coaches
        public const int coachNameMin = 2;
        public const int coachNameMax = 40;
    }
}
