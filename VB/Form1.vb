Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo

Namespace HotTrack
	''' <summary>
	''' Summary description for Form1.
	''' </summary>
	Public Class Form1
		Inherits System.Windows.Forms.Form
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private WithEvents gridControl1 As DevExpress.XtraGrid.GridControl
		Private WithEvents gridView1 As DevExpress.XtraGrid.Views.Grid.GridView

		Private components As System.ComponentModel.Container = Nothing

		Public Sub New()
			InitializeComponent()
		End Sub

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		Protected Overrides Overloads Sub Dispose(ByVal disposing As Boolean)
			If disposing Then
				If components IsNot Nothing Then
					components.Dispose()
				End If
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"
		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.gridControl1 = New DevExpress.XtraGrid.GridControl()
			Me.gridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
			CType(Me.gridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gridView1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' gridControl1
			' 
			Me.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.gridControl1.EmbeddedNavigator.Name = ""
			Me.gridControl1.Location = New System.Drawing.Point(0, 0)
			Me.gridControl1.MainView = Me.gridView1
			Me.gridControl1.Name = "gridControl1"
			Me.gridControl1.Size = New System.Drawing.Size(466, 256)
			Me.gridControl1.TabIndex = 0
			Me.gridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() { Me.gridView1})
'			Me.gridControl1.MouseLeave += New System.EventHandler(Me.gridControl1_MouseLeave);
			' 
			' gridView1
			' 
			Me.gridView1.GridControl = Me.gridControl1
			Me.gridView1.Name = "gridView1"
'			Me.gridView1.RowCellStyle += New DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(Me.gridView1_RowCellStyle);
'			Me.gridView1.MouseMove += New System.Windows.Forms.MouseEventHandler(Me.gridView1_MouseMove);
			' 
			' Form1
			' 
			Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
			Me.ClientSize = New System.Drawing.Size(466, 256)
			Me.Controls.Add(Me.gridControl1)
			Me.Name = "Form1"
			Me.Text = "Form1"
'			Me.Load += New System.EventHandler(Me.Form1_Load);
			CType(Me.gridControl1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gridView1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
		#End Region

		''' <summary>
		''' The main entry point for the application.
		''' </summary>
		<STAThread> _
		Shared Sub Main()
			Application.Run(New Form1())
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
			' load some sample data
			Dim TempXViewsPrinting As DevExpress.XtraGrid.Design.XViewsPrinting = New DevExpress.XtraGrid.Design.XViewsPrinting(gridControl1)
		End Sub

		Private hotTrackRow_Renamed As Integer = DevExpress.XtraGrid.GridControl.InvalidRowHandle

		Private Property HotTrackRow() As Integer
			Get
				Return hotTrackRow_Renamed
			End Get
			Set(ByVal value As Integer)
				If hotTrackRow_Renamed <> value Then
					Dim prevHotTrackRow As Integer = hotTrackRow_Renamed
					hotTrackRow_Renamed = value

					gridView1.RefreshRow(prevHotTrackRow)
					gridView1.RefreshRow(hotTrackRow_Renamed)

					If hotTrackRow_Renamed >= 0 Then
						gridControl1.Cursor = Cursors.Hand
					Else
						gridControl1.Cursor = Cursors.Default
					End If
				End If
			End Set
		End Property
		Private Sub gridView1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles gridView1.MouseMove
			Dim view As GridView = TryCast(sender, GridView)
			Dim info As GridHitInfo = view.CalcHitInfo(New Point(e.X, e.Y))

			If info.InRowCell Then
				HotTrackRow = info.RowHandle
			Else
				HotTrackRow = DevExpress.XtraGrid.GridControl.InvalidRowHandle
			End If
		End Sub
		Private Sub gridControl1_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridControl1.MouseLeave
			HotTrackRow = DevExpress.XtraGrid.GridControl.InvalidRowHandle
		End Sub
		Private Sub gridView1_RowCellStyle(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs) Handles gridView1.RowCellStyle
			If e.RowHandle = HotTrackRow Then
				e.Appearance.BackColor = Color.PaleGoldenrod
			End If
		End Sub
	End Class
End Namespace
