<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="SmartPlaygroundWeb.test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            This is running
            <br />
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SmartPlaygroundConnectionString %>" SelectCommand="SELECT * FROM [test]"></asp:SqlDataSource>
            <br />
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1">
                <Columns>
                    <asp:BoundField DataField="Column1" HeaderText="Column1" SortExpression="Column1" />
                    <asp:BoundField DataField="test1" HeaderText="test1" SortExpression="test1" />
                    <asp:BoundField DataField="test2" HeaderText="test2" SortExpression="test2" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
