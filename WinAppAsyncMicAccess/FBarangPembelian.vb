Imports System.Threading.Tasks

Public Class FBarangPembelian

    Dim dtBarang As DataTable


    Sub hitung()
        Label4.Text = Val(TextBox1.Text.Trim) * Val(TextBox2.Text.Trim)
    End Sub
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        hitung()
    End Sub

    Private Sub FBarangPembelian_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call loadBarang()
    End Sub

    Async Function loadBarang() As Threading.Tasks.Task
        Dim sql As String = "select id_barang, nama from barang"

        MProgress.showProgress(ProgressBar1)

        'ambil data table
        dtBarang = Await Task(Of DataTable).Factory.StartNew(Function() MKoneksi.getList(sql))

        ComboBox1.DataSource = dtBarang
        ComboBox1.DisplayMember = "nama"
        ComboBox1.ValueMember = "id_barang"

        ComboBox1_SelectedIndexChanged(Nothing, Nothing)

        MProgress.hideProgress(ProgressBar1)
    End Function

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim lst As ListViewItem
        With FPembelian.ListView1
            lst = .Items.Add(ComboBox1.SelectedValue.ToString)
            lst.SubItems.Add(ComboBox1.Text)
            lst.SubItems.Add(TextBox2.Text)
            lst.SubItems.Add(TextBox1.Text)
            lst.SubItems.Add(Label4.Text)
        End With
        FPembelian.hitungtotal()

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        hitung()
    End Sub
End Class