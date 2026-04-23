using System;

namespace CurrencyAppWPF.Models
{
    public class Currency
    {
        public string Id { get; set; }              
        public string CharCode { get; set; }        
        public string NumCode { get; set; }         
        public int Nominal { get; set; }           
        public string Name { get; set; }            
        public double Value { get; set; }          
        public double Previous { get; set; }        
        public bool IsUserAdded { get; set; }       
        public DateTime AddedDate { get; set; }     
    }
}