<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="monitorednodes.aspx.vb" Inherits="stomonitoring.monitorednodes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/monitorednodes.css" rel="stylesheet" type="text/css" />
    <link href="css/custom-theme/jquery-ui-1.10.1.custom.min.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="js/jquery-ui-1.10.1.custom.min.js" type="text/javascript"></script>
    <script language="javascript">
        function confirmMsg(uniqueID, msgPrompt, msgTitle) {
            $('#msgBox').dialog({
                title: msgTitle,
                resizable: false,
                height: 200,
                width: 400,
                modal: true,
                buttons: {
                    'Yes': function () {
                        $('#msgResult').val('yes');
                        __doPostBack(uniqueID, '');
                        $(this).dialog('close');
                    },
                    'No': function () {
                        $(this).dialog('close');
                    }
                }
            });
            $('#lblMsg').html(msgPrompt);
            $('#msgBox').dialog('open');
            return false;
        }
        function alertMsg(uniqueID, msgPrompt, msgTitle) {
            $('#msgBox').dialog({
                title: msgTitle,
                resizable: false,
                height: 200,
                width: 400,
                modal: true,
                buttons: {
                    'Ok': function () {
                        $(this).dialog('close');
                    }
                }
            });
            $('#lblMsg').html(msgPrompt);
            $('#msgBox').dialog('open');
            return false;
        }
    </script> 
    <script>
        function inputShow(paramTitle) {
            document.getElementById("hdnTitle").value = paramTitle;
            document.getElementById("lblTitleInput").innerHTML = document.getElementById("hdnTitle").value;
            if ((document.getElementById("hdnTitle").value.toLowerCase().indexOf("update") >= 0) || (document.getElementById("hdnTitle").value.toLowerCase().indexOf("create") >= 0)) {
                document.getElementById("txtSecurityTool").value = document.getElementById("hdnSelectSecurityTool").value;
                document.getElementById("txtInstance").value = document.getElementById("hdnSelectInstance").value;
                document.getElementById("txtDomain").value = document.getElementById("hdnSelectDomain").value;
                document.getElementById("txtIP").value = document.getElementById("hdnSelectIP").value;
                document.getElementById("txthostname").value = document.getElementById("hdnSelecthostname").value;
                document.getElementById("txtfqdn").value = document.getElementById("hdnSelectfqdn").value;
                document.getElementById("txtPort").value = document.getElementById("hdnSelectPort").value;
                document.getElementById("txtPath").value = document.getElementById("hdnSelectPath").value;
                document.getElementById("cblUseFqdnInsteadOfIP").value = document.getElementById("hdnSelectUseFqdnInsteadOfIP").value;
                document.getElementById("cblIsTlsEncryted").value = document.getElementById("hdnSelectIsTlsEncryted").value;
                document.getElementById("cblMonitoringLevel").value = document.getElementById("hdnSelectMonitoringLevel").value;
            } else if (document.getElementById("hdnTitle").value.toLowerCase().indexOf("filter") >= 0) {
                document.getElementById("txtSecurityTool").value = document.getElementById("hdnWhereSecurityTool").value;
                document.getElementById("txtInstance").value = document.getElementById("hdnWhereInstance").value;
                document.getElementById("txtDomain").value = document.getElementById("hdnWhereDomain").value;
                document.getElementById("txtIP").value = document.getElementById("hdnWhereIP").value;
                document.getElementById("txthostname").value = document.getElementById("hdnWherehostname").value;
                document.getElementById("txtfqdn").value = document.getElementById("hdnWherefqdn").value;
                document.getElementById("txtPort").value = document.getElementById("hdnWherePort").value;
                document.getElementById("txtPath").value = document.getElementById("hdnWherePath").value;
                document.getElementById("cblUseFqdnInsteadOfIP").value = document.getElementById("hdnWhereUseFqdnInsteadOfIP").value;
                document.getElementById("cblIsTlsEncryted").value = document.getElementById("hdnWhereIsTlsEncryted").value;
                document.getElementById("cblMonitoringLevel").value = document.getElementById("hdnWhereMonitoringLevel").value;
            }
            document.getElementById("overlayInput").style.display = "block";
        }

        function sortShow(paramTitle) {
            document.getElementById("hdnTitle").value = paramTitle;
            document.getElementById("lblTitleSort").innerHTML = document.getElementById("hdnTitle").value;

            document.getElementById("cblOrder01").value = '';
            document.getElementById("cblOrder02").value = '';
            document.getElementById("cblOrder03").value = '';
            document.getElementById("cblOrder04").value = '';
            document.getElementById("cblOrder05").value = '';
            document.getElementById("cblOrder06").value = '';
            document.getElementById("cblOrder07").value = '';
            document.getElementById("cblOrder08").value = '';
            document.getElementById("cblOrder09").value = '';
            document.getElementById("cblOrder10").value = '';
            document.getElementById("cblOrder11").value = '';

            document.getElementById("descOrder01").checked = false;
            document.getElementById("descOrder02").checked = false;
            document.getElementById("descOrder03").checked = false;
            document.getElementById("descOrder04").checked = false;
            document.getElementById("descOrder05").checked = false;
            document.getElementById("descOrder06").checked = false;
            document.getElementById("descOrder07").checked = false;
            document.getElementById("descOrder08").checked = false;
            document.getElementById("descOrder09").checked = false;
            document.getElementById("descOrder10").checked = false;
            document.getElementById("descOrder11").checked = false;

            var sort = document.getElementById("hdnSort").value;
            if (sort != '') {
                var sortArr = sort.split(',');
                for (i = 1; i <= sortArr.length; i++) {
                    var field = sortArr[i - 1];
                    var fieldArr = field.split(' ');
                    document.getElementById("cblOrder" + ('0' + i).slice(-2)).value = fieldArr[0];
                    if (fieldArr.length > 1) {
                        document.getElementById("descOrder" + ('0' + i).slice(-2)).checked = true;
                    }
                }
            }

            document.getElementById("overlaySort").style.display = "block";
        }
    </script>
