Public Class monitorednodes
    Inherits System.Web.UI.Page

    Private Sub monitorednodes_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        ClientScript.GetPostBackEventReference(Me, String.Empty)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Not IsPostBack) Then
            cblUseFqdnInsteadOfIP.Items.Add("")
            cblUseFqdnInsteadOfIP.Items.Add(True)
            cblUseFqdnInsteadOfIP.Items.Add(False)
            cblUseFqdnInsteadOfIP.SelectedIndex = 0

            cblIsTlsEncryted.Items.Add("")
            cblIsTlsEncryted.Items.Add(True)
            cblIsTlsEncryted.Items.Add(False)
            cblIsTlsEncryted.SelectedIndex = 0

            cblMonitoringLevel.Items.Add(New ListItem("", ""))
            cblMonitoringLevel.Items.Add(New ListItem("ping", 1))
            cblMonitoringLevel.Items.Add(New ListItem("telnet >> ping", 2))
            cblMonitoringLevel.Items.Add(New ListItem("http >> telnet >> ping", 3))
            cblMonitoringLevel.SelectedIndex = 0
            ShowGrid()
            ShowDropDown()
            For i As Int16 = 1 To gridMonitoredNodes.Columns.Count - 1
                cblOrder01.Items.Add(gridMonitoredNodes.Columns(i).HeaderText)
                cblOrder02.Items.Add(gridMonitoredNodes.Columns(i).HeaderText)
                cblOrder03.Items.Add(gridMonitoredNodes.Columns(i).HeaderText)
                cblOrder04.Items.Add(gridMonitoredNodes.Columns(i).HeaderText)
                cblOrder05.Items.Add(gridMonitoredNodes.Columns(i).HeaderText)
                cblOrder06.Items.Add(gridMonitoredNodes.Columns(i).HeaderText)
                cblOrder07.Items.Add(gridMonitoredNodes.Columns(i).HeaderText)
                cblOrder08.Items.Add(gridMonitoredNodes.Columns(i).HeaderText)
                cblOrder09.Items.Add(gridMonitoredNodes.Columns(i).HeaderText)
                cblOrder10.Items.Add(gridMonitoredNodes.Columns(i).HeaderText)
                cblOrder11.Items.Add(gridMonitoredNodes.Columns(i).HeaderText)
            Next
            cblOrder01.SelectedIndex = 0
            cblOrder02.SelectedIndex = 0
            cblOrder03.SelectedIndex = 0
            cblOrder04.SelectedIndex = 0
            cblOrder05.SelectedIndex = 0
            cblOrder06.SelectedIndex = 0
            cblOrder07.SelectedIndex = 0
            cblOrder08.SelectedIndex = 0
            cblOrder09.SelectedIndex = 0
            cblOrder10.SelectedIndex = 0
            cblOrder11.SelectedIndex = 0
        End If
    End Sub

    Private Sub ShowDropDown()
        Dim con As SqlClient.SqlConnection = New SqlClient.SqlConnection(modMain.ConnectionString)
        Try
            con.Open()
            Dim cmd As SqlClient.SqlCommand
            Dim rdr As SqlClient.SqlDataReader

            cmd = New SqlClient.SqlCommand("CMN.USP_SelectSecurityTools", con)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            rdr = cmd.ExecuteReader
            If ((rdr.Read)) Then
                dlstSecurityTool.InnerHtml = vbCrLf
                Do
                    dlstSecurityTool.InnerHtml &= "<option value=""" & rdr.Item("SecurityTool") & """/>" & vbCrLf
                Loop While (rdr.Read)
            End If
            rdr.Close()
            cmd.Dispose()

            cmd = New SqlClient.SqlCommand("CMN.USP_SelectInstances", con)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            rdr = cmd.ExecuteReader
            If ((rdr.Read)) Then
                dlstInstance.InnerHtml = vbCrLf
                Do
                    dlstInstance.InnerHtml &= "<option value=""" & rdr.Item("Instance") & """/>" & vbCrLf
                Loop While (rdr.Read)
            End If
            rdr.Close()
            cmd.Dispose()

            cmd = New SqlClient.SqlCommand("CMN.USP_SelectDomains", con)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0
            rdr = cmd.ExecuteReader
            If ((rdr.Read)) Then
                dlstDomain.InnerHtml = vbCrLf
                Do
                    dlstDomain.InnerHtml &= "<option value=""" & rdr.Item("Domain") & """/>" & vbCrLf
                Loop While (rdr.Read)
            End If
            rdr.Close()
            cmd.Dispose()
        Catch ex As Exception

        Finally
            con.Close()
            con.Dispose()
        End Try
    End Sub

    Private Sub ShowGrid()
        Dim con As SqlClient.SqlConnection = New SqlClient.SqlConnection(modMain.ConnectionString)
        Try
            con.Open()
            Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand("CMN.USP_CrudSelectMonitoredNode", con)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 0

            cmd.Parameters.Add("@TotalRowCount", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output

            cmd.Parameters.Add("@PageNo", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.InputOutput
            cmd.Parameters.Item("@PageNo").Value = Val(txtPage.Text)

            cmd.Parameters.Add("@RowCountPerPage", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Input
            cmd.Parameters.Item("@RowCountPerPage").Value = Val(txtRowCountPerPage.Text)

            If (hdnWhereSecurityTool.Value.Trim <> "") Then
                cmd.Parameters.Add("@SecurityTool", System.Data.SqlDbType.NVarChar, 300).Direction = System.Data.ParameterDirection.Input
                cmd.Parameters.Item("@SecurityTool").Value = hdnWhereSecurityTool.Value
            End If

            If (hdnWhereInstance.Value.Trim <> "") Then
                cmd.Parameters.Add("@Instance", System.Data.SqlDbType.NVarChar, 300).Direction = System.Data.ParameterDirection.Input
                cmd.Parameters.Item("@Instance").Value = hdnWhereInstance.Value
            End If

            If (hdnWhereDomain.Value.Trim <> "") Then
                cmd.Parameters.Add("@Domain", System.Data.SqlDbType.NVarChar, 300).Direction = System.Data.ParameterDirection.Input
                cmd.Parameters.Item("@Domain").Value = hdnWhereDomain.Value
            End If

            If (hdnWhereIP.Value.Trim <> "") Then
                cmd.Parameters.Add("@IP", System.Data.SqlDbType.NVarChar, 50).Direction = System.Data.ParameterDirection.Input
                cmd.Parameters.Item("@IP").Value = hdnWhereIP.Value
            End If

            If (hdnWherehostname.Value.Trim <> "") Then
                cmd.Parameters.Add("@hostname", System.Data.SqlDbType.NVarChar, 50).Direction = System.Data.ParameterDirection.Input
                cmd.Parameters.Item("@hostname").Value = hdnWherehostname.Value
            End If

            If (hdnWherefqdn.Value.Trim <> "") Then
                cmd.Parameters.Add("@fqdn", System.Data.SqlDbType.NVarChar, 100).Direction = System.Data.ParameterDirection.Input
                cmd.Parameters.Item("@fqdn").Value = hdnWherefqdn.Value
            End If

            If (hdnWherePort.Value.Trim <> "") Then
                cmd.Parameters.Add("@Port", System.Data.SqlDbType.NVarChar, 10).Direction = System.Data.ParameterDirection.Input
                cmd.Parameters.Item("@Port").Value = hdnWherePort.Value
            End If

            If (hdnWherePath.Value.Trim <> "") Then
                cmd.Parameters.Add("@Path", System.Data.SqlDbType.NVarChar, 400).Direction = System.Data.ParameterDirection.Input
                cmd.Parameters.Item("@Path").Value = hdnWherePath.Value
            End If

            If (hdnWhereUseFqdnInsteadOfIP.Value.Trim <> "") Then
                cmd.Parameters.Add("@UseFqdnInsteadOfIP", System.Data.SqlDbType.Bit).Direction = System.Data.ParameterDirection.Input
                cmd.Parameters.Item("@UseFqdnInsteadOfIP").Value = hdnWhereUseFqdnInsteadOfIP.Value
            End If

            If (hdnWhereIsTlsEncryted.Value.Trim <> "") Then
                cmd.Parameters.Add("@IsTlsEncryted", System.Data.SqlDbType.Bit).Direction = System.Data.ParameterDirection.Input
                cmd.Parameters.Item("@IsTlsEncryted").Value = hdnWhereIsTlsEncryted.Value
            End If

            If (hdnWhereMonitoringLevel.Value.Trim <> "") Then
                cmd.Parameters.Add("@MonitoringLevel", System.Data.SqlDbType.TinyInt).Direction = System.Data.ParameterDirection.Input
                cmd.Parameters.Item("@MonitoringLevel").Value = hdnWhereMonitoringLevel.Value
            End If

            If (hdnSort.Value.Trim <> "") Then
                cmd.Parameters.Add("@Sort", System.Data.SqlDbType.NVarChar, 300).Direction = System.Data.ParameterDirection.Input
                cmd.Parameters.Item("@Sort").Value = hdnSort.Value
            End If

            Dim adap As SqlClient.SqlDataAdapter = New SqlClient.SqlDataAdapter(cmd)
            Dim dtbl As DataTable = New DataTable
            dtbl.Clear()
            adap.Fill(dtbl)
            gridMonitoredNodes.DataSource = dtbl
            gridMonitoredNodes.DataBind()
            lblTotalRowCount.Text = cmd.Parameters.Item("@TotalRowCount").Value
            txtPage.Text = cmd.Parameters.Item("@PageNo").Value
            Dim TotalPage As Integer = Math.Ceiling(Val(lblTotalRowCount.Text) / Val(txtRowCountPerPage.Text))
            lblTotal.Text = "of " & TotalPage
            Dim LastRow As Integer = Val(txtPage.Text) * Val(txtRowCountPerPage.Text)
            If (TotalPage = Val(txtPage.Text)) Then LastRow = Val(lblTotalRowCount.Text)
            lblDisplay.Text = "Displaying Approved Files " & Val(txtPage.Text) * Val(txtRowCountPerPage.Text) - Val(txtRowCountPerPage.Text) + 1 & " - " & LastRow & " of "
            adap.Dispose()
            cmd.Dispose()
        Catch ex As Exception

        Finally
            con.Close()
            con.Dispose()
        End Try
    End Sub

    Private Sub gridMonitoredNodes_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridMonitoredNodes.RowDataBound
        e.Row.Cells(0).Visible = False
    End Sub

    Private Sub gridMonitoredNodes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridMonitoredNodes.RowCommand
        If (e.CommandName = "updateMonitoredNode") Then
            hdnSelectMonitoredNodeId.Value = e.CommandArgument
            Dim con As SqlClient.SqlConnection = New SqlClient.SqlConnection(modMain.ConnectionString)
            Try
                con.Open()
                Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand("CMN.USP_CrudSelectMonitoredNodeById", con)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandTimeout = 0

                cmd.Parameters.Add("@MonitoredNodeId", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Input
                cmd.Parameters.Item("@MonitoredNodeId").Value = hdnSelectMonitoredNodeId.Value

                Dim rdr As SqlClient.SqlDataReader = cmd.ExecuteReader
                If (rdr.Read) Then
                    hdnSelectSecurityTool.Value = rdr.Item("SecurityTool")
                    hdnSelectInstance.Value = rdr.Item("Instance")
                    hdnSelectDomain.Value = rdr.Item("Domain")
                    hdnSelectIP.Value = rdr.Item("IP")
                    hdnSelecthostname.Value = modMain.DBNulltoNothing(rdr.Item("hostname"))
                    hdnSelectfqdn.Value = modMain.DBNulltoNothing(rdr.Item("fqdn"))
                    hdnSelectPort.Value = modMain.DBNulltoNothing(rdr.Item("Port"))
                    hdnSelectPath.Value = modMain.DBNulltoNothing(rdr.Item("Path"))
                    hdnSelectUseFqdnInsteadOfIP.Value = modMain.DBNulltoNothing(rdr.Item("UseFqdnInsteadOfIP"))
                    hdnSelectIsTlsEncryted.Value = modMain.DBNulltoNothing(rdr.Item("IsTlsEncryted"))
                    hdnSelectMonitoringLevel.Value = rdr.Item("MonitoringLevel")
                End If
                rdr.Close()
                cmd.Dispose()
            Catch ex As Exception

            Finally
                con.Close()
                con.Dispose()
            End Try
            ClientScript.RegisterStartupScript(Me.[GetType](), "update", "javascript:inputShow('Update Records');", True)
        End If
    End Sub

    Protected Sub btnFrst_Click(sender As Object, e As EventArgs) Handles btnFrst.Click
        txtPage.Text = 1
        ShowGrid()
    End Sub

    Protected Sub btnPrev_Click(sender As Object, e As EventArgs) Handles btnPrev.Click
        If (Val(txtPage.Text) > 1) Then txtPage.Text = Val(txtPage.Text) - 1
        ShowGrid()
    End Sub

    Protected Sub txtPage_TextChanged(sender As Object, e As EventArgs) Handles txtPage.TextChanged
        ValidatePage()
        ShowGrid()
    End Sub

    Protected Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Dim lastPage As Integer = Math.Ceiling(Val(lblTotalRowCount.Text) / Val(txtRowCountPerPage.Text))
        If (Val(txtPage.Text) < lastPage) Then txtPage.Text = Val(txtPage.Text) + 1
        ShowGrid()
    End Sub

    Protected Sub btnLast_Click(sender As Object, e As EventArgs) Handles btnLast.Click
        Dim lastPage As Integer = Math.Ceiling(Val(lblTotalRowCount.Text) / Val(txtRowCountPerPage.Text))
        If (lastPage = 0) Then
            txtPage.Text = 1
        Else
            txtPage.Text = lastPage
        End If
        ShowGrid()
    End Sub

    Protected Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        ValidatePage()
        ValidateRowCountPerPage()
        ShowGrid()
    End Sub

    Protected Sub txtRowCountPerPage_TextChanged(sender As Object, e As EventArgs) Handles txtRowCountPerPage.TextChanged
        ValidateRowCountPerPage()
        ShowGrid()
    End Sub

    Private Sub ValidatePage()
        Dim lastPage As Integer = Math.Ceiling(Val(lblTotalRowCount.Text) / Val(txtRowCountPerPage.Text))
        If (Val(txtPage.Text) < 1) Then
            txtPage.Text = 1
        ElseIf (Val(txtPage.Text) > lastPage) Then
            If (lastPage = 0) Then
                txtPage.Text = 1
            Else
                txtPage.Text = lastPage
            End If
        End If
    End Sub

    Private Sub ValidateRowCountPerPage()
        If (Val(txtRowCountPerPage.Text) < 10) Then
            txtRowCountPerPage.Text = 10
        ElseIf ((Val(lblTotalRowCount.Text) > 0) AndAlso (Val(txtRowCountPerPage.Text) > Val(lblTotalRowCount.Text))) Then
            txtRowCountPerPage.Text = Val(lblTotalRowCount.Text)
        End If
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim msgResult As String = Request.Form("msgResult")
        If (msgResult.ToLower = "yes") Then
            Dim MonitoredNodeIds As String = ""
            For Each row As GridViewRow In gridMonitoredNodes.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim chkRow As CheckBox = TryCast(row.Cells(1).FindControl("cbSelect"), CheckBox)
                    If chkRow.Checked Then
                        MonitoredNodeIds &= IIf(MonitoredNodeIds = "", "", ",") & row.Cells(0).Text
                    End If
                End If
            Next
            If (MonitoredNodeIds <> "") Then
                Dim con As SqlClient.SqlConnection = New SqlClient.SqlConnection(modMain.ConnectionString)
                Try
                    con.Open()
                    Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand("CMN.USP_CrudDeleteMonitoredNode", con)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandTimeout = 0

                    cmd.Parameters.Add("@MonitoredNodeIds", System.Data.SqlDbType.NVarChar, 1000).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@MonitoredNodeIds").Value = MonitoredNodeIds

                    cmd.Parameters.Add("@UserAccount", System.Data.SqlDbType.NVarChar, 50).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@UserAccount").Value = DBNull.Value

                    cmd.ExecuteNonQuery()
                    cmd.Dispose()
                Catch ex As Exception

                Finally
                    con.Close()
                    con.Dispose()
                End Try
                ShowGrid()
            End If
        End If
    End Sub

    Protected Sub btnSubmitInput_Click(sender As Object, e As EventArgs) Handles btnSubmitInput.Click
        If (InStr(hdnTitle.Value, "Update", CompareMethod.Text) > 0) Then
            hdnSelectSecurityTool.Value = txtSecurityTool.Text.Trim
            hdnSelectInstance.Value = txtInstance.Text.Trim
            hdnSelectDomain.Value = txtDomain.Text.Trim
            hdnSelectIP.Value = txtIP.Text.Trim
            hdnSelecthostname.Value = txthostname.Text.Trim
            hdnSelectfqdn.Value = txtfqdn.Text.Trim
            hdnSelectPort.Value = txtPort.Text.Trim
            hdnSelectPath.Value = txtPath.Text.Trim
            hdnSelectUseFqdnInsteadOfIP.Value = cblUseFqdnInsteadOfIP.SelectedValue
            hdnSelectIsTlsEncryted.Value = cblIsTlsEncryted.SelectedValue
            hdnSelectMonitoringLevel.Value = cblMonitoringLevel.SelectedValue
            If (hdnSelectSecurityTool.Value.Trim = "") Then
                ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "javascript:alertMsg('" & sender.ID & "','Please input a SecurityTool!','Missing Input');", True)
                ClientScript.RegisterStartupScript(Me.[GetType](), "update", "javascript:inputShow('Update Records');", True)
            ElseIf (hdnSelectInstance.Value.Trim = "") Then
                ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "javascript:alertMsg('" & sender.ID & "','Please input an Instance!','Missing Input');", True)
                ClientScript.RegisterStartupScript(Me.[GetType](), "update", "javascript:inputShow('Update Records');", True)
            ElseIf (hdnSelectDomain.Value.Trim = "") Then
                ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "javascript:alertMsg('" & sender.ID & "','Please input a Domain!','Missing Input');", True)
                ClientScript.RegisterStartupScript(Me.[GetType](), "update", "javascript:inputShow('Update Records');", True)
            ElseIf (hdnSelectIP.Value.Trim = "") Then
                ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "javascript:alertMsg('" & sender.ID & "','Please input an IP!','Missing Input');", True)
                ClientScript.RegisterStartupScript(Me.[GetType](), "update", "javascript:inputShow('Update Records');", True)
            ElseIf (hdnSelectMonitoringLevel.Value.Trim = "") Then
                ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "javascript:alertMsg('" & sender.ID & "','Please input a MonitoringLevel!','Missing Input');", True)
                ClientScript.RegisterStartupScript(Me.[GetType](), "update", "javascript:inputShow('Update Records');", True)
            Else
                Dim con As SqlClient.SqlConnection = New SqlClient.SqlConnection(modMain.ConnectionString)
                Try
                    con.Open()
                    Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand("CMN.USP_CrudUpdateMonitoredNode", con)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandTimeout = 0

                    cmd.Parameters.Add("@MonitoredNodeId", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@MonitoredNodeId").Value = hdnSelectMonitoredNodeId.Value

                    cmd.Parameters.Add("@SecurityTool", System.Data.SqlDbType.NVarChar, 300).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@SecurityTool").Value = hdnSelectSecurityTool.Value

                    cmd.Parameters.Add("@Instance", System.Data.SqlDbType.NVarChar, 300).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@Instance").Value = hdnSelectInstance.Value

                    cmd.Parameters.Add("@Domain", System.Data.SqlDbType.NVarChar, 300).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@Domain").Value = hdnSelectDomain.Value

                    cmd.Parameters.Add("@IP", System.Data.SqlDbType.NVarChar, 50).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@IP").Value = hdnSelectIP.Value

                    cmd.Parameters.Add("@hostname", System.Data.SqlDbType.NVarChar, 50).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@hostname").Value = modMain.NothingtoDBNull(hdnSelecthostname.Value)

                    cmd.Parameters.Add("@fqdn", System.Data.SqlDbType.NVarChar, 100).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@fqdn").Value = modMain.NothingtoDBNull(hdnSelectfqdn.Value)

                    cmd.Parameters.Add("@Port", System.Data.SqlDbType.NVarChar, 10).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@Port").Value = modMain.NothingtoDBNull(hdnSelectPort.Value)

                    cmd.Parameters.Add("@Path", System.Data.SqlDbType.NVarChar, 400).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@Path").Value = modMain.NothingtoDBNull(modMain.ManageAddress(hdnSelectPath.Value, "/", modMain.PathPart.AtBegin))

                    cmd.Parameters.Add("@UseFqdnInsteadOfIP", System.Data.SqlDbType.Bit).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@UseFqdnInsteadOfIP").Value = modMain.NothingtoDBNull(hdnSelectUseFqdnInsteadOfIP.Value)

                    cmd.Parameters.Add("@IsTlsEncryted", System.Data.SqlDbType.Bit).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@IsTlsEncryted").Value = modMain.NothingtoDBNull(hdnSelectIsTlsEncryted.Value)

                    cmd.Parameters.Add("@MonitoringLevel", System.Data.SqlDbType.TinyInt).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@MonitoringLevel").Value = hdnSelectMonitoringLevel.Value

                    cmd.Parameters.Add("@UserAccount", System.Data.SqlDbType.NVarChar, 50).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@UserAccount").Value = DBNull.Value

                    cmd.ExecuteNonQuery()
                    cmd.Dispose()
                Catch ex As Exception

                Finally
                    con.Close()
                    con.Dispose()
                End Try
                hdnSelectSecurityTool.Value = ""
                hdnSelectInstance.Value = ""
                hdnSelectDomain.Value = ""
                hdnSelectIP.Value = ""
                hdnSelecthostname.Value = ""
                hdnSelectfqdn.Value = ""
                hdnSelectPort.Value = ""
                hdnSelectPath.Value = ""
                hdnSelectUseFqdnInsteadOfIP.Value = ""
                hdnSelectIsTlsEncryted.Value = ""
                hdnSelectMonitoringLevel.Value = ""
                ShowGrid()
                ShowDropDown()
            End If
        ElseIf (InStr(hdnTitle.Value, "Create", CompareMethod.Text) > 0) Then
            hdnSelectSecurityTool.Value = txtSecurityTool.Text.Trim
            hdnSelectInstance.Value = txtInstance.Text.Trim
            hdnSelectDomain.Value = txtDomain.Text.Trim
            hdnSelectIP.Value = txtIP.Text.Trim
            hdnSelecthostname.Value = txthostname.Text.Trim
            hdnSelectfqdn.Value = txtfqdn.Text.Trim
            hdnSelectPort.Value = txtPort.Text.Trim
            hdnSelectPath.Value = txtPath.Text.Trim
            hdnSelectUseFqdnInsteadOfIP.Value = cblUseFqdnInsteadOfIP.SelectedValue
            hdnSelectIsTlsEncryted.Value = cblIsTlsEncryted.SelectedValue
            hdnSelectMonitoringLevel.Value = cblMonitoringLevel.SelectedValue
            If (hdnSelectSecurityTool.Value.Trim = "") Then
                ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "javascript:alertMsg('" & sender.ID & "','Please input a SecurityTool!','Missing Input');", True)
                ClientScript.RegisterStartupScript(Me.[GetType](), "create", "javascript:inputShow('Create Records');", True)
            ElseIf (hdnSelectInstance.Value.Trim = "") Then
                ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "javascript:alertMsg('" & sender.ID & "','Please input an Instance!','Missing Input');", True)
                ClientScript.RegisterStartupScript(Me.[GetType](), "create", "javascript:inputShow('Create Records');", True)
            ElseIf (hdnSelectDomain.Value.Trim = "") Then
                ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "javascript:alertMsg('" & sender.ID & "','Please input a Domain!','Missing Input');", True)
                ClientScript.RegisterStartupScript(Me.[GetType](), "create", "javascript:inputShow('Create Records');", True)
            ElseIf (hdnSelectIP.Value.Trim = "") Then
                ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "javascript:alertMsg('" & sender.ID & "','Please input an IP!','Missing Input');", True)
                ClientScript.RegisterStartupScript(Me.[GetType](), "create", "javascript:inputShow('Create Records');", True)
            ElseIf (hdnSelectMonitoringLevel.Value.Trim = "") Then
                ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "javascript:alertMsg('" & sender.ID & "','Please input a MonitoringLevel!','Missing Input');", True)
                ClientScript.RegisterStartupScript(Me.[GetType](), "create", "javascript:inputShow('Create Records');", True)
            Else
                Dim con As SqlClient.SqlConnection = New SqlClient.SqlConnection(modMain.ConnectionString)
                Try
                    con.Open()
                    Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand("CMN.USP_CrudInsertMonitoredNode", con)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandTimeout = 0

                    cmd.Parameters.Add("@SecurityTool", System.Data.SqlDbType.NVarChar, 300).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@SecurityTool").Value = hdnSelectSecurityTool.Value

                    cmd.Parameters.Add("@Instance", System.Data.SqlDbType.NVarChar, 300).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@Instance").Value = hdnSelectInstance.Value

                    cmd.Parameters.Add("@Domain", System.Data.SqlDbType.NVarChar, 300).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@Domain").Value = hdnSelectDomain.Value

                    cmd.Parameters.Add("@IP", System.Data.SqlDbType.NVarChar, 50).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@IP").Value = hdnSelectIP.Value

                    cmd.Parameters.Add("@hostname", System.Data.SqlDbType.NVarChar, 50).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@hostname").Value = modMain.NothingtoDBNull(hdnSelecthostname.Value)

                    cmd.Parameters.Add("@fqdn", System.Data.SqlDbType.NVarChar, 100).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@fqdn").Value = modMain.NothingtoDBNull(hdnSelectfqdn.Value)

                    cmd.Parameters.Add("@Port", System.Data.SqlDbType.NVarChar, 10).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@Port").Value = modMain.NothingtoDBNull(hdnSelectPort.Value)

                    cmd.Parameters.Add("@Path", System.Data.SqlDbType.NVarChar, 400).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@Path").Value = modMain.NothingtoDBNull(modMain.ManageAddress(hdnSelectPath.Value, "/", modMain.PathPart.AtBegin))

                    cmd.Parameters.Add("@UseFqdnInsteadOfIP", System.Data.SqlDbType.Bit).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@UseFqdnInsteadOfIP").Value = modMain.NothingtoDBNull(hdnSelectUseFqdnInsteadOfIP.Value)

                    cmd.Parameters.Add("@IsTlsEncryted", System.Data.SqlDbType.Bit).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@IsTlsEncryted").Value = modMain.NothingtoDBNull(hdnSelectIsTlsEncryted.Value)

                    cmd.Parameters.Add("@MonitoringLevel", System.Data.SqlDbType.TinyInt).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@MonitoringLevel").Value = hdnSelectMonitoringLevel.Value

                    cmd.Parameters.Add("@UserAccount", System.Data.SqlDbType.NVarChar, 50).Direction = System.Data.ParameterDirection.Input
                    cmd.Parameters.Item("@UserAccount").Value = DBNull.Value

                    cmd.ExecuteNonQuery()
                    cmd.Dispose()
                Catch ex As Exception

                Finally
                    con.Close()
                    con.Dispose()
                End Try
                hdnSelectSecurityTool.Value = ""
                hdnSelectInstance.Value = ""
                hdnSelectDomain.Value = ""
                hdnSelectIP.Value = ""
                hdnSelecthostname.Value = ""
                hdnSelectfqdn.Value = ""
                hdnSelectPort.Value = ""
                hdnSelectPath.Value = ""
                hdnSelectUseFqdnInsteadOfIP.Value = ""
                hdnSelectIsTlsEncryted.Value = ""
                hdnSelectMonitoringLevel.Value = ""
                ShowGrid()
                ShowDropDown()
            End If
        ElseIf (InStr(hdnTitle.Value, "Filter", CompareMethod.Text) > 0) Then
            hdnWhereSecurityTool.Value = txtSecurityTool.Text.Trim
            hdnWhereInstance.Value = txtInstance.Text.Trim
            hdnWhereDomain.Value = txtDomain.Text.Trim
            hdnWhereIP.Value = txtIP.Text.Trim
            hdnWherehostname.Value = txthostname.Text.Trim
            hdnWherefqdn.Value = txtfqdn.Text.Trim
            hdnWherePort.Value = txtPort.Text.Trim
            hdnWherePath.Value = txtPath.Text.Trim
            hdnWhereUseFqdnInsteadOfIP.Value = cblUseFqdnInsteadOfIP.SelectedValue
            hdnWhereIsTlsEncryted.Value = cblIsTlsEncryted.SelectedValue
            hdnWhereMonitoringLevel.Value = cblMonitoringLevel.SelectedValue
            ShowGrid()
        End If
        hdnTitle.Value = ""
    End Sub

    Protected Sub btnCancelInput_Click(sender As Object, e As EventArgs) Handles btnCancelInput.Click
        hdnSelectSecurityTool.Value = ""
        hdnSelectInstance.Value = ""
        hdnSelectDomain.Value = ""
        hdnSelectIP.Value = ""
        hdnSelecthostname.Value = ""
        hdnSelectfqdn.Value = ""
        hdnSelectPort.Value = ""
        hdnSelectPath.Value = ""
        hdnSelectUseFqdnInsteadOfIP.Value = ""
        hdnSelectIsTlsEncryted.Value = ""
        hdnSelectMonitoringLevel.Value = ""

        txtSecurityTool.Text = ""
        txtInstance.Text = ""
        txtDomain.Text = ""
        txtIP.Text = ""
        txthostname.Text = ""
        txtfqdn.Text = ""
        txtPort.Text = ""
        txtPath.Text = ""
        cblUseFqdnInsteadOfIP.SelectedIndex = 0
        cblIsTlsEncryted.SelectedIndex = 0
        cblMonitoringLevel.SelectedIndex = 0
    End Sub

    Protected Sub btnSubmitSort_Click(sender As Object, e As EventArgs) Handles btnSubmitSort.Click
        Dim sort As String = ""
        If (cblOrder01.Text <> "") Then sort &= IIf(sort = "", "", ",") & cblOrder01.Text & IIf(descOrder01.Checked, " DESC", "")
        If (cblOrder02.Text <> "") Then sort &= IIf(sort = "", "", ",") & cblOrder02.Text & IIf(descOrder02.Checked, " DESC", "")
        If (cblOrder03.Text <> "") Then sort &= IIf(sort = "", "", ",") & cblOrder03.Text & IIf(descOrder03.Checked, " DESC", "")
        If (cblOrder04.Text <> "") Then sort &= IIf(sort = "", "", ",") & cblOrder04.Text & IIf(descOrder04.Checked, " DESC", "")
        If (cblOrder05.Text <> "") Then sort &= IIf(sort = "", "", ",") & cblOrder05.Text & IIf(descOrder05.Checked, " DESC", "")
        If (cblOrder06.Text <> "") Then sort &= IIf(sort = "", "", ",") & cblOrder06.Text & IIf(descOrder06.Checked, " DESC", "")
        If (cblOrder07.Text <> "") Then sort &= IIf(sort = "", "", ",") & cblOrder07.Text & IIf(descOrder07.Checked, " DESC", "")
        If (cblOrder08.Text <> "") Then sort &= IIf(sort = "", "", ",") & cblOrder08.Text & IIf(descOrder08.Checked, " DESC", "")
        If (cblOrder09.Text <> "") Then sort &= IIf(sort = "", "", ",") & cblOrder09.Text & IIf(descOrder09.Checked, " DESC", "")
        If (cblOrder10.Text <> "") Then sort &= IIf(sort = "", "", ",") & cblOrder10.Text & IIf(descOrder10.Checked, " DESC", "")
        If (cblOrder11.Text <> "") Then sort &= IIf(sort = "", "", ",") & cblOrder11.Text & IIf(descOrder11.Checked, " DESC", "")
        hdnSort.Value = sort
        ShowGrid()
    End Sub

    Protected Sub btnCancelSort_Click(sender As Object, e As EventArgs) Handles btnCancelSort.Click
        cblOrder01.SelectedIndex = 0
        cblOrder02.SelectedIndex = 0
        cblOrder03.SelectedIndex = 0
        cblOrder04.SelectedIndex = 0
        cblOrder05.SelectedIndex = 0
        cblOrder06.SelectedIndex = 0
        cblOrder07.SelectedIndex = 0
        cblOrder08.SelectedIndex = 0
        cblOrder09.SelectedIndex = 0
        cblOrder10.SelectedIndex = 0
        cblOrder11.SelectedIndex = 0

        descOrder01.Checked = False
        descOrder02.Checked = False
        descOrder03.Checked = False
        descOrder04.Checked = False
        descOrder05.Checked = False
        descOrder06.Checked = False
        descOrder07.Checked = False
        descOrder08.Checked = False
        descOrder09.Checked = False
        descOrder10.Checked = False
        descOrder11.Checked = False
    End Sub
End Class