using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.DomainModel;
namespace lm.Comol.Modules.CallForPapers.Domain
{
     [Serializable]
    public class dtoCallField :dtoBase 
    {
        public virtual long IdSection { get; set; }
        public virtual String Name { get; set; }
        //public virtual String Text { get; set; }
        public virtual String Description { get; set; }
        public virtual int DisplayOrder { get; set; }
        public virtual int MaxLength { get; set; }
        public virtual FieldType Type { get; set; }
        public virtual Boolean Mandatory { get; set; }
        public virtual List<long> Submitters { get; set; }
        public virtual List<dtoFieldOption> Options { get; set; }
        public virtual Boolean isMultiple { get { return (Options != null); } }
        public virtual String ToolTip { get; set; }
        public virtual String RegularExpression { get; set; }
        public virtual Int32 MaxOption { get; set; }
        public virtual Int32 MinOption { get; set; }
        public virtual DisclaimerType DisclaimerType { get; set; }
        public virtual Boolean HasFreeOption {get { return isMultiple && Options.Where(o=> o.Deleted == BaseStatusDeleted.None && o.IsFreeValue ).Any();}}
        public dtoCallField()
            : base()
        {
        }

        public dtoCallField(long id, String name, int display)
            : base()
        {
            Id = id;
            DisplayOrder = display;
            Name = name;
        }
        public dtoCallField(FieldDefinition definition)
            : base()
        {
            CreateDto(definition);
            Submitters = new List<long>();
        }

        public dtoCallField(FieldDefinition definition, List<long> idSubmitters)
            : base()
        {
            CreateDto(definition);
            Submitters = idSubmitters;
        }

        private void CreateDto(FieldDefinition definition)
        {
            Id = definition.Id;
            if (definition.Section!=null)
                IdSection = definition.Section.Id;
            Deleted = definition.Deleted;
            DisplayOrder = definition.DisplayOrder;
            Name = definition.Name;
            Description = definition.Description;
            MaxLength = definition.MaxLength;
            Type = definition.Type;
            Mandatory = definition.Mandatory;
            ToolTip = definition.ToolTip;
            RegularExpression = definition.RegularExpression;
            Options = new List<dtoFieldOption>();
            switch (definition.Type) { 
                case FieldType.DropDownList:
                case FieldType.CheckboxList:
                case FieldType.RadioButtonList:
                    Options = definition.Options.Where(o=>o.Deleted== BaseStatusDeleted.None).OrderBy(o=>o.DisplayOrder).ThenBy(o=>o.Name).Select(o => new dtoFieldOption(o)).ToList();
                    MaxOption = definition.MaxOption;
                    MinOption = definition.MinOption;
                    break;
                case FieldType.Disclaimer:
                    Options = definition.Options.Where(o=>o.Deleted== BaseStatusDeleted.None).OrderBy(o=>o.DisplayOrder).ThenBy(o=>o.Name).Select(o => new dtoFieldOption(o)).ToList();
                    MaxOption = definition.MaxOption;
                    MinOption = definition.MinOption;
                    DisclaimerType = definition.DisclaimerType;
                    break;
            }
        }
    }
}