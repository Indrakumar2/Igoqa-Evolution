using System.ComponentModel.DataAnnotations;

namespace Evolution.Visit.Domain.Enum
{
    public enum ExpenseType
    {
        [Display(Name = "R")]
        R,  //Time
        [Display(Name = "C")]
        C,  //Consumables
        [Display(Name = "T")]
        T, // Travel
        [Display(Name = "Q")]
        Q, //Quipment
        [Display(Name = "E")]
        E  //Expense
    }
}
