<%@ Page Language="C#" MasterPageFile="~/CPanel/MasterPage.master" AutoEventWireup="true" CodeFile="School_AddEdit.aspx.cs" Inherits="CPanel_School_AddEdit" %>

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
                                        School Name  <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtSchoolName" runat="server" CssClass="form-control" placeholder="Enter school name." MaxLength="150" TabIndex="1" />
                                        <asp:RequiredFieldValidator ID="reqfld_SchoolName" runat="server" ControlToValidate="txtSchoolName" ErrorMessage="Please enter school name."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        EmailID <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtEmailID" runat="server" CssClass="form-control" placeholder="Enter emailid." MaxLength="150" TabIndex="2" />
                                        <asp:RequiredFieldValidator ID="reqfld_emailid" runat="server" ControlToValidate="txtEmailID" ErrorMessage="Please enter emailid."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Address <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtAddress" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control" placeholder="Enter address." MaxLength="250" TabIndex="3" />
                                        <asp:RequiredFieldValidator ID="reqfld_address" runat="server" ControlToValidate="txtAddress" ErrorMessage="Please enter address."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Phone No<span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtPhone1" runat="server" CssClass="form-control" placeholder="Enter phone no." MaxLength="15" TabIndex="4" />
                                        <asp:RequiredFieldValidator ID="reqfld_phone1" runat="server" ControlToValidate="txtPhone1" ErrorMessage="Please enter phone no."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Alternate Phone No 
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtPhone2" runat="server" CssClass="form-control" placeholder="Enter phone no." MaxLength="15" TabIndex="5" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        City<span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control" TabIndex="6" AutoPostBack="true" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged" />
                                        <asp:RequiredFieldValidator ID="reqfld_city" InitialValue="" runat="server" ControlToValidate="ddlCity" ErrorMessage="Please select city."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group"  id="dvRegion" runat="server" visible="false">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Region<span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:DropDownList ID="ddlRegion" runat="server" CssClass="form-control" TabIndex="7" />
                                        <asp:RequiredFieldValidator ID="reqfld_region" InitialValue="" runat="server" ControlToValidate="ddlRegion" ErrorMessage="Please select region."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        No of subscribers<span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-2 col-md-3 col-sm-3 col-xs-4">
                                        <asp:TextBox ID="txtNoofSubscribers" runat="server" CssClass="form-control" placeholder="Enter no of subscribers." MaxLength="30" TabIndex="8" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNoofSubscribers" ErrorMessage="Enter no of subscribers."
                                            Display="Dynamic" CssClass="text-danger" ValidationGroup="vg" EnableClientScript="true" />
                                    </div>
                                </div>
                                  <div class="row form-group">
                                    <div class="col-lg-5 col-md-5 col-sm-5 control-label">
                                        Status <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-4">
                                        <div class="radio radio-inline">
                                            <asp:RadioButton ID="rdbActive" runat="server" Text="Active" GroupName="Status" Checked="true" TabIndex="9" />&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:RadioButton ID="rdbInActive" runat="server" Text="Inactive" GroupName="Status" TabIndex="10" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <hr style="color: #ccc; background-color: #ccc; height: 1px; border: none; clear: both; margin: 10px 0;" />

                        <a href="/Cpanel/SchoolList.aspx" class="btn btn-secondary btn-sm" tabindex="11"><i class="fa fa-chevron-left"></i>&nbsp;Back</a>

                        <div class="pull-right" id="dvSubmit">
                            <asp:LinkButton ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" ValidationGroup="vg" CausesValidation="true" CssClass="btn btn-primary btn-sm icon-btnrt"
                                TabIndex="11">Submit&nbsp;<i class="fa fa-thumbs-up"></i></asp:LinkButton>
                        </div>
                        <div class="clear"></div>
                        <asp:CustomValidator ID="custVal" runat="server" ValidationGroup="vg" OnServerValidate="custVal_ServerValidate"></asp:CustomValidator>
                        <asp:HiddenField ID="hfSchoolID" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
