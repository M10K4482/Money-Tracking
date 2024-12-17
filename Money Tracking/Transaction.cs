using System;

namespace Money_Tracking
{

    //A class with constructor and get set for the different user entered values
    class Transaction
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }

}
