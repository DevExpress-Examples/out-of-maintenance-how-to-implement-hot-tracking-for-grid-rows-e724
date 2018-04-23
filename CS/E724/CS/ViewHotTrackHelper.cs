using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotTrack {
    using System;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraGrid.Views.Grid.ViewInfo;
    using System.Drawing;

    namespace dxSample {
        public enum GridHotTrackMode { Cell, Row };
        public class GridHotTrackHelper : IDisposable {
            GridHotTrackMode mode;
            GridView targetView;
            Color hotTrackColor;
            public GridHotTrackHelper(GridView targetView, Color hotTrackColor, GridHotTrackMode mode = GridHotTrackMode.Cell) {
                this.mode = mode;
                this.targetView = targetView;
                this.hotTrackColor = hotTrackColor;
                Subscribe();
            }
            public Color HotTrackColor {
                get { return hotTrackColor; }
                set { hotTrackColor = value; }
            }
            protected void Subscribe() {
                targetView.MouseMove += TargetView_MouseMove;
                targetView.MouseLeave += TargetView_MouseLeave;
                targetView.ShownEditor += TargetView_ShownEditor;
                targetView.RowCellStyle += TargetView_RowCellStyle;
            }
            protected void Unsubscribe() {
                if (targetView == null) return;
                SetHotTrack(GridControl.InvalidRowHandle);
                targetView.MouseMove -= TargetView_MouseMove;
                targetView.MouseLeave -= TargetView_MouseLeave;
                targetView.ShownEditor -= TargetView_ShownEditor;
                targetView.RowCellStyle -= TargetView_RowCellStyle;
            }
            int hotTrackedRow = GridControl.InvalidRowHandle;
            GridColumn hotTrackedColumn = null;
            void SetHotTrack(int rowHandle, GridColumn column = null) {
                if (hotTrackedRow != rowHandle || hotTrackedColumn != column) {
                    RefreshAppearance(hotTrackedRow, hotTrackedColumn);
                }
                this.hotTrackedRow = rowHandle;
                this.hotTrackedColumn = column;
                RefreshAppearance(hotTrackedRow, hotTrackedColumn);
            }
            void RefreshAppearance(int rowHandle, GridColumn column) {
                if (rowHandle == GridControl.InvalidRowHandle) return;
                if (targetView == null) return;
                var viewInfo = targetView.GetViewInfo() as GridViewInfo;
                if (viewInfo == null) return;
                var rowInfo = viewInfo.GetGridRowInfo(rowHandle);
                if (rowInfo != null) RefreshAppearance(viewInfo, rowInfo, column);
                targetView.InvalidateRow(rowHandle);
            }
            void RefreshAppearance(GridViewInfo viewInfo, GridRowInfo rowInfo, GridColumn column) {
                if (column != null) {
                    GridDataRowInfo dataRow = rowInfo as GridDataRowInfo;
                    if (dataRow != null) {
                        var cell = dataRow.Cells[column];
                        if (cell != null) cell.State = DevExpress.XtraGrid.Views.Base.GridRowCellState.Dirty;
                    }
                }
                else {
                    rowInfo.SetDataDirty();
                }
            }
            void TargetView_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) {
                if (targetView.IsEditing) return;
                var hitInfo = targetView.CalcHitInfo(e.X, e.Y);
                SetHotTrack(hitInfo.RowHandle, mode == GridHotTrackMode.Cell ? hitInfo.Column : null);
            }
            void TargetView_MouseLeave(object sender, EventArgs e) { SetHotTrack(GridControl.InvalidRowHandle); }
            void TargetView_ShownEditor(object sender, EventArgs e) { SetHotTrack(GridControl.InvalidRowHandle); }
            void TargetView_RowCellStyle(object sender, RowCellStyleEventArgs e) {
                if (e.RowHandle == hotTrackedRow) {
                    if (mode == GridHotTrackMode.Cell) {
                        if (hotTrackedColumn == e.Column && e.Column != null) e.Appearance.BackColor = HotTrackColor;
                    }
                    else
                        e.Appearance.BackColor = HotTrackColor;
                }
            }
            public void Dispose() {
                Unsubscribe();
                targetView = null;
            }
        }
    }
}
