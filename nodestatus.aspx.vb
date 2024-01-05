Public Class nodestatus
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Not IsPostBack) Then
            ShowGrid()
        End If
    End Sub

    Private Sub gridCategory_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridNodeStatus.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            e.Row.Cells(0).Text = HttpUtility.HtmlDecode(e.Row.Cells(0).Text)
            e.Row.Cells(4).Text = HttpUtility.HtmlDecode(e.Row.Cells(4).Text)
        End If
    End Sub

    Private Sub ShowGrid()
        Dim con As SqlClient.SqlConnection = New SqlClient.SqlConnection(modMain.ConnectionString)
        Try
            con.Open()
            Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand("CMN.USP_SelectNodeStatus", con)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0

            Dim adap As SqlClient.SqlDataAdapter = New SqlClient.SqlDataAdapter(cmd)
            Dim dtbl As DataTable = New DataTable
            dtbl.Clear()
            adap.Fill(dtbl)
            gridNodeStatus.DataSource = dtbl
            gridNodeStatus.DataBind()
            adap.Dispose()
            cmd.Dispose()
        Catch ex As Exception
            lbl.Text = ex.Message

        Finally
            con.Close()
            con.Dispose()
        End Try
    End Sub

    Protected Sub tmrAsOf_Tick(sender As Object, e As EventArgs) Handles tmrAsOf.Tick
        ShowGrid()
    End Sub
End Class