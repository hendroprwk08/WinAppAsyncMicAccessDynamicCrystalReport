Imports System.Threading.Tasks
Imports System.Security.Cryptography
Imports System.Text
Public Class FBarang
    Dim tempID As Integer
    Dim lst As ListViewItem
    Private Sub FBarang_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call loadGrid(Nothing)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim sql As String

        If tempID = 0 Then
            sql = "insert into barang (nama, stok, jenis) " & _
            "values ('" & TextBox1.Text.Trim & "' , '" & TextBox2.Text.Trim & "', " & _
            "'" & ComboBox1.Text.Trim & "')"
        Else
            sql = "update barang set nama = '" & TextBox1.Text.Trim & "', " & _
                "stok = '" & TextBox2.Text.Trim & "', jenis = '" & ComboBox1.Text.Trim & "' " & _
                "where id_barang = " & tempID
        End If

        MProgress.showProgress(ProgressBar1)
        Dim myTask = Task.Factory.StartNew(Sub() MKoneksi.exec(sql))
        Task.WaitAll(myTask) 'menunggu hingga selesai
        MProgress.hideProgress(ProgressBar1)

        kosong()
        Call loadGrid(Nothing)
    End Sub
    Private Sub kosong()
        tempID = Nothing
        TextBox1.Text = Nothing
        TextBox2.Text = Nothing

        ComboBox1.SelectedIndex = 0
    End Sub
    Async Function loadGrid(ByVal cari As String) As Task
        MProgress.showProgress(ProgressBar1)

        Dim sql As String

        If cari = Nothing Then
            sql = "select * from barang"
        Else
            sql = "select * from barang " & _
                    "where nama like '%" & cari & "%'"
        End If

        Dim dt As DataTable = Await Task(Of DataTable).Factory.StartNew(Function() MKoneksi.getList(sql))

        ListView1.Items.Clear()
        For Each dr As DataRow In dt.Rows
            lst = ListView1.Items.Add(dr(0))
            lst.SubItems.Add(dr(1))
            lst.SubItems.Add(dr(2))
            lst.SubItems.Add(dr(3))

        Next

        tempID = Nothing
        MProgress.hideProgress(ProgressBar1)
    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim sql As String = "delete from barang where id_barang = " & tempID

        MProgress.showProgress(ProgressBar1)
        Dim myTask = Task.Factory.StartNew(Sub() MKoneksi.exec(sql))
        Task.WaitAll(myTask) 'menunggu hingga selesai
        MProgress.hideProgress(ProgressBar1)

        kosong()
        Call loadGrid(Nothing)
    End Sub

    Private Sub ListView1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseDoubleClick
        With ListView1
            tempID = .SelectedItems.Item(0).Text
            TextBox1.Text = .SelectedItems.Item(0).SubItems(1).Text
            TextBox2.Text = .SelectedItems.Item(0).SubItems(2).Text
            ComboBox1.Text = .SelectedItems.Item(0).SubItems(3).Text
        End With

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Call loadGrid(TextBox3.Text.Trim)
    End Sub
End Class