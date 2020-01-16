Imports CrystalDecisions.Shared

Public Class FCRPrint

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbx_pilih.SelectedIndexChanged
        Dim sql As String
        Dim dt, dtDari, dtSampai As DataTable

        cbx_dari.DataSource = Nothing : cbx_dari.Items.Clear()
        cbx_sampai.DataSource = Nothing : cbx_sampai.Items.Clear()

        If cbx_pilih.SelectedItem = "Barang" Then
            sql = "select id_barang, nama from barang order by id_barang asc"

            dtDari = New DataTable
            dtSampai = New DataTable

            dtDari = MKoneksi.getList(sql)
            dtSampai = MKoneksi.getList(sql)

            With cbx_dari
                .DataSource = dtDari
                .ValueMember = "id_barang"
                .DisplayMember = "nama"
            End With

            With cbx_sampai
                .DataSource = dtSampai
                .ValueMember = "id_barang"
                .DisplayMember = "nama"
            End With
        ElseIf cbx_pilih.SelectedItem = "Supplier" Then
            sql = "select id_supplier, nama from supplier order by id_supplier asc"

            dtDari = New DataTable
            dtSampai = New DataTable

            dtDari = MKoneksi.getList(sql)
            dtSampai = MKoneksi.getList(sql)

            With cbx_dari
                .DataSource = dtDari
                .ValueMember = "id_supplier"
                .DisplayMember = "nama"
            End With

            With cbx_sampai
                .DataSource = dtSampai
                .ValueMember = "id_supplier"
                .DisplayMember = "nama"
            End With
        ElseIf cbx_pilih.SelectedItem = "Pengguna" Then
            sql = "select email, tipe from pengguna order by email asc"

            dtDari = New DataTable
            dtSampai = New DataTable

            dtDari = MKoneksi.getList(sql)
            dtSampai = MKoneksi.getList(sql)

            With cbx_dari
                .DataSource = dtDari
                .ValueMember = "email"
                .DisplayMember = "email"
            End With

            With cbx_sampai
                .DataSource = dtSampai
                .ValueMember = "email"
                .DisplayMember = "email"
            End With
        Else
            sql = "select id_beli from pembelian order by id_beli asc"

            dtDari = New DataTable
            dtSampai = New DataTable

            dtDari = MKoneksi.getList(sql)
            dtSampai = MKoneksi.getList(sql)

            With cbx_dari
                .DataSource = dtDari
                .ValueMember = "id_beli"
                .DisplayMember = "id_beli"
            End With

            With cbx_sampai
                .DataSource = dtSampai
                .ValueMember = "id_beli"
                .DisplayMember = "id_beli"
            End With
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim dari, sampai As String
        Dim lokasiDatabase As String = Application.StartupPath() & "\DB.mdb"
        Dim Eng As CrystalDecisions.CrystalReports.Engine.Table
        Dim DbInfo As TableLogOnInfo

        dari = cbx_dari.SelectedValue.ToString
        sampai = cbx_sampai.SelectedValue.ToString

        If cbx_pilih.SelectedItem = "Barang" Then
            Dim report As New CRBarang
          
            report.RecordSelectionFormula = "{barang.id_barang} >= " &
                cbx_dari.SelectedValue.ToString & " and {barang.id_barang} <= " & cbx_sampai.SelectedValue.ToString

            'Set database Dinamis u/ Crystal report
            DbInfo = New TableLogOnInfo
            For Each Eng In report.Database.Tables
                With DbInfo.ConnectionInfo
                    .ServerName = lokasiDatabase
                    .UserID = "Admin"
                    .Password = ""
                    .DatabaseName = ""
                End With
                Eng.ApplyLogOnInfo(DbInfo)
            Next

            crv.ReportSource = report
            crv.RefreshReport()

        ElseIf cbx_pilih.SelectedItem = "Supplier" Then
            Dim report1 As New CRSupplier

            report1.RecordSelectionFormula = "{supplier.id_supplier} >= " &
                cbx_dari.SelectedValue.ToString & " and {supplier.id_supplier} <= " & cbx_sampai.SelectedValue.ToString

            'Set database Dinamis u/ Crystal report
            DbInfo = New TableLogOnInfo
            For Each Eng In report1.Database.Tables
                With DbInfo.ConnectionInfo
                    .ServerName = lokasiDatabase
                    .UserID = "Admin"
                    .Password = ""
                    .DatabaseName = ""
                End With
                Eng.ApplyLogOnInfo(DbInfo)
            Next

            crv.ReportSource = report1
            crv.RefreshReport()

        ElseIf cbx_pilih.SelectedItem = "Pengguna" Then
            Dim report2 As New CRPengguna

            report2.RecordSelectionFormula = "{pengguna.email} >= '" &
                cbx_dari.SelectedValue.ToString & "' and {pengguna.email} <= '" & cbx_sampai.SelectedValue.ToString & "'"

            'Set database Dinamis u/ Crystal report
            DbInfo = New TableLogOnInfo
            For Each Eng In report2.Database.Tables
                With DbInfo.ConnectionInfo
                    .ServerName = lokasiDatabase
                    .UserID = "Admin"
                    .Password = ""
                    .DatabaseName = ""
                End With
                Eng.ApplyLogOnInfo(DbInfo)
            Next

            crv.ReportSource = report2
            crv.RefreshReport()
        Else
            Dim report3 As New CRPembelianrpt

            report3.RecordSelectionFormula = "{pembelian.id_beli} >= " &
                cbx_dari.SelectedValue.ToString & " and {pembelian.id_beli} <= " & cbx_sampai.SelectedValue.ToString

            'Set database Dinamis u/ Crystal report
            DbInfo = New TableLogOnInfo
            For Each Eng In report3.Database.Tables
                With DbInfo.ConnectionInfo
                    .ServerName = lokasiDatabase
                    .UserID = "Admin"
                    .Password = ""
                    .DatabaseName = ""
                End With
                Eng.ApplyLogOnInfo(DbInfo)
            Next

            crv.ReportSource = report3
            crv.RefreshReport()
        End If
    End Sub

End Class