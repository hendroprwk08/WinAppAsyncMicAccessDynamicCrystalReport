Imports System.Threading.Tasks

Public Class FCariPembelian
    Dim lst As ListViewItem

    Private Sub FCariPembelian_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initGrid()
    End Sub

    Sub initGrid()
        With ListView1
            .View = View.Details
            .GridLines = True
            .FullRowSelect = True
            .Columns.Clear()
            .Columns.Add("#", 50, HorizontalAlignment.Left)
            .Columns.Add("Tgl", 120, HorizontalAlignment.Left)
            .Columns.Add("Nama", 100, HorizontalAlignment.Left)
        End With
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Call cari(TextBox1.Text.Trim)
    End Sub

    Async Function cari(ByVal s As String) As Task

        Dim sql As String

        If s = Nothing Then
            sql = "select * from vlistpembelian"
        Else
            sql = "select * from vlistpembelian where nama like '%" & s & "%' " & _
                    "or id_beli like '%" & s & "%'"
        End If

        Dim dt As DataTable = Await Task(Of DataTable).Factory.StartNew(Function() MKoneksi.getList(sql))

        ListView1.Items.Clear()

        For Each dr As DataRow In dt.Rows
            lst = ListView1.Items.Add(dr(0))
            lst.SubItems.Add(dr(1))
            lst.SubItems.Add(dr(3))
        Next
    End Function

    Private Sub ListView1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListView1.MouseDoubleClick
        With ListView1
            Dim id As Integer = .SelectedItems.Item(0).Text
            
            'nanti dibuat di fpembelian fungsi untuk load data
            Call FPembelian.display(id)
            Hide()
        End With
    End Sub
End Class