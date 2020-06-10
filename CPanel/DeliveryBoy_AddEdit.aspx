<%@ Page Language="C#" MasterPageFile="~/CPanel/MasterPage.master" AutoEventWireup="true" CodeFile="DeliveryBoy_AddEdit.aspx.cs" Inherits="CPanel_DeliveryBoy_AddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" />
        <div class="row">
            <div class="col-md-12 inBodyTop">
                <div class="card">
                    <div class="card-body">
                        <div align="center">
                            <span align="left" id="lblTitle" class="text-dark" runat="server"></span>
                        </div>

                        <div class="row mt-20">
                            <div class="alert alert-danger p-10 mb-15" id="dvError" runat="server"></div>
                            <div class="form-horizontal form-sm col-lg-12">
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        First Name  <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtFName" runat="server" CssClass="form-control" placeholder="Enter first name." MaxLength="50" TabIndex="1" />
                                        <asp:RequiredFieldValidator ID="reqfld_FName" runat="server" ControlToValidate="txtFName" ErrorMessage="Please enter first name."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Last Name <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtLName" runat="server" CssClass="form-control" placeholder="Enter last name." MaxLength="50" TabIndex="2" />
                                        <asp:RequiredFieldValidator ID="reqfld_lname" runat="server" ControlToValidate="txtLName" ErrorMessage="Please enter last name."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        User Name<span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" placeholder="Enter user name." MaxLength="50" TabIndex="3" />
                                        <asp:RequiredFieldValidator ID="reqfld_username" runat="server" ControlToValidate="txtUserName" ErrorMessage="Please enter user name."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Password<span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="Enter password." MaxLength="50" TabIndex="4" />
                                        <asp:RequiredFieldValidator ID="reqfld_password" runat="server" ControlToValidate="txtPassword" ErrorMessage="Please enter password."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Address <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtAddress" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control" placeholder="Enter address." MaxLength="250" TabIndex="5" />
                                        <asp:RequiredFieldValidator ID="reqfld_address" runat="server" ControlToValidate="txtAddress" ErrorMessage="Please enter address."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Email <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Enter email." MaxLength="120" TabIndex="6" />
                                        <asp:RequiredFieldValidator ID="reqfld_email" runat="server" ControlToValidate="txtEmail" ErrorMessage="Please enter email."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Phone No<span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtPhone1" runat="server" CssClass="form-control" placeholder="Enter phone no." MaxLength="15" TabIndex="7" />
                                        <asp:RequiredFieldValidator ID="reqfld_phone1" runat="server" ControlToValidate="txtPhone1" ErrorMessage="Please enter phone no."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Alternate Phone No 
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtPhone2" runat="server" CssClass="form-control" placeholder="Enter phone no." MaxLength="15" TabIndex="8" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        City<span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control" TabIndex="9" AutoPostBack="true" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged" />
                                        <asp:RequiredFieldValidator ID="reqfld_city" InitialValue="" runat="server" ControlToValidate="ddlCity" ErrorMessage="Please select city."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group" id="dvRegion" runat="server" visible="false">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Region<span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:DropDownList ID="ddlRegion" runat="server" CssClass="form-control" TabIndex="10" />
                                        <asp:RequiredFieldValidator ID="reqfld_region" InitialValue="" runat="server" ControlToValidate="ddlRegion" ErrorMessage="Please select region."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>

                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        License No<span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtLicenseNo" runat="server" CssClass="form-control" placeholder="Enter license no." MaxLength="30" TabIndex="11" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLicenseNo" ErrorMessage="Please enter license no."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Vehicle No 
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:DropDownList ID="ddlVehicle" runat="server" CssClass="form-control" TabIndex="12" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <hr style="color: #ccc; background-color: #ccc; height: 1px; border: none; clear: both; margin: 10px 0;" />

                        <a href="/Cpanel/DeliveryBoysList.aspx" class="btn btn-secondary btn-sm" tabindex="11"><i class="fa fa-chevron-left"></i>&nbsp;Back</a>

                        <div class="pull-right" id="dvSubmit">
                            <asp:LinkButton ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" ValidationGroup="vg" CausesValidation="true" CssClass="btn btn-primary btn-sm icon-btnrt"
                                TabIndex="8">Submit&nbsp;<i class="fa fa-thumbs-up"></i></asp:LinkButton>
                        </div>
                        <div class="clear"></div>
                        <asp:CustomValidator ID="custVal" runat="server" OnServerValidate="custVal_ServerValidate"></asp:CustomValidator>
                        <asp:HiddenField ID="hfDeliveryBoyID" runat="server" />

                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
