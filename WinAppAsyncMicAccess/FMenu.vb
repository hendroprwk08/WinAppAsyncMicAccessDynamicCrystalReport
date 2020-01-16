Public Class FMenu

    Private Sub PembelianToolStripMenuItem1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub KeluarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles KeluarToolStripMenuItem.Click
        End
    End Sub

    Private Sub SupplierToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SupplierToolStripMenuItem.Click
        FSupplier.Show()
        FSupplier.MdiParent = Me
    End Sub

    Private Sub PenggunaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PenggunaToolStripMenuItem.Click
        FPengguna.Show()
        FPengguna.MdiParent = Me
    End Sub

    Private Sub MinimizeAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MinimizeAllToolStripMenuItem.Click
        For Each form As Form In Me.MdiChildren
            form.WindowState = FormWindowState.Minimized
        Next
    End Sub

    Private Sub CloseAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseAllToolStripMenuItem.Click
        For Each form As Form In Me.MdiChildren
            form.Close()
        Next
    End Sub

    Private Sub NormalAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NormalAllToolStripMenuItem.Click
        For Each form As Form In Me.MdiChildren
            form.WindowState = FormWindowState.Normal
        Next
    End Sub

    Private Sub PembelianToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PembelianToolStripMenuItem.Click
        FPembelian.Show()
        FPembelian.MdiParent = Me
    End Sub

    Private Sub LaporanToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LaporanToolStripMenuItem.Click
        FCRPrint.Show()
        FCRPrint.MdiParent = Me
    End Sub
End Class