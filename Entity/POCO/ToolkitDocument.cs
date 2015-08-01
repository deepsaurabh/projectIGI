using Core;
using System;

namespace Entity.POCO
{
   public class ToolkitDocument : BaseEntity
    {
       public Int64 ToolkitId { get; set; }
        public DocumentType DocumentType { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentName { get; set; }    
        public string DocumentPseudoName { get; set; }
        public DocumentScope Scope { get; set; }
        public virtual Toolkit Toolkit { get; set; } 
    }
}
