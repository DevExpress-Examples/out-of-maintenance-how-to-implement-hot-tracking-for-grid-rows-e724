Imports System.Drawing
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace HotTrack

    Namespace dxSample
        Public Enum GridHotTrackMode
            Cell
            Row
        End Enum
        Public Class GridHotTrackHelper
            Implements IDisposable

            Private mode As GridHotTrackMode
            Private targetView As GridView

            Private hotTrackColor_Renamed As Color
            Public Sub New(ByVal targetView As GridView, ByVal hotTrackColor As Color, Optional ByVal mode As GridHotTrackMode = GridHotTrackMode.Cell)
                Me.mode = mode
                Me.targetView = targetView
                Me.hotTrackColor_Renamed = hotTrackColor
                Subscribe()
            End Sub
            Public Property HotTrackColor() As Color
                Get
                    Return hotTrackColor_Renamed
                End Get
                Set(ByVal value As Color)
                    hotTrackColor_Renamed = value
                End Set
            End Property
            Protected Sub Subscribe()
                AddHandler targetView.MouseMove, AddressOf TargetView_MouseMove
                AddHandler targetView.MouseLeave, AddressOf TargetView_MouseLeave
                AddHandler targetView.ShownEditor, AddressOf TargetView_ShownEditor
                AddHandler targetView.RowCellStyle, AddressOf TargetView_RowCellStyle
            End Sub
            Protected Sub Unsubscribe()
                If targetView Is Nothing Then
                    Return
                End If
                SetHotTrack(GridControl.InvalidRowHandle)
                RemoveHandler targetView.MouseMove, AddressOf TargetView_MouseMove
                RemoveHandler targetView.MouseLeave, AddressOf TargetView_MouseLeave
                RemoveHandler targetView.ShownEditor, AddressOf TargetView_ShownEditor
                RemoveHandler targetView.RowCellStyle, AddressOf TargetView_RowCellStyle
            End Sub
            Private hotTrackedRow As Integer = GridControl.InvalidRowHandle
            Private hotTrackedColumn As GridColumn = Nothing
            Private Sub SetHotTrack(ByVal rowHandle As Integer, Optional ByVal column As GridColumn = Nothing)
                If hotTrackedRow <> rowHandle OrElse hotTrackedColumn IsNot column Then
                    RefreshAppearance(hotTrackedRow, hotTrackedColumn)
                End If
                Me.hotTrackedRow = rowHandle
                Me.hotTrackedColumn = column
                RefreshAppearance(hotTrackedRow, hotTrackedColumn)
            End Sub
            Private Sub RefreshAppearance(ByVal rowHandle As Integer, ByVal column As GridColumn)
                If rowHandle = GridControl.InvalidRowHandle Then
                    Return
                End If
                If targetView Is Nothing Then
                    Return
                End If
                Dim viewInfo = TryCast(targetView.GetViewInfo(), GridViewInfo)
                If viewInfo Is Nothing Then
                    Return
                End If
                Dim rowInfo = viewInfo.GetGridRowInfo(rowHandle)
                If rowInfo IsNot Nothing Then
                    RefreshAppearance(viewInfo, rowInfo, column)
                End If
                targetView.InvalidateRow(rowHandle)
            End Sub
            Private Sub RefreshAppearance(ByVal viewInfo As GridViewInfo, ByVal rowInfo As GridRowInfo, ByVal column As GridColumn)
                If column IsNot Nothing Then
                    Dim dataRow As GridDataRowInfo = TryCast(rowInfo, GridDataRowInfo)
                    If dataRow IsNot Nothing Then
                        Dim cell = dataRow.Cells(column)
                        If cell IsNot Nothing Then
                            cell.State = DevExpress.XtraGrid.Views.Base.GridRowCellState.Dirty
                        End If
                    End If
                Else
                    rowInfo.SetDataDirty()
                End If
            End Sub
            Private Sub TargetView_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
                If targetView.IsEditing Then
                    Return
                End If
                Dim hitInfo = targetView.CalcHitInfo(e.X, e.Y)
                SetHotTrack(hitInfo.RowHandle,If(mode = GridHotTrackMode.Cell, hitInfo.Column, Nothing))
            End Sub
            Private Sub TargetView_MouseLeave(ByVal sender As Object, ByVal e As EventArgs)
                SetHotTrack(GridControl.InvalidRowHandle)
            End Sub
            Private Sub TargetView_ShownEditor(ByVal sender As Object, ByVal e As EventArgs)
                SetHotTrack(GridControl.InvalidRowHandle)
            End Sub
            Private Sub TargetView_RowCellStyle(ByVal sender As Object, ByVal e As RowCellStyleEventArgs)
                If e.RowHandle = hotTrackedRow Then
                    If mode = GridHotTrackMode.Cell Then
                        If hotTrackedColumn Is e.Column AndAlso e.Column IsNot Nothing Then
                            e.Appearance.BackColor = HotTrackColor
                        End If
                    Else
                        e.Appearance.BackColor = HotTrackColor
                    End If
                End If
            End Sub
            Public Sub Dispose() Implements IDisposable.Dispose
                Unsubscribe()
                targetView = Nothing
            End Sub
        End Class
    End Namespace
End Namespace
