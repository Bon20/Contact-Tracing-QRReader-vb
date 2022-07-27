Imports System.IO
Imports AForge
Imports AForge.Video
Imports AForge.Video.DirectShow
Imports ZXing
Public Class Form1
    Dim MyCam As VideoCaptureDevice
    Dim infobit As Bitmap
    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Using sw As StreamWriter = New StreamWriter(Application.StartupPath & "" + txtbxFname.Text & " " + txtbxLname.Text & ".txt")
            MessageBox.Show(" Record Submitted!")
            Dim datenow = DateTime.Now

            sw.WriteLine(datenow.ToLongDateString())
            sw.WriteLine(datenow.ToShortTimeString())

            sw.WriteLine(vbCrLf & lblFname.Text & " " + txtbxFname.Text)
            sw.WriteLine(lblLname.Text & " " + txtbxLname.Text)
            sw.WriteLine(lblCnumber.Text & " " + txtbxCnumber.Text)
            sw.WriteLine(lblAge.Text & " " + txtbxAge.Text)
            sw.WriteLine(lblGender.Text & " " + cbxGender.Text)
            sw.WriteLine(lblAddress.Text & " " + txtbxAddress.Text)
            sw.WriteLine(vbCrLf & lblQuestion2.Text)
            sw.WriteLine(lblFever.Text & " " + cboxFever.Text)
            sw.WriteLine(lblCough.Text & " " + cboxCough.Text)
            sw.WriteLine(lblColds.Text & " " + cboxColds.Text)
            sw.WriteLine(lblSorethroat.Text & " " + cboxSorethroat.Text)
            sw.WriteLine(lblDiffInBreath.Text & " " + cboxDiffInBreath.Text)
            sw.WriteLine(lbldiarrhea.Text & " " + cBoxDiarrhea.Text)

            If radbuttonYes.Checked = True Then
                sw.WriteLine(vbCrLf & gbxQuestion1.Text + radbuttonYes.Text)
            Else
                sw.WriteLine(vbCrLf & gbxQuestion1.Text & " " + radbuttonNo.Text)
            End If

            sw.Close()

            txtbxFname.Text = ""
            txtbxLname.Text = ""
            txtbxCnumber.Text = ""
            txtbxAge.Text = ""
            txtbxAddress.Text = ""

            cbxGender.SelectedIndex = -1
            cboxFever.SelectedIndex = -1
            cboxCough.SelectedIndex = -1
            cboxColds.SelectedIndex = -1
            cboxSorethroat.SelectedIndex = -1
            cboxDiffInBreath.SelectedIndex = -1
            cBoxDiarrhea.SelectedIndex = -1

            radbuttonYes.Checked = False
            radbuttonNo.Checked = False

        End Using
    End Sub

    Private Sub txtbxCnumber_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxCnumber.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
            MessageBox.Show("This field is accepting numbers only!")
        ElseIf txtbxCnumber.Text.Length > 11 Then
            If e.KeyChar <> ControlChars.Back Then
                e.Handled = True
                MessageBox.Show("Contact number should not be more than 11 numbers!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

    Private Sub txtbxAge_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxAge.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
            MessageBox.Show("This field is accepting numbers only!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf txtbxAge.Text.Length > 3 Then
            If e.KeyChar <> ControlChars.Back Then
                e.Handled = True
                MessageBox.Show("Age should not be more than 3 numbers!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

    Private Sub txtbxFname_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxFname.KeyPress
        If Not Char.IsLetter(e.KeyChar) And Not e.KeyChar = Chr(Keys.Delete) And Not e.KeyChar = Chr(Keys.Back) And Not e.KeyChar = Chr(Keys.Space) Then
            e.Handled = True
            MessageBox.Show("This field is accepting letters only!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub txtbxLname_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxLname.KeyPress
        If Not Char.IsLetter(e.KeyChar) And Not e.KeyChar = Chr(Keys.Delete) And Not e.KeyChar = Chr(Keys.Back) And Not e.KeyChar = Chr(Keys.Space) Then
            e.Handled = True
            MessageBox.Show("This field is accepting letters only!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

    Private Sub btnViewDisplay_Click(sender As Object, e As EventArgs) Handles btnViewDisplay.Click
        Dim ViewDisplay As OpenFileDialog = New OpenFileDialog()

        If ViewDisplay.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            If (ViewDisplay.OpenFile()) IsNot Nothing Then
                Dim strfilename As String = ViewDisplay.FileName
                Dim filetext As String = File.ReadAllText(strfilename)
                RboxDisplay.Text = filetext
            End If

        End If
    End Sub

    Private Sub btnClearDisplay_Click(sender As Object, e As EventArgs) Handles btnClearDisplay.Click
        RboxDisplay.Clear()
    End Sub

    Private Sub btnScan_Click(sender As Object, e As EventArgs) Handles btnScan.Click
        Dim ScanQr As VideoCaptureDeviceForm = New VideoCaptureDeviceForm
        If ScanQr.ShowDialog() = Windows.Forms.DialogResult.OK Then
            MyCam = ScanQr.VideoDevice
            AddHandler MyCam.NewFrame, New NewFrameEventHandler(AddressOf OutputPic)
            MyCam.Start()
            DetectQR()
        End If
    End Sub
    Private Sub OutputPic(sender As Object, eventArgs As NewFrameEventArgs)
        infobit = CType(eventArgs.Frame.Clone(), Bitmap)
        pboxQrDisplay.Image = CType(eventArgs.Frame.Clone(), Bitmap)
    End Sub

    Private Sub btnDetect_Click(sender As Object, e As EventArgs) Handles btnDetect.Click
        DetectQR()
    End Sub
    Public Sub DetectQR()
        If pboxQrDisplay.Image IsNot Nothing Then
            Dim QrImage As BarcodeReader = New BarcodeReader()
            Dim QROutput As Result = QrImage.Decode(CType(pboxQrDisplay.Image, Bitmap))
            If QROutput IsNot Nothing Then
                MsgBox(" QR Detected")
                Dim QRdetails As String = QROutput.ToString()
                Dim QrInfo As String() = QRdetails.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                txtbxFname.Text = QrInfo(0)
                txtbxLname.Text = QrInfo(1)
                txtbxCnumber.Text = QrInfo(2)
                txtbxAge.Text = QrInfo(3)
                txtbxAddress.Text = QrInfo(5)

                If QrInfo(4) = "NO" Then
                    cbxGender.SelectedIndex = "1"
                Else
                    cbxGender.SelectedIndex = "0"
                End If

                If QrInfo(6) = "NO" Then
                    cboxFever.SelectedIndex = "0"
                Else
                    cboxFever.SelectedIndex = "1"
                End If

                If QrInfo(8) = "NO" Then
                    cboxCough.SelectedIndex = "0"
                Else
                    cboxCough.SelectedIndex = "1"
                End If

                If QrInfo(9) = "NO" Then
                    cboxColds.SelectedIndex = "0"
                Else
                    cboxColds.SelectedIndex = "1"
                End If

                If QrInfo(10) = "NO" Then
                    cboxSorethroat.SelectedIndex = "0"
                Else
                    cboxSorethroat.SelectedIndex = "1"
                End If

                If QrInfo(11) = "NO" Then
                    cboxDiffInBreath.SelectedIndex = "0"
                Else
                    cboxDiffInBreath.SelectedIndex = "1"
                End If

                If QrInfo(12) = "NO" Then
                    cBoxDiarrhea.SelectedIndex = "0"
                Else
                    cBoxDiarrhea.SelectedIndex = "1"
                End If

                If QrInfo(7) = "NO" Then
                    radbuttonNo.Checked = True
                Else
                    radbuttonYes.Checked = True
                End If
            End If
        End If
    End Sub
End Class
