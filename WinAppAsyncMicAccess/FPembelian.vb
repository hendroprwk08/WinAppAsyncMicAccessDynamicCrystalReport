Imports System.Threading.Tasks

Public Class FPembelian
    Dim dtSupplier As DataTable
    Dim TEMP_ID As String
    Dim lst As ListViewItem

    Private Sub FPembelian_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call loadSupplier()
        initPembelian()
    End Sub

    Async Function loadSupplier() As Threading.Tasks.Task
        'ambil data supplier dan tampilkan dalam bentuk key - value 
        'seperti <option value="xxx">yyy</option> pada HTML
        MProgress.showProgress(ProgressBar1)

        Dim sql As String = "select id_supplier, nama, alamat, telepon from supplier"

        'ambil data table
        dtSupplier = Await Task(Of DataTable).Factory.StartNew(Function() MKoneksi.getList(sql))

        ComboBox1.DataSource = dtSupplier
        ComboBox1.DisplayMember = "nama"
        ComboBox1.ValueMember = "id_supplier"

        ComboBox1_SelectedIndexChanged(Nothing, Nothing)

        MProgress.hideProgress(ProgressBar1)
    End Function

    Sub initPembelian()
        With ListView1
            .View = View.Details
            .GridLines = True
            .FullRowSelect = True
            .Columns.Clear()
            .Columns.Add("IDBarang", 120, HorizontalAlignment.Left)
            .Columns.Add("Nama", 200, HorizontalAlignment.Left)
            .Columns.Add("Qty", 70, HorizontalAlignment.Right)
            .Columns.Add("Harga", 120, HorizontalAlignment.Right)
            .Columns.Add("Jumlah", 120, HorizontalAlignment.Right)
        End With
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim sid As String = ComboBox1.SelectedValue.ToString

        'ambil alamat yang tertapung pada datatable 
        Try
            Dim row As DataRow() = dtSupplier.Select("id_supplier = " & sid)

            TextBox2.Text = row(0)(2) & vbCrLf & row(0)(3)   'vbCrLf = enter
        Catch ex As Exception
            Console.Write(ex.ToString)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FBarangPembelian.ShowDialog()
    End Sub

    Sub hitungtotal()
        Dim subtotal, total, dp As Long
        For i As Integer = 0 To ListView1.Items.Count - 1
            subtotal += Val(ListView1.Items(i).SubItems(4).Text)

        Next
        dp = Val(TextBox1.Text.Trim)
        total = subtotal - dp

        TextBox3.Text = subtotal : TextBox4.Text = total
    End Sub

    Private Sub ListView1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListView1.MouseDoubleClick

    End Sub

    Private Sub ListView1_MouseDown(sender As Object, e As MouseEventArgs) Handles ListView1.MouseDown
        If ListView1.Items.Count = 0 Then Exit Sub


        If e.Button = Windows.Forms.MouseButtons.Right Then
            ContextMenuStrip1.Show(MousePosition.X, MousePosition.Y)
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        hitungtotal()
    End Sub

    Private Sub HapusToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HapusToolStripMenuItem.Click
        ListView1.Items.Remove(ListView1.SelectedItems(0))
        hitungtotal()

    End Sub

    Private Sub UbahToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UbahToolStripMenuItem.Click
        'taruh di fbarangpembelian


        With FBarangPembelian
            .ComboBox1.SelectedValue = ListView1.SelectedItems(0).Text
            .TextBox1.Text = ListView1.SelectedItems(0).SubItems(3).Text
            .TextBox2.Text = ListView1.SelectedItems(0).SubItems(2).Text
            .Label4.Text = ListView1.SelectedItems(0).SubItems(4).Text
            .Show()
        End With

        ListView1.Items.Remove(ListView1.SelectedItems(0))
        hitungtotal()

    End Sub

    Private Sub btn_hapus_Click(sender As Object, e As EventArgs) Handles btn_hapus.Click

    End Sub

    Private Async Sub btn_simpan_Click(sender As Object, e As EventArgs) Handles btn_simpan.Click

        If (ComboBox2.Text = Nothing Or
            TextBox1.TextLength = 0 Or
            ListView1.Items.Count = 0) Then

            MsgBox("Lengkapi data")
            Exit Sub
        End If
        
        If btn_simpan.Text = "Simpan" Then


            sql = "insert into Pembelian (tanggal, id_supplier, tipe ,dp ,term) values ( " & _
                "'" & DateTimePicker1.Value & "', " & ComboBox1.SelectedValue.ToString & ", " & _
                "'" & ComboBox2.Text & "', " & Val(TextBox1.Text) & ", " & _
                "'" & TextBox5.Text & "')"


            'simpan
            MProgress.showProgress(ProgressBar1)
            Dim myTask = Task.Factory.StartNew(Sub() MKoneksi.exec(sql))
            Task.WaitAll(myTask) 'wait

            'ambil ID
            Dim dt As DataTable = Await Task(Of DataTable).Factory.StartNew(
                Function() MKoneksi.getLastID("Pembelian", "id_beli"))
            Dim id_beli As String = dt.Rows(0).Item(0).ToString


            For i As Integer = 0 To ListView1.Items.Count - 1
                sql = "insert into detail_pembelian values (" & id_beli & ", " & _
                        "" & ListView1.Items(i).Text & ", " & _
                        "" & ListView1.Items(i).SubItems(2).Text & ", " & _
                        "" & ListView1.Items(i).SubItems(3).Text & " )"
                myTask = Task.Factory.StartNew(Sub() MKoneksi.exec(sql))
                Task.WaitAll(myTask)
            Next

            MProgress.hideProgress(ProgressBar1)

            MsgBox("Data Berhaisl Disimpan", , "Pesan")

        ElseIf btn_simpan.Text = "Perbarui" Then
        End If

        initform()
    End Sub

    Sub initform()
        ListView1.Items.Clear()
        ComboBox1.SelectedIndex = 0
        ComboBox2.SelectedIndex = 0
        TextBox1.Text = "0"
        TextBox3.Text = "0"
        TextBox4.Text = "0"
        TextBox5.Text = Nothing
        DateTimePicker1.Value = Date.Now

    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        FCariPembelian.ShowDialog()
    End Sub

    Async Function display(ByVal id As Integer) As Task
        sql = "select * from pembelian where id_beli = " & id
        Dim dtPemb As DataTable = Await Task(Of DataTable).Factory.StartNew(
                Function() MKoneksi.getList(sql))

        sql = "select * from detail_pembelian where id_beli = " & id
        Dim dtDPemb As DataTable = Await Task(Of DataTable).Factory.StartNew(
                Function() MKoneksi.getList(sql))

        'cek apakah datanya ada?
        If (dtPemb.Rows.Count = 0) Then Exit Function
        If (dtDPemb.Rows.Count = 0) Then Exit Function

        'load header
        TEMP_ID = dtPemb.Rows(0).Item(0).ToString
        ComboBox1.SelectedValue = dtPemb.Rows(0).Item(0).ToString 'id supplier
        DateTimePicker1.Value = dtPemb.Rows(0).Item(0).ToString 'tanggal
        ComboBox2.SelectedText = dtPemb.Rows(0).Item(0).ToString 'jenis bayar
        TextBox5.Text = dtPemb.Rows(0).Item(0).ToString 'term
        TextBox1.Text = dtPemb.Rows(0).Item(0).ToString 'dp

        'load detail pembelian
        ListView1.Items.Clear()
        For Each dr As DataRow In dtDPemb.Rows
            lst = ListView1.Items.Add(dr(0))
            lst.SubItems.Add(dr(1))
            lst.SubItems.Add(dr(2))
            lst.SubItems.Add(dr(3))
            lst.SubItems.Add(dr(4))
            lst.SubItems.Add(dr(5))
        Next

    End Function

End Class