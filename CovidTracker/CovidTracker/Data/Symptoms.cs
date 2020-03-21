using System;
namespace CovidTracker
{
    public class Symptoms
    {
        /* ---------------------------------------------------------------------------------- 
         * SYMPTOMS LIST -- IMPORTANT: must be kept in sync with the variables of the class
         */
        public static readonly string[][] LIST = {
            new string[] { "tested_positive", "Tested Positive" },
            new string[] { "fever", "Fever" },
            new string[] { "tiredeness", "Tiredeness" },
            new string[] { "dry_cough", "Dry Cough" },
            new string[] { "aches_and_pains", "Aches and pains" },
            new string[] { "nasal_congestion", "Nasal congestion" },
            new string[] { "runny_nose", "Runny nose" },
            new string[] { "sore_throat", "sore throat" },
            new string[] { "diarrhea", "diarrhea" }
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
