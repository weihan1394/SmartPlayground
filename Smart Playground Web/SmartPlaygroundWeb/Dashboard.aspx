<%@ Page Title="" Language="C#" MasterPageFile="~/default.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="SmartPlaygroundWeb.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Morris charts -->
    <link rel="stylesheet" href="Resource/bower_components/morris.js/morris.css">

    <script type="text/javascript">
        $(document).ready(function () {
            var hdn1 = document.getElementById('<%=lblHiddenKidId.ClientID%>').innerText;
            var hdn2 = document.getElementById('<%=lblHiddenSelectedDate.ClientID%>').innerText;

            var parameters = '{selectedKidId:\"' + hdn1 + '\", selectedDate:\"' + hdn2 + '\"}';
            console.log(parameters);

            // GAME1 Score
            $.ajax({
                type: 'POST',
                url: "Dashboard.aspx/GetGame1ScoreChartData",
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                data: parameters,
                success: function (result) {
                    new Morris.Line({
                        element: 'game1ScoreChart',
                        behaveLikeLine: true,
                        data: result.d,
                        xkey: 'w',
                        ykeys: ['x'],
                        labels: ['X'],
                        xLabels: "hour",
                        parseTime: false,
                        pointSize: 2,
                        hideHover: 'auto',
                        lineColors: ['#a0d0e0', '#3c8dbc', 'rgb(255, 117, 142)']
                    });
                },
                error: function (error) { alert(error.responseText); }
            });

            // GAME1 Wrong Color
            $.ajax({
                type: 'POST',
                url: "Dashboard.aspx/GetGame1WrongColorChartData",
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                data: parameters,
                success: function (result) {
                    new Morris.Bar({
                        element: 'game1WrongColorChart',
                        behaveLikeLine: true,
                        data: result.d,
                        xkey: 'w',
                        ykeys: ['x'],
                        labels: ['X'],
                        pointSize: 2,
                        hideHover: 'auto',
                        lineColors: ['#a0d0e0', '#3c8dbc', 'rgb(255, 117, 142)']
                    });
                },
                error: function (error) { alert(error.responseText); }
            });

            // GAME1 Board Hit
            $.ajax({
                type: 'POST',
                url: "Dashboard.aspx/GetGame1BoardHitChartData",
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                data: parameters,
                success: function (result) {
                    new Morris.Bar({
                        element: 'game1BoardHitChart',
                        behaveLikeLine: true,
                        data: result.d,
                        xkey: 'w',
                        ykeys: ['x'],
                        labels: ['X'],
                        pointSize: 2,
                        hideHover: 'auto',
                        lineColors: ['#a0d0e0', '#3c8dbc', 'rgb(255, 117, 142)']
                    });
                },
                error: function (error) { alert(error.responseText); }
            });

            // GAME2 Score
            $.ajax({
                type: 'POST',
                url: "Dashboard.aspx/GetGame2ScoreChartData",
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                data: parameters,
                success: function (result) {
                    new Morris.Line({
                        element: 'game2ScoreChart',
                        behaveLikeLine: true,
                        data: result.d,
                        xkey: 'w',
                        ykeys: ['x'],
                        labels: ['X'],
                        xLabels: "hour",
                        parseTime: false,
                        pointSize: 2,
                        hideHover: 'auto',
                        lineColors: ['#a0d0e0', '#3c8dbc', 'rgb(255, 117, 142)']
                    });
                },
                error: function (error) { alert(error.responseText); }
            });

            // GAME2 Wrong Color
            $.ajax({
                type: 'POST',
                url: "Dashboard.aspx/GetGame2MissHitChartData",
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                data: parameters,
                success: function (result) {
                    new Morris.Bar({
                        element: 'game2MissHitChart',
                        behaveLikeLine: true,
                        data: result.d,
                        xkey: 'w',
                        ykeys: ['x'],
                        labels: ['X'],
                        pointSize: 2,
                        hideHover: 'auto',
                        lineColors: ['#a0d0e0', '#3c8dbc', 'rgb(255, 117, 142)']
                    });
                },
                error: function (error) { alert(error.responseText); }
            });

            // GAME2 Power
            $.ajax({
                type: 'POST',
                url: "Dashboard.aspx/GetGame2PowerChartData",
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                data: parameters,
                success: function (result) {
                    new Morris.Bar({
                        element: 'game2BoardHitChart',
                        behaveLikeLine: true,
                        data: result.d,
                        xkey: 'w',
                        ykeys: ['x'],
                        labels: ['X'],
                        pointSize: 2,
                        hideHover: 'auto',
                        lineColors: ['#a0d0e0', '#3c8dbc', 'rgb(255, 117, 142)']
                    });
                },
                error: function (error) { alert(error.responseText); }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Dashboard
            <small>Personalize Information</small>
        </h1>
    </section>

    <!-- Main content -->
    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <!-- AREA CHART -->
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title"><i class="fa fa-tag"></i>Search Kids</h3>
                    </div>
                    <div class="box-body chart-responsive">
                        <div class="form-group">
                            <label>Email address</label>
                            <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-control col-lg-push-5" placeholder="test" Text="kid1"></asp:TextBox>
                        </div>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-block btn-primary btn-flat" Width="200px" OnClick="btnSearch_Click" />
                        <asp:DropDownList CssClass="form-control" ID="ddlDates" runat="server" Enabled="false" DataValueField="Value" DataTextField="Text" OnSelectedIndexChanged="ddlDates_SelectedIndexChanged" AutoPostBack="true" />
                        <asp:Label ID="lblHiddenKidId" runat="server" Style="display: none;"></asp:Label>
                        <asp:Label ID="lblHiddenSelectedDate" runat="server" Style="display: none;"></asp:Label>
                    </div>
                    <!-- /.box-body -->
                </div>
                <!-- /.box -->
            </div>
        </div>
        <!-- /.row -->

        <div class="row">
            <!-- DONUT CHART -->
            <div class="col-md-12">
                <div class="box box-danger">
                    <div class="box-header with-border">
                        <h3 class="box-title">Game 1</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>
                    <div class="box-body chart-responsive">
                        <div class="row">
                            <div class="col-md-2">
                                <h3>Highest Score</h3>
                                <h4>
                                    <asp:Label ID="lblGame1HighestScore" runat="server" Text="---"></asp:Label></h4>
                                <br />
                                <h3>Average Score</h3>
                                <h4>
                                    <asp:Label ID="lblGame1AverageScore" runat="server" Text="---"></asp:Label></h4>
                            </div>
                            <div class="col-md-10">
                                <p class="text-center">
                                    <strong>Score</strong>
                                </p>
                                <div class="chart" id="game1ScoreChart"></div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <p class="text-center">
                                    <strong>Wrong Color</strong>
                                </p>
                                <div class="chart" id="game1WrongColorChart"></div>
                            </div>
                            <div class="col-md-6">
                                <p class="text-center">
                                    <strong>Hit the Board</strong>
                                </p>
                                <div class="chart" id="game1BoardHitChart"></div>
                            </div>
                        </div>
                    </div>
                    <!-- /.box-body -->
                </div>
            </div>
            <!-- /.box -->
        </div>

        <div class="row">
            <!-- DONUT CHART -->
            <div class="col-md-12">
                <div class="box box-danger">
                    <div class="box-header with-border">
                        <h3 class="box-title">Game 2</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                    </div>
                    <div class="box-body chart-responsive">
                        <div class="row">
                            <div class="col-md-2">
                                <h3>Highest Score</h3>
                                <h4>
                                    <asp:Label ID="lblGame2HighestScore" runat="server" Text="---"></asp:Label></h4>
                                <br />
                                <h3>Average Score</h3>
                                <h4>
                                    <asp:Label ID="lblGame2AverageScore" runat="server" Text="---"></asp:Label></h4>
                            </div>
                            <div class="col-md-10">
                                <p class="text-center">
                                    <strong>Score</strong>
                                </p>
                                <div class="chart" id="game2ScoreChart"></div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <p class="text-center">
                                    <strong>Wrong Color</strong>
                                </p>
                                <div class="chart" id="game2MissHitChart"></div>
                            </div>
                            <div class="col-md-6">
                                <p class="text-center">
                                    <strong>Hit the Board</strong>
                                </p>
                                <div class="chart" id="game2BoardHitChart"></div>
                            </div>
                        </div>
                    </div>
                    <!-- /.box-body -->
                </div>
            </div>
            <!-- /.box -->
        </div>
    </section>
    <!-- /.content -->

    <!-- Morris.js charts -->
    <script src="Resource/bower_components/raphael/raphael.min.js"></script>
    <script src="Resource/bower_components/morris.js/morris.min.js"></script>
</asp:Content>
