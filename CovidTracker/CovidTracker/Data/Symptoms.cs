using System.Collections.Generic;
using CovidTracker.Localization;

namespace CovidTracker
{
    public class Symptoms
    {
        // IMPORTANT -- This map MUST be kept in sync with the variables defined in the class
        //
        public static readonly Dictionary<string, string> SymptomsDict = new Dictionary<string, string> {
            { "tested_positive", LocResources.tested_positive },
            { "fever", LocResources.fever },
            { "tiredeness", LocResources.tiredeness },
            { "dry_cough", LocResources.dry_cough },
            { "aches_and_pains", LocResources.aches_and_pains },
            { "nasal_congestion", LocResources.nasal_congestion },
            { "runny_nose", LocResources.runny_nose },
            { "sore_throat", LocResources.sore_throat },
            { "diarrhea", LocResources.diarrhea },
        };

        public bool tested_positive;
        public bool fever;
        public bool tiredeness;
        public bool dry_cough;
        public bool aches_and_pains;
        public bool nasal_congestion;
        public bool runny_nose;
        public bool sore_throat;
        public bool diarrhea;
    }
}
