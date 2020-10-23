<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ClientUI.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Auditor Portal</title>
</head>
<body>
    <link href="//maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css">
    <script src="//maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"></script>
    <script src="//code.jquery.com/jquery-1.11.1.min.js"></script>

    <!------ Include the above in your HEAD tag ---------->
    <link href="css/inner.css" rel="stylesheet" />
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <div class="sidenav">
                <div class="login-main-text">
                    <table>

                        <tr>
                            <td>
                                <h5>Welcome </h5>
                            </td>

                            <td style="padding-left: 10px;">
                                <h5>
                                    <asp:Label ID="lblAuditorname" runat="server"></asp:Label></h5>
                            </td>
                        </tr>

                    </table>

                    <br>
                    <table>
                        <%--  
                        <tr>
                            <td>Total Task : 
                            </td>
                            <td style="padding-left: 10px;">
                                <asp:Label ID="lblTaskCount" runat="server"> </asp:Label>
                            </td>

                        </tr>--%>

                        <%-- <tr>
                            <td>Total Comments : 
                            </td>
                            <td style="padding-left: 10px;">
                                <asp:Label ID="Label1" runat="server"> 12</asp:Label>

                            </td>
                        </tr>
                        <tr>
                            <td>Total Documents : 
                            </td>
                            <td style="padding-left: 10px;">
                                <asp:Label ID="Label2" runat="server"> 12</asp:Label>

                            </td>
                        </tr>
                        <tr>

                            <td style="padding-left: 10px;">
                                <asp:LinkButton ID="lnkLogout" runat="server" Text="Logout"></asp:LinkButton>

                            </td>
                        </tr>--%>
                    </table>

                </div>
            </div>
        </div>

        <%-- Create Task Section--%>
        <div class="main">
            <header style="padding-bottom: 60px;"></header>
            <div class="card bg-light mb-3" style="width: 60rem;">
                <header>
                    <h4 style="color: #000000; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-style: italic;">Create Task</h4>
                </header>
                <%-- <img src="..." class="card-img-top" alt="...">--%>
                <div class="card-body">

                    <asp:UpdatePanel runat="server" ID="up" UpdateMode="Always" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="row">
                                <table>
                                    <tr>
                                        <td>Select Portfolio</td>
                                        <td>
                                            <asp:DropDownList ID="ddlPortfolio" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlPortfolio_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Text="- Select Audit Portfolio-" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                    </tr>
                                    <tr>
                                        <td>Select Client</td>
                                        <td>
                                            <asp:DropDownList ID="ddlClient" AutoPostBack="true" runat="server">
                                                <asp:ListItem Selected="True" Text="-Audit Client-" Value="0"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>Enter Task</td>
                                        <td>
                                            <asp:TextBox ID="txtTask" runat="server" TextMode="MultiLine"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="dtnCreateTask" runat="server" CssClass="align-content-around" Text="Create Task" OnClick="dtnCreateTask_Click" /></td>
                                                    <%-- <td>
                                                        <asp:Button ID="btnReset" runat="server" CssClass="align-content-around" Text="Reset" Visible="false" OnClick="btnReset_Click" />
                                                        <td>--%>
                                                    <asp:Button ID="dtnSearch" runat="server" CssClass="align-content-around" Text="View Task" OnClick="dtnSearch_Click" />
                                        </td>
                                        </td>
                                    </tr>
                                </table>
                                </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>


        <%-- Task Details Section--%>

        <div class="main">
            <header style="padding-bottom: 60px;"></header>
            <div class="card bg-light mb-3" style="width: 60rem;">
                <header>
                    <h4 style="color: #000000; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-style: italic;">View Task</h4>
                </header>

                <div class="card-body">

                    <asp:UpdatePanel runat="server" ID="UpdatePanelViewTaskComment" UpdateMode="Always" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="row">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="grdViewTask" runat="server" Width="650px" BorderStyle="None" HeaderStyle-BackColor="#000" HeaderStyle-ForeColor="White"
                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                <Columns>

                                                    <%--  <asp:BoundField DataField="ClientName" HeaderText="Name" ItemStyle-Width="150" />--%>
                                                    <asp:TemplateField HeaderText="Task Details" ItemStyle-Width="350">
                                                        <ItemTemplate>
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblTask" runat="server" Text='<%# Eval("TaskDescription") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Task ID" Visible="false" ItemStyle-Width="350">
                                                        <ItemTemplate>
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblTaskid" runat="server" Text='<%# Eval("Tskid") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="CID" Visible="false" ItemStyle-Width="350">
                                                        <ItemTemplate>
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblCID" runat="server" Text='<%# Eval("Cid") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PID" Visible="false" ItemStyle-Width="350">
                                                        <ItemTemplate>
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblPID" runat="server" Text='<%# Eval("Pid") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Messages">
                                                        <ItemTemplate>
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkRowComments" runat="server" AutoPostBack="true" OnCheckedChanged="chkRowComments_OnCheckedChanged" /></td>
                                                                </tr>
                                                            </table>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Documents" ItemStyle-Width="350">
                                                        <ItemTemplate>
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkRowDoc" runat="server" OnCheckedChanged="chkRowDoc_CheckedChanged" AutoPostBack="true" /></td>
                                                                </tr>
                                                            </table>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Complete Task" ItemStyle-Width="350">
                                                        <ItemTemplate>
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkRowClose" runat="server" AutoPostBack="true" OnCheckedChanged="chkRowClose_CheckedChanged" /></td>
                                                                </tr>
                                                            </table>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                                <EmptyDataTemplate>No Record Available right now..!!!</EmptyDataTemplate>
                                            </asp:GridView>
                                    </tr>
                                </table>


                            </div>


                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Button ID="btnTs" runat="server" Visible="false" OnClick="btnTs_Click" />

                </div>
            </div>

        </div>

        <%--  Message Section--%>

        <div class="main">
            <header style="padding-bottom: 60px;"></header>
            <div class="card bg-light mb-3" style="width: 60rem;">
                <header>
                    <h4 style="color: #000000; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-style: italic;">Messages</h4>
                </header>

                <div class="card-body">

                    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Always" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="row">
                                <table>
                                    <tr>
                                        <td>New Message :  
                                            <asp:TextBox runat="server" ID="txtSendComment"></asp:TextBox>
                                            <asp:Button ID="btnSendMsg" Style="padding-left: 10px;" runat="server" Text="Send" OnClick="btnSendMsg_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="grdViewComment" runat="server" Width="650px" BorderStyle="None" HeaderStyle-BackColor="#000" HeaderStyle-ForeColor="White"
                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Messages" ItemStyle-Width="350">
                                                        <ItemTemplate>
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblComment" runat="server" Text='<%# Eval("CommentDetails") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Commented By">
                                                        <ItemTemplate>
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblCommentedBy" runat="server" Text='<%# Eval("CommentedBy") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                </Columns>
                                                <EmptyDataTemplate>No Record Available right now..!!!</EmptyDataTemplate>
                                            </asp:GridView>
                                    </tr>
                                </table>


                            </div>


                        </ContentTemplate>
                    </asp:UpdatePanel>


                </div>
            </div>

        </div>

        <%--  Document Section--%>

        <div class="main">
            <asp:Panel ID="pnlDocument" runat="server">
                <header style="padding-bottom: 60px;"></header>
                <div class="card bg-light mb-3" style="width: 60rem;">
                    <header>
                        <h4 style="color: #000000; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-style: italic;">Documents</h4>
                    </header>

                    <div class="card-body">

                        <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Always" ChildrenAsTriggers="true">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnUpload" />
                            </Triggers>
                            <ContentTemplate>


                                <div class="row">
                                    Add New:
                                    <asp:FileUpload ID="FileUploadDoc" runat="server" />
                                    <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />

                                </div>
                                <div class="row">
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <asp:GridView ID="grvDocument" runat="server" Width="650px" BorderStyle="None" HeaderStyle-BackColor="#000" HeaderStyle-ForeColor="White"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                    <Columns>

                                                        <asp:TemplateField HeaderText="Task ID" Visible="false" ItemStyle-Width="350">
                                                            <ItemTemplate>
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblTaskid" runat="server" Text='<%# Eval("Tskid") %>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="CID" Visible="false" ItemStyle-Width="350">
                                                            <ItemTemplate>
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblCID" runat="server" Text='<%# Eval("Cid") %>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="DOCUID" Visible="false" ItemStyle-Width="350">
                                                            <ItemTemplate>
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblDOCUID" runat="server" Text='<%# Eval("DOCUID") %>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Document">

                                                            <ItemTemplate>
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td>

                                                                            <asp:LinkButton ID="lnkDocumentName" runat="server" Text='<%# Eval("DocumentName") %>' OnClick="lnkDocumentName_Click"></asp:LinkButton>

                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Delete">
                                                            <ItemTemplate>
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkDelete" runat="server" AutoPostBack="true"  OnCheckedChanged="chkDelete_CheckedChanged"></asp:CheckBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>No Record Available right now..!!!</EmptyDataTemplate>
                                                </asp:GridView>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>


                    </div>
                </div>
            </asp:Panel>
        </div>

    </form>
</body>
</html>
