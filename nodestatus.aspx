<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="nodestatus.aspx.vb" Inherits="stomonitoring.nodestatus" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="frmNodeStatus" runat="server">
        <asp:ScriptManager ID="smNodeStatus" runat="server" />
        <div>
            <asp:Timer ID="tmrAsOf" OnTick="tmrAsOf_Tick" runat="server"></asp:Timer>
        </div>
        <asp:UpdatePanel ID="upNodeStatus" UpdateMode="Conditional" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="tmrAsOf" EventName="Tick" />
            </Triggers>
            <ContentTemplate>
                <div>
                    <div style="text-align: center;padding-bottom: 20px;"><asp:Label ID="lbl" runat="server" Font-Names="arial" Font-Size="XX-Large" Text="Node Status"></asp:Label></div>
                    <asp:GridView ID="gridNodeStatus" runat="server" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" EnableModelValidation="True" Font-Names="Arial" Font-Size="Small" GridLines="Horizontal" Width="100%">
                        <HeaderStyle BackColor="#D6DFEF" Font-Names="arial" Font-Size="Small" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
