<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ClientUI.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Client Portal</title>
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
                                <h4>Welcome </h4>
                            </td>

                            <td style="padding-left: 10px;">
                                <h4>
                                    <asp:Label ID="lblClientname" runat="server"></asp:Label></h4>
                            </td>
                        </tr>

                    </table>

                    <br>
                    <table>
                        <tr>
                            <td>Total Task : 
                            </td>
                            <td style="padding-left: 10px;">
                                <asp:Label ID="lblTaskCount" runat="server"> </asp:Label>
                            </td>

                        </tr>

                        <%--<tr>
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
                        </tr>--%>
                    </table>

                </div>
            </div>
        </div>
        <div class="main">
        </div>
        <div style="padding-bottom: 50px;"></div>
        <%-- Task Details --%>

        <div class="main">
            <div class="card bg-light mb-3" style="width: 60rem;">
                 <header>
                    <h4 style="color:#060c49;font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;font-style:italic;">Task</h4>
                </header>
                <div class="card-body">
                    <div class="col-6 col-md-4">
                        <asp:UpdatePanel runat="server" ID="upTAsk" UpdateMode="Always" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <asp:GridView ID="grdTask" runat="server" Width="650px" BorderStyle="None" HeaderStyle-BackColor="#060c49" HeaderStyle-ForeColor="White"
                                    AutoGenerateColumns="false">
                                    <Columns>

                                        <asp:TemplateField HeaderText="Task Details" ItemStyle-Width="350">
                                            <ItemTemplate>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblTaskDet" runat="server" Text='<%# Eval("TaskDescription") %>'></asp:Label>
                                                        </td>
                                                    </tr>

                                                </table>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TSKID" Visible="false" ItemStyle-Width="350">
                                            <ItemTemplate>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblTSKID" runat="server" Text='<%# Eval("TSKID") %>'></asp:Label>
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
                                                            <asp:Label ID="lblCID" runat="server" Text='<%# Eval("CID") %>'></asp:Label>
                                                        </td>
                                                    </tr>

                                                </table>

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Respond" ItemStyle-Width="350">
                                            <ItemTemplate>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkRow" runat="server" AutoPostBack="true" OnCheckedChanged="chkRow_CheckedChanged" />

                                                        </td>
                                                    </tr>
                                                </table>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Document" ItemStyle-Width="350">
                                            <ItemTemplate>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkDoc" runat="server" AutoPostBack="true" OnCheckedChanged="chkDoc_CheckedChanged" />

                                                        </td>
                                                    </tr>
                                                </table>

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <%-- Message Details --%>
        <div class="main">
            <div class="card bg-light mb-3" style="width: 60rem;">
                 <header>
                    <h4 style="color:#060c49;font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;font-style:italic;">Messages</h4>
                </header>
                <div class="card-body">
                    <div class="col-6 col-md-4">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Always" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>New Message :  
                                            <asp:TextBox runat="server" ID="txtSendComment"></asp:TextBox>
                                            <asp:Button ID="btnSendMsg" Style="padding-left: 10px;" runat="server" Text="Send" OnClick="btnSendMsg_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="grdMsg" runat="server" Width="650px" BorderStyle="None" HeaderStyle-BackColor="#060c49" HeaderStyle-ForeColor="White"
                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                <Columns>

                                                    <asp:TemplateField HeaderText="Messages" ItemStyle-Width="350">
                                                        <ItemTemplate>
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblMessages" runat="server" Text='<%# Eval("CommentDetails") %>'></asp:Label>
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
                                                <EmptyDataTemplate>No Record Available,select  "Respond"  in above Grid</EmptyDataTemplate>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <%-- Document Details --%>
        <div class="main">
            <div class="card bg-light mb-3" style="width: 60rem;">
                 <header>
                    <h4 style="color:#060c49;font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;font-style:italic;">Documents</h4>
                </header>
                <div class="card-body">
                    <div class="col-6 col-md-4">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Always" ChildrenAsTriggers="true">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnUpload" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="row">
                                    <table>
                                        <tr>
                                          
                                             <td> <asp:FileUpload ID="FileUploadDoc" runat="server" /></td>
                                             <td> <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" /></td>
                                        </tr>
                                    </table>                            
                                   
                                   

                                </div>
                                <div class="row">
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <asp:GridView ID="grdDocument" runat="server" Width="650px" BorderStyle="None" HeaderStyle-BackColor="#060c49" HeaderStyle-ForeColor="White"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Document">
                                                            <ItemTemplate>
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td>

                                                                            <asp:LinkButton ID="lnkDocumentName" runat="server" Text='<%# Eval("DocumentName") %>'></asp:LinkButton>

                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                    <EmptyDataTemplate>No Record Available,select  "Document"  in above Grid</EmptyDataTemplate>
                                                </asp:GridView>
                                        </tr>
                                    </table>
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
