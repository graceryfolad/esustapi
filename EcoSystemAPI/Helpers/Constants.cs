using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHMSData.Helpers
{
    public static class Constants
    {
       public  const int ExamOngoing = 210;
       public const int ExamClosed = 210;
       public const int ExamOpened = 210;


    }

    public enum ExamStatus
    {
        ExamOngoing = 905,
        ExamClosed = 910,
        ExamOpened = 900,
       
    }
    public enum ExamLevel
    {
        Easy = 915,
        Intermediate = 916,
        Difficult = 917,

    }

    public enum ExamCategory
    {
        Primary = 925,
        JuniorSeconday = 926,
        SeniorSecondary = 927,
        Tertiary=928,
        Professional=929,
        General = 930

    }

    public enum UserType
    {
        Aggregator = 920,
        Admin = 915,
        SuperAdmin = 925

    }

}
