Public Class frmPowerUpTimed
    'MRL 2019-08-07
    Public cycles As Integer = 15
    Public cyclecount As Integer = 0
    Public cyclesleft As Integer = 0
    Public jobPath As String = vbNullString
    Public priority As Integer = 0

    'This program displays a splash screen with a countdown timer
    'After the specified timer duration, an executable will be started.
    'The purpose of the program is to allow background applications to be up and running prior to 
    'attempting to establishing connection with the UI
    '
    'The Power UP Run It.bat file contains the sequence to run the executable.
    'An example entry in Power UP Run It.bat
    ''START /LOW C:\780\PowerUpTimer.exe 45 C:\780\SPS780.exe 1
    '/LOW runs the PowerUpTimer.exe in Low Priority
    'The next entry is "C:\780\PowerUpTimer.exe" is the path to the PowerUpTimer.exe
    'Following the path is the timer duration in the above example it is 45 seconds - if the entry is less than 5 the timer is set to 5 seconds if
    'the duration is greater than 120 the timer is set to 120 seconds.
    'Following the duration is the path to executable to run, here the path is "C:\780\SPS780.exe"
    'Following duration is the priority in the above example the program will be run will RealTime priority
    ' 0 = High (default)
    ' 1 = Realtime 
    ' 2 = Above Normal
    ' anything else is regarded as Normal priority

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        Dim args() As String = Environment.GetCommandLineArgs()

        'args(0) is the path to the PowerUpTimer.exe

        If args.Length > 1 Then
            cycles = Convert.ToInt32(args(1))

            If cycles < 5 Then cycles = 5
            If cycles > 120 Then cycles = 120

        Else
            cycles = 15

        End If

        If args.Length > 2 Then jobPath = args(2)

        If args.Length > 3 Then priority = args(3)


        ' Add any initialization after the InitializeComponent() call.
        Label1.Text = "STARTING IN " & cycles.ToString & " SECONDS"
        Timer1.Start()

    End Sub
    Public Sub New(ByVal duration As Integer)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'cycles = duration
        'Label1.Text = "STARTING IN " & cycles.ToString & " SECONDS"
        'Timer1.Start()

    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        cyclecount += 1
        cyclesleft = cycles - cyclecount
        Dim m As Integer = cyclesleft Mod 2
        If m > 0 Then
            Label1.ForeColor = Color.Black
        Else
            Label1.ForeColor = Color.Red
        End If
        Label1.Text = "STARTING IN " & cyclesleft.ToString & " SECONDS"
        If cyclesleft > 0 Then
            Timer1.Start()
        Else
            Try
                If jobPath <> vbNullString Then
                    Dim job As New Process
                    job.StartInfo.FileName = jobPath
                    job.Start()
                    Select Case priority
                        Case 0
                            job.PriorityClass = ProcessPriorityClass.High
                        Case 1
                            job.PriorityClass = ProcessPriorityClass.RealTime
                        Case 2
                            job.PriorityClass = ProcessPriorityClass.AboveNormal
                        Case Else
                            job.PriorityClass = ProcessPriorityClass.Normal
                    End Select


                End If
            Catch ex As Exception
                MessageBox.Show("No jobpath entered. " & jobPath & " >> " & vbCrLf & ex.Message)
            End Try


            Me.Close()

        End If
    End Sub
End Class