</head>
<body>
    <form id="frmMonitoredNodes" runat="server">
        <input type="hidden" name="msgResult" id="msgResult" />
        <div id="msgBox" ><span id="lblMsg" /></div>
        <asp:HiddenField ID="hdnTitle" runat="server" />
        <asp:HiddenField ID="hdnSelectMonitoredNodeId" runat="server" />
        <asp:HiddenField ID="hdnSelectSecurityTool" runat="server" />
        <asp:HiddenField ID="hdnSelectInstance" runat="server" />
        <asp:HiddenField ID="hdnSelectDomain" runat="server" />
        <asp:HiddenField ID="hdnSelectIP" runat="server" />
        <asp:HiddenField ID="hdnSelecthostname" runat="server" />
        <asp:HiddenField ID="hdnSelectfqdn" runat="server" />
        <asp:HiddenField ID="hdnSelectPort" runat="server" />
        <asp:HiddenField ID="hdnSelectPath" runat="server" />
        <asp:HiddenField ID="hdnSelectUseFqdnInsteadOfIP" runat="server" />
        <asp:HiddenField ID="hdnSelectIsTlsEncryted" runat="server" />
        <asp:HiddenField ID="hdnSelectMonitoringLevel" runat="server" />
        <asp:HiddenField ID="hdnWhereSecurityTool" runat="server" />
        <asp:HiddenField ID="hdnWhereInstance" runat="server" />
        <asp:HiddenField ID="hdnWhereDomain" runat="server" />
        <asp:HiddenField ID="hdnWhereIP" runat="server" />
        <asp:HiddenField ID="hdnWherehostname" runat="server" />
        <asp:HiddenField ID="hdnWherefqdn" runat="server" />
        <asp:HiddenField ID="hdnWherePort" runat="server" />
        <asp:HiddenField ID="hdnWherePath" runat="server" />
        <asp:HiddenField ID="hdnWhereUseFqdnInsteadOfIP" runat="server" />
        <asp:HiddenField ID="hdnWhereIsTlsEncryted" runat="server" />
        <asp:HiddenField ID="hdnWhereMonitoringLevel" runat="server" />
        <asp:HiddenField ID="hdnSort" runat="server" />
        <div id="overlayInput" class="overlay">
            <div class="inputs">
                <div class="overlayTitle"><span id="lblTitleInput" ></span></div>
                <div>
                    <table>
                        <tr><td class="inputLabel">SecurityTool*:</td>
                            <td>
                                <asp:TextBox ID="txtSecurityTool" runat="server" CssClass="inputTextBox" list="dlstSecurityTool"></asp:TextBox>
                                <datalist id="dlstSecurityTool" runat="server"></datalist>
                            </td>
                        </tr>
                        <tr><td class="inputLabel">Instance*:</td>
                            <td>
                                <asp:TextBox ID="txtInstance" runat="server" CssClass="inputTextBox" list="dlstInstance"></asp:TextBox>
                                <datalist id="dlstInstance" runat="server"></datalist>
                            </td>
                        </tr>
                        <tr><td class="inputLabel">Domain*:</td>
                            <td>
                                <asp:TextBox ID="txtDomain" runat="server" CssClass="inputTextBox" list="dlstDomain"></asp:TextBox>
                                <datalist id="dlstDomain" runat="server"></datalist>
                            </td>
                        </tr>
                        <tr><td class="inputLabel">IP*:</td><td><asp:TextBox ID="txtIP" runat="server" CssClass="inputTextBox"></asp:TextBox></td></tr>
                        <tr><td class="inputLabel">hostname:</td><td><asp:TextBox ID="txthostname" runat="server" CssClass="inputTextBox"></asp:TextBox></td></tr>
                        <tr><td class="inputLabel">fqdn:</td><td><asp:TextBox ID="txtfqdn" runat="server" CssClass="inputTextBox"></asp:TextBox></td></tr>
                        <tr><td class="inputLabel">Port:</td><td><asp:TextBox ID="txtPort" runat="server" CssClass="inputTextBox"></asp:TextBox></td></tr>
                        <tr><td class="inputLabel">Path:</td><td><asp:TextBox ID="txtPath" runat="server" CssClass="inputTextBox"></asp:TextBox></td></tr>
                        <tr><td class="inputLabel">UseFqdnInsteadOfIP:</td><td><asp:DropDownList ID="cblUseFqdnInsteadOfIP" runat="server" CssClass="inputDropDownList"></asp:DropDownList></td></tr>
                        <tr><td class="inputLabel">IsTlsEncryted:</td><td><asp:DropDownList ID="cblIsTlsEncryted" runat="server" CssClass="inputDropDownList"></asp:DropDownList></td></tr>
                        <tr><td class="inputLabel">MonitoringLevel*:</td><td><asp:DropDownList ID="cblMonitoringLevel" runat="server" CssClass="inputDropDownList"></asp:DropDownList></td></tr>
                    </table>
                </div>
                <div>
                    <table class="overlayControls">
                        <tr>
                            <td>
                                <asp:Button ID="btnSubmitInput" runat="server" Text="Submit" CssClass="command" />
                            </td>
                            <td>
                                <asp:Button ID="btnCancelInput" runat="server" Text="Cancel" CssClass="command" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div id="overlaySort" class="overlay">
            <div class="inputs">
                <div class="overlayTitle"><span id="lblTitleSort" ></span></div>
                <div>
                    <table>
                        <tr><td>1</td><td><asp:DropDownList ID="cblOrder01" runat="server" CssClass="sortListBox"></asp:DropDownList></td><td><asp:CheckBox ID="descOrder01" runat="server" Text="DESC"></asp:CheckBox></td></tr>
                        <tr><td>2</td><td><asp:DropDownList ID="cblOrder02" runat="server" CssClass="sortListBox"></asp:DropDownList></td><td><asp:CheckBox ID="descOrder02" runat="server" Text="DESC"></asp:CheckBox></td></tr>
                        <tr><td>3</td><td><asp:DropDownList ID="cblOrder03" runat="server" CssClass="sortListBox"></asp:DropDownList></td><td><asp:CheckBox ID="descOrder03" runat="server" Text="DESC"></asp:CheckBox></td></tr>
                        <tr><td>4</td><td><asp:DropDownList ID="cblOrder04" runat="server" CssClass="sortListBox"></asp:DropDownList></td><td><asp:CheckBox ID="descOrder04" runat="server" Text="DESC"></asp:CheckBox></td></tr>
                        <tr><td>5</td><td><asp:DropDownList ID="cblOrder05" runat="server" CssClass="sortListBox"></asp:DropDownList></td><td><asp:CheckBox ID="descOrder05" runat="server" Text="DESC"></asp:CheckBox></td></tr>
                        <tr><td>6</td><td><asp:DropDownList ID="cblOrder06" runat="server" CssClass="sortListBox"></asp:DropDownList></td><td><asp:CheckBox ID="descOrder06" runat="server" Text="DESC"></asp:CheckBox></td></tr>
                        <tr><td>7</td><td><asp:DropDownList ID="cblOrder07" runat="server" CssClass="sortListBox"></asp:DropDownList></td><td><asp:CheckBox ID="descOrder07" runat="server" Text="DESC"></asp:CheckBox></td></tr>
                        <tr><td>8</td><td><asp:DropDownList ID="cblOrder08" runat="server" CssClass="sortListBox"></asp:DropDownList></td><td><asp:CheckBox ID="descOrder08" runat="server" Text="DESC"></asp:CheckBox></td></tr>
                        <tr><td>9</td><td><asp:DropDownList ID="cblOrder09" runat="server" CssClass="sortListBox"></asp:DropDownList></td><td><asp:CheckBox ID="descOrder09" runat="server" Text="DESC"></asp:CheckBox></td></tr>
                        <tr><td>10</td><td><asp:DropDownList ID="cblOrder10" runat="server" CssClass="sortListBox"></asp:DropDownList></td><td><asp:CheckBox ID="descOrder10" runat="server" Text="DESC"></asp:CheckBox></td></tr>
                        <tr><td>11</td><td><asp:DropDownList ID="cblOrder11" runat="server" CssClass="sortListBox"></asp:DropDownList></td><td><asp:CheckBox ID="descOrder11" runat="server" Text="DESC"></asp:CheckBox></td></tr>
                    </table>
                </div>
                <div>
                    <table class="overlayControls">
                        <tr>
                            <td>
                                <asp:Button ID="btnSubmitSort" runat="server" Text="Submit" CssClass="command" />
                            </td>
                            <td>
                                <asp:Button ID="btnCancelSort" runat="server" Text="Cancel" CssClass="command" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div id="container">
            <div id="lbl">Monitored Node Status Inventory</div>
            <div>
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnDelete" CssClass="command" runat="server" Text="Delete" OnClientClick="javascript:return confirmMsg(this.id,'You are about to delete.<br />Are you sure?','Confirm');" />
                        </td>
                        <td>
                            <input id="btnCreate" class="command" type="button" value="Create" onclick="inputShow('Create Records')" />
                        </td>
                        <td>
                            <input id="btnFilter" class="command" type="button" value="Filter" onclick="inputShow('Filter Records')" />
                        </td>
                        <td>
                            <input id="btnSort" class="command" type="button" value="Sort" onclick="sortShow('Sort Records')" />
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <asp:GridView ID="gridMonitoredNodes" runat="server" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" EnableModelValidation="True" GridLines="Horizontal" Width="100%" AutoGenerateColumns="False">
                    <columns>
                        <asp:boundfield DataField="MonitoredNodeId"></asp:boundfield>
                        <asp:templatefield>
                            <itemtemplate>
                                <asp:checkbox ID="cbSelect" runat="server"></asp:checkbox>
                            </itemtemplate>
                        </asp:templatefield>
                        <asp:boundfield DataField="SecurityTool" HeaderText="SecurityTool"></asp:boundfield>
                        <asp:boundfield DataField="Instance" HeaderText="Instance"></asp:boundfield>
                        <asp:boundfield DataField="Domain" HeaderText="Domain"></asp:boundfield>
                        <asp:templatefield HeaderText = "IP">
                            <itemtemplate>
                                <asp:LinkButton ID="lnkIP" runat="server" Text='<%# Eval("IP") %>' CommandName="updateMonitoredNode" CommandArgument='<%# Eval("MonitoredNodeId") %>'></asp:LinkButton>
                            </itemtemplate>
                        </asp:templatefield>
                        <asp:boundfield DataField="hostname" HeaderText="hostname"></asp:boundfield>
                        <asp:boundfield DataField="fqdn" HeaderText="fqdn"></asp:boundfield>
                        <asp:boundfield DataField="Port" HeaderText="Port"></asp:boundfield>
                        <asp:boundfield DataField="Path" HeaderText="Path"></asp:boundfield>
                        <asp:boundfield DataField="UseFqdnInsteadOfIP" HeaderText="UseFqdnInsteadOfIP"></asp:boundfield>
                        <asp:boundfield DataField="IsTlsEncryted" HeaderText="IsTlsEncryted"></asp:boundfield>
                        <asp:boundfield DataField="MonitoringLevel" HeaderText="MonitoringLevel"></asp:boundfield>
                    </columns>
                    <HeaderStyle BackColor="#D6DFEF" />
                </asp:GridView>
            </div>
            <div>
                <table id="navigate">
                    <tr>
                        <td id="colBtnFrst"><asp:Button ID="btnFrst" runat="server" Text="|<" CssClass="controls" /></td>
                        <td id="colBtnPrev"><asp:Button ID="btnPrev" runat="server" Text="<<" CssClass="controls" /></td>
                        <td id="colTxtPageNo"><asp:TextBox ID="txtPage" runat="server">1</asp:TextBox></td>
                        <td id="colLblTotalPages"><asp:Label ID="lblTotal" runat="server" Text="of" CssClass="controls"></asp:Label></td>
                        <td id="colBtnNext"><asp:Button ID="btnNext" runat="server" Text=">>" CssClass="controls" /></td>
                        <td id="colBtnLast"><asp:Button ID="btnLast" runat="server" Text=">|" CssClass="controls" /></td>
                        <td id="colBtnRefresh"><asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="controls" /></td>
                        <td id="colLblRowCountPerPage">Number of items per page:</td>
                        <td id="colTxtRowCountPerPage"><asp:TextBox ID="txtRowCountPerPage" runat="server" CssClass="controls" >20</asp:TextBox></td>
                        <td id="colLblDisplay"><asp:Label ID="lblDisplay" runat="server" Text="Displaying Approved Files 0 - 0 of 0" CssClass="controls"></asp:Label></td>
                        <td id="colLblTotalRowCount"><asp:Label ID="lblTotalRowCount" runat="server" Text="1000" CssClass="controls"></asp:Label></td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
