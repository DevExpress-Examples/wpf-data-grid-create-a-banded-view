using DevExpress.Xpf.Grid.Native;
using System.Collections.Generic;
using DevExpress.Xpf.Grid;
namespace BandedViewExtension {
    public class BandedViewColumnsLayoutCalculator : AutoWidthColumnsLayoutCalculator {
        public BandedViewColumnsLayoutCalculator(GridViewInfo viewInfo)
            : base(viewInfo) {
        }
        public override void ApplyResize(ColumnBase resizeColumn, double newWidth, double maxWidth, double indentWidth, bool correctWidths) {
            ColumnsLayoutControl c = BandedViewBehavior.GetColumnsLayoutControl(resizeColumn);
            c.Resize(resizeColumn, newWidth);
        }
        protected override void UpdateHasLeftRightSibling(IList<ColumnBase> columns) {
            for(int i = 0; i < columns.Count; i++) {
                BandedViewBehavior.GetIsRightColumn(columns[i]);
                columns[i].HasRightSibling = !BandedViewBehavior.GetIsRightColumn(columns[i]);
                columns[i].HasLeftSibling = !BandedViewBehavior.GetIsLeftColumn(columns[i]);
            }
        }
    }
    public class BandedViewLayoutCalculatorFactory : GridTableViewLayoutCalculatorFactory {
        public override ColumnsLayoutCalculator CreateCalculator(GridViewInfo viewInfo, bool autoWidth) {
            return new BandedViewColumnsLayoutCalculator(viewInfo);
        }
    }
}