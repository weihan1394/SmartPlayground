<%@ Page Title="" Language="C#" MasterPageFile="~/default.Master" AutoEventWireup="true" CodeBehind="RegisterKid.aspx.cs" Inherits="SmartPlaygroundWeb.RegisterStudent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <section class="content">
        <div class="row">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                </Triggers>
                <ContentTemplate>
                    <div class="col-md-4">
                        <div class="box">
                            <div class="box-header with-border">
                                <h3 class="box-title"><i class="fa fa-tag"></i>    Search Kids</h3>

                                <div class="box-tools pull-right">
                                    <button runat="server" type="button" class="btn btn-box-tool" data-widget="collapse" data-toggle="tooltip" title="" data-original-title="Collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                            </div>
                            <div class="box-body " style="">
                                <%-- Email address --%>
                                <div class="form-group">
                                    <label>Email address</label>
                                    <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-control col-lg-push-5" placeholder="test" Text="kid1"></asp:TextBox>
                                </div>
                            </div>
                            <div class="box-footer">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-block btn-primary btn-flat" Width="200px" OnClick="btnSearch_Click" />
                            </div>
                        </div>


                        <div class="box">
                            <div class="box-header with-border">
                                <h3 class="box-title"><i class="fa fa-info-circle"></i>    Details</h3>

                                <div class="box-tools pull-right">
                                    <button runat="server" type="button" class="btn btn-box-tool" data-widget="collapse" data-toggle="tooltip" title="" data-original-title="Collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                            </div>
                            <div class="box-body" style="">
                                <asp:Image ID="imgProfilePic" runat="server" CssClass="profile-user-img img-responsive img-circle" ImageUrl="~/Resource/image/kid/default.jpg" alt="User profile picture" />
                                <h3 class="profile-username text-center">
                                    <asp:Label ID="lblChildName" runat="server" Text="" /></h3>
                                <p class="text-muted text-center">
                                    <asp:Label ID="lblChildMoreInfo" runat="server" Text="" />
                                </p>
                                <asp:Label ID="lblTimeStamp" runat="server" CssClass="text-muted text-center" Text="" />

                                <ul class="list-group list-group-unbordered">
                                    <li class="list-group-item">
                                        <b>ID</b> <a class="pull-right">
                                            <asp:Label ID="lblID" runat="server" Text="" /></a>
                                    </li>
                                    <li class="list-group-item">
                                        <b>Parent</b> <a class="pull-right">
                                            <asp:Label ID="lblParent" runat="server" Text="" /></a>
                                    </li>
                                    <li class="list-group-item">
                                        <b>Parent</b> <a class="pull-right">
                                            <asp:Label ID="lblParentContact" runat="server" Text="" /></a>
                                    </li>
                                    <li class="list-group-item">
                                        <b>Relationship</b> <a class="pull-right">
                                            <asp:Label ID="lblRelationship" runat="server" Text="" /></a>
                                    </li>
                                    <li class="list-group-item">
                                        <b>Last Visit</b> <a class="pull-right">
                                            <asp:Label ID="lblLastVisit" runat="server" Text="" /></a>
                                    </li>
                                    <li class="list-group-item">
                                        <b>Last Visit (End)</b> <a class="pull-right">
                                            <asp:Label ID="lblLastVisitEnd" runat="server" Text="" /></a>
                                    </li>
                                    <li class="list-group-item">
                                        <b>Registered</b> <a class="pull-right">
                                            <asp:Label ID="lblRegisteredDate" runat="server" Text="" /></a>
                                    </li>
                                    <li class="list-group-item">
                                        <b>Special Note</b> <a class="pull-right">
                                            <asp:Label ID="lblNote" runat="server" Text="" /></a>
                                    </li>
                                    <li class="list-group-item">
                                        <b><asp:Label ID="lblZone" runat="server" Text="" /></b> <a class="pull-right">
                                            <asp:Label ID="lblZoneValue" runat="server" Text="" /></a>
                                    </li>
                                </ul>
                                <div class="form-group">
                                    <asp:Label ID="lblTag" runat="server" Text="Tag:" CssClass="col-sm-2 control-label" Enabled="false" Visible="false"/>
                                    <asp:DropDownList CssClass="col-sm-10" ID="ddlRfidTag" runat="server" Enabled="false" Visible="false">
                                    </asp:DropDownList>
                                </div>

                            </div>
                            <div class="box-footer">
                                <div class="btn-group pull-right">
                                    <asp:Button ID="btnStart" runat="server" Text="Start" CssClass="btn btn-flat btn-primary" OnClick="btnStart_Click" Enabled="false" />
                                    <asp:Button ID="btnEnd" runat="server" Text="End" CssClass="btn btn-flat btn-warning" OnClick="btnEnd_Click" Enabled="false" />

                                </div>
                            </div>
                        </div>




                    </div>
                    <div class="col-md-8">
                        <div class="box">
                            <div class="box-header with-border">
                                <h3 class="box-title"><i class="fa fa-map-marker"></i>    Current Situation</h3>
                            </div>
                            <div class="box-body " style="">
                                <div class="row">
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="info-box">
                                            <span id="zone1Span" runat="server" class="info-box-icon bg-yellow"><i class="fa fa-child"></i></span>

                                            <div class="info-box-content">
                                                <span class="info-box-text">Zone 1</span>
                                                <span class="info-box-number">
                                                    <asp:Label ID="lblZone1" runat="server" Text=""></asp:Label>
                                                    <small>kids</small></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="info-box">
                                            <span id="zone2Span" runat="server" class="info-box-icon bg-aqua"><i class="fa fa-book"></i></span>

                                            <div class="info-box-content">
                                                <span class="info-box-text">Zone 2</span>
                                                <span class="info-box-number">
                                                    <asp:Label ID="lblZone2" runat="server" Text=""></asp:Label>
                                                    <small>kids</small></span>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="clearfix visible-sm-block"></div>

                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="info-box">
                                            <span id="zone3Span" runat="server" class="info-box-icon bg-yellow"><i class="fa fa-paint-brush"></i></span>

                                            <div class="info-box-content">
                                                <span class="info-box-text">Zone 3</span>
                                                <span class="info-box-number">
                                                    <asp:Label ID="lblZone3" runat="server" Text=""></asp:Label>
                                                    <small>kids</small></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="info-box">
                                            <span id="zone4Span" runat="server" class="info-box-icon bg-red"><i class="fa fa-cubes"></i></span>

                                            <div class="info-box-content">
                                                <span class="info-box-text">Zone 4</span>
                                                <span class="info-box-number">
                                                    <asp:Label ID="lblZone4" runat="server" Text=""></asp:Label>
                                                    <small>kids</small></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="nav-tabs-custom">
                                    <!-- Tabs within a box -->
                                    <ul class="nav nav-tabs pull-right ui-sortable-handle">
                                        <li class=""><a href="#zone4" data-toggle="tab" aria-expanded="false">Zone 4</a></li>
                                        <li class=""><a href="#zone3" data-toggle="tab" aria-expanded="true">Zone 3</a></li>
                                        <li class=""><a href="#zone2" data-toggle="tab" aria-expanded="false">Zone 2</a></li>
                                        <li class="active"><a href="#zone1" data-toggle="tab" aria-expanded="true">Zone 1</a></li>
                                        <li class="pull-left header"><i class="fa fa-inbox"></i>Crowd</li>
                                    </ul>
                                    <div class="tab-content no-padding">
                                        <!-- Morris chart - Sales -->
                                        <div class="chart tab-pane active" id="zone1" style="position: relative; min-height: 300px;">
                                            <div class="row docs-premium-template">
                                                <asp:Repeater ID="zone1Repeater" runat="server" DataSourceID="Zone1SqlDataSource">
                                                    <ItemTemplate>
                                                        <div class="col-sm-12 col-md-4">
                                                            <div class="box box-solid">
                                                                <div class="box-body">
                                                                    <h4 style="background-color: #f7f7f7; font-size: 18px; text-align: center; padding: 7px 10px; margin-top: 0;">
                                                                        <asp:Label ID="lblName" runat="server" Text='<%#Eval("name") %>' />
                                                                    </h4>
                                                                    <asp:Image runat="server" CssClass="profile-user-img img-responsive img-circle" ImageUrl='<%#Eval("profileImageUrl") %>' alt="User profile picture" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                        <div class="chart tab-pane" id="zone2" style="position: relative; height: 300px;">
                                            <asp:Repeater ID="zone2Repeater" runat="server" DataSourceID="Zone2SqlDataSource">
                                                <ItemTemplate>
                                                    <div class="col-sm-12 col-md-4">
                                                        <div class="box box-solid">
                                                            <div class="box-body">
                                                                <h4 style="background-color: #f7f7f7; font-size: 18px; text-align: center; padding: 7px 10px; margin-top: 0;">
                                                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("name") %>' />
                                                                </h4>
                                                                <asp:Image runat="server" CssClass="profile-user-img img-responsive img-circle" ImageUrl='<%#Eval("profileImageUrl") %>' alt="User profile picture" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                        <div class="chart tab-pane" id="zone3" style="position: relative; height: 300px;">
                                            <asp:Repeater ID="zone3Repeater" runat="server" DataSourceID="Zone3SqlDataSource">
                                                <ItemTemplate>
                                                    <div class="col-sm-12 col-md-4">
                                                        <div class="box box-solid">
                                                            <div class="box-body">
                                                                <h4 style="background-color: #f7f7f7; font-size: 18px; text-align: center; padding: 7px 10px; margin-top: 0;">
                                                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("name") %>' />
                                                                </h4>
                                                                <asp:Image runat="server" CssClass="profile-user-img img-responsive img-circle" ImageUrl='<%#Eval("profileImageUrl") %>' alt="User profile picture" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                        <div class="chart tab-pane" id="zone4" style="position: relative; height: 300px;">
                                            <asp:Repeater ID="zone4Repeater" runat="server" DataSourceID="Zone4SqlDataSource">
                                                <ItemTemplate>
                                                    <div class="col-sm-12 col-md-4">
                                                        <div class="box box-solid">
                                                            <div class="box-body">
                                                                <h4 style="background-color: #f7f7f7; font-size: 18px; text-align: center; padding: 7px 10px; margin-top: 0;">
                                                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("name") %>' />
                                                                </h4>
                                                                <asp:Image runat="server" CssClass="profile-user-img img-responsive img-circle" ImageUrl='<%#Eval("profileImageUrl") %>' alt="User profile picture" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <asp:Timer ID="Timer1" runat="server" Interval="10000" OnTick="Timer1_Tick"></asp:Timer>

        <asp:SqlDataSource ID="Zone1SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SmartPlaygroundConnectionString %>" SelectCommand="SELECT * FROM [User] WHERE ([zone] = @zone) AND (login = 1)">
            <SelectParameters>
                <asp:Parameter DefaultValue="1" Name="zone" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="Zone2SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SmartPlaygroundConnectionString %>" SelectCommand="SELECT * FROM [User] WHERE ([zone] = @zone) AND (login = 1)">
            <SelectParameters>
                <asp:Parameter DefaultValue="2" Name="zone" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="Zone3SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SmartPlaygroundConnectionString %>" SelectCommand="SELECT * FROM [User] WHERE ([zone] = @zone) AND (login = 1)">
            <SelectParameters>
                <asp:Parameter DefaultValue="3" Name="zone" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="Zone4SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SmartPlaygroundConnectionString %>" SelectCommand="SELECT * FROM [User] WHERE (zone = @zone) AND (login = 1)">
            <SelectParameters>
                <asp:Parameter DefaultValue="4" Name="zone" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    </section>

</asp:Content>
