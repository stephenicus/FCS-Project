using System;
using System.Collections.Generic;

namespace FCS_Funding.Models
{
    public partial class Donation
    {
        public Donation()
        {
            this.DonationPurposes = new List<DonationPurpose>();
            this.Expenses = new List<Expense>();
            this.In_Kind_Item = new List<In_Kind_Item>();
            this.In_Kind_Service = new List<In_Kind_Service>();
        }

        public int DonationID { get; set; }
        public Nullable<int> EventID { get; set; }
        public int DonorID { get; set; }
        public Nullable<int> RequestForPersonalID { get; set; }
        public bool Restricted { get; set; }
        public bool InKind { get; set; }
        public decimal DonationAmount { get; set; }
        public System.DateTime DonationDate { get; set; }
        public virtual Donor Donor { get; set; }
        public virtual ICollection<DonationPurpose> DonationPurposes { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual ICollection<In_Kind_Item> In_Kind_Item { get; set; }
        public virtual ICollection<In_Kind_Service> In_Kind_Service { get; set; }
    }
}