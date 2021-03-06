//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibraryManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BillBorrow
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BillBorrow()
        {
            this.DetailBillBorrows = new HashSet<DetailBillBorrow>();
            this.DetailBillReturns = new HashSet<DetailBillReturn>();
        }
    
        public int idBillBorrow { get; set; }
        public System.DateTime borrowDate { get; set; }
        public int idReader { get; set; }
    
        public virtual Reader Reader { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetailBillBorrow> DetailBillBorrows { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetailBillReturn> DetailBillReturns { get; set; }
    }
}
