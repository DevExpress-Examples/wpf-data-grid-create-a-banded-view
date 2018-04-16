using System;
using DevExpress.Xpf.Grid;
namespace BandedViewExtension {
    public class InvalidColumnPropertyValueException : InvalidOperationException {
        public ColumnBase GridColumn { get; private set; }
        public string GridColumnPropertyName { get; private set; }
        public override string Message {
            get { return "You cannot set the " + GridColumnPropertyName + "property for the GridColumn when BandedBehavior is used."; }
        }
        public InvalidColumnPropertyValueException(ColumnBase column, string propertyName) {
            GridColumn = column;
            GridColumnPropertyName = propertyName;
        }
    }
}